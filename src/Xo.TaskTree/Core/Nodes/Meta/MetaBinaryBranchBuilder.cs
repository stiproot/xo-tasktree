namespace Xo.TaskTree.Abstractions;

public class MetaBinaryBranchBuilder : CoreNodeBuilder, IMetaBinaryBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));

		return this;
	}

	public virtual IMetaBranchBuilder Validate()
	{
		this._MetaNode.ThrowIfNull();
		
		if(this._MetaNode!.NodeType is not MetaNodeTypes.Binary) throw new InvalidOperationException("Invalid meta node type.");

		this._MetaNode.NodeEdge.ThrowIfNull();

		if(this._MetaNode.NodeEdge!.True is null && this._MetaNode.NodeEdge!.False is null) throw new InvalidOperationException();

		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		this.Validate();

		INode? @true = this.BuildBinary(metaNodeMapper, this._MetaNode!.NodeEdge!.True, true);
		INode? @false = this.BuildBinary(metaNodeMapper, this._MetaNode!.NodeEdge!.False, false);
		INodeEdge e = new BinariusNodeEdge { Edge1 = @true, Edge2 = @false };

		IAsyncFunctory fn = this._MetaNode!.FunctoryType.ToFunctory(this._Functitect, this._MetaNode.NodeConfiguration?.NextParamName);
		INode[] promisedArgs = _MetaNode.NodeConfiguration!.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		INode n = this._NodeFactory
			.Create(this._Logger, context: this._Context)
			.SetFunctory(fn)
			.AddArg(this._MetaNode.NodeConfiguration.Args.ToArray())
			.AddArg(promisedArgs)
			.SetNodeEdge(e);
		
		if(this._MetaNode.NodeConfiguration?.RequiresResult is true)
		{
			n.RequireResult();
		}

		return n;
	}

	protected INode? BuildBinary(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn,
		bool binaryBranchType
	) 
	{
		if(mn is null) return null;

		IAsyncFunctory fn = mn.FunctoryType.ToFunctory(this._Functitect, mn.NodeConfiguration.NextParamName);
		INode[] promisedArgs = mn.NodeConfiguration!.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		INode n = this._NodeFactory
			.Create(this._Logger, context: this._Context)
			.SetFunctory(fn)
			.AddArg(promisedArgs);
		
		if(mn.NodeConfiguration is not null)
		{
			n.AddArg(mn.NodeConfiguration.Args.ToArray());
		}

		if(mn.NodeConfiguration?.RequiresResult is true)
		{
			n.RequireResult();
		}

		if(mn.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		Func<IArgs, Func<IMsg>> decisionFn = binaryBranchType
			? (p) => () => SMsgFactory.Create<bool>(((p.First() as Msg<bool>)!.GetData()) is true)
			: (p) => () => SMsgFactory.Create<bool>(((p.First() as Msg<bool>)!.GetData()) is false);

		var decisionEdge = new MonariusNodeEdge().Add(n);

		var decisionNode  = this._NodeFactory
			.Create() 
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge)
			.RequireResult();
	
		return decisionNode;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaBinaryBranchBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(
			functitect, 
			nodeFactory,
			logger, 
			id, 
			context
	)
	{
	}
}
