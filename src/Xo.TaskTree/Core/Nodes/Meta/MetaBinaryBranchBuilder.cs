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

		INode @true = this.BuildTrue(metaNodeMapper, this._MetaNode!.NodeEdge!.True);
		INode @false = this.BuildFalse(metaNodeMapper, this._MetaNode!.NodeEdge!.False);
		INodeEdge e = new BinariusNodeEdge { Edge1 = @true, Edge2 = @false };

		IAsyncFunctory fn = this._MetaNode!.FunctoryType.ToFunctory(this._Functitect, this._MetaNode.NodeConfiguration?.NextParamName);
		INode[] promisedArgs = this._MetaNode!.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		INode n = this._NodeFactory
			.Create(this._Logger, context: this._Context)
			.SetFunctory(fn)
			.AddArg(this._MetaNode.Args.ToArray())
			.AddArg(promisedArgs)
			.SetNodeEdge(e);

		return n;
	}

	protected INode BuildTrue(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		// mn is true metanode...
		if(mn is null) throw new InvalidOperationException();

		IAsyncFunctory fn = mn.FunctoryType.ToFunctory(this._Functitect, mn.NodeConfiguration?.NextParamName);
		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
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
			INode trueThenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge trueThenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(trueThenNode);

			n.SetNodeEdge(trueThenEdge);
		}

		// todo: this is ridiculous...
		Func<IArgs, Func<IMsg>> decisionFn = (p) => () => SMsgFactory.Create<bool>(((p.First() as Msg<bool>)!.GetData()) is true, "__");

		var decisionEdge = new MonariusNodeEdge().Add(n);

		var decisionNode  = this._NodeFactory
			.Create() 
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge)
			.RequireResult();
	
		return decisionNode;
	}

	// todo: this is duplication... not cool bro!
	protected INode BuildFalse(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFunctory fn = mn.FunctoryType.ToFunctory(this._Functitect, mn.NodeConfiguration?.NextParamName ?? "__");
		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		INode n = this._NodeFactory
			.Create(this._Logger, context: this._Context)
			.SetFunctory(fn)
			.AddArg(promisedArgs);

		if(mn.NodeConfiguration?.RequiresResult is true)
		{
			n.RequireResult();
		}


		// todo: this is ridiculous...
		Func<IArgs, Func<IMsg>> decisionFn = (p) => () => SMsgFactory.Create<bool>(((p.First() as Msg<bool>)!.GetData()) is false, "__");
		var decisionEdge = new MonariusNodeEdge().Add(n);
		var decisionNode = new Node()
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
