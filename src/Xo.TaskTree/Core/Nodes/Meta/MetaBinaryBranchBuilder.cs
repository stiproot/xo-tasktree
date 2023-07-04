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

		IAsyncFn fn = this._MetaNode!.ServiceType.ToFn(this._FnFactory, this._MetaNode.NodeConfiguration?.NextParamName);
		INode[] promisedArgs = _MetaNode.NodeConfiguration!.MetaPromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		INode n = this._NodeFactory
			.Create(this._Logger, context: this._Context)
			.SetFn(fn)
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

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory, mn.NodeConfiguration.NextParamName);
		INode[] promisedArgs = mn.NodeConfiguration!.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		INode n = this._NodeFactory
			.Create(this._Logger, context: this._Context)
			.SetFn(fn)
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

		Func<IArgs, IMsg?> decisionFn = binaryBranchType
			? (p) => SMsgFactory.Create<bool>(((p.First() as Msg<bool>)!.GetData()) is true)
			: (p) => SMsgFactory.Create<bool>(((p.First() as Msg<bool>)!.GetData()) is false);

		var decisionEdge = new MonariusNodeEdge().Add(n);

		var decisionNode  = this._NodeFactory
			.Create() 
			.SetFn(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge)
			.RequireResult();
	
		return decisionNode;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaBinaryBranchBuilder(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(
			fnFactory, 
			nodeFactory,
			logger, 
			id, 
			context
	)
	{
	}
}
