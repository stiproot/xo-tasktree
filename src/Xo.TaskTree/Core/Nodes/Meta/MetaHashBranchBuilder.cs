namespace Xo.TaskTree.Abstractions;

public class MetaHashBranchBuilder : CoreNodeBuilder, IMetaHashBranchBuilder
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

		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		IAsyncFunctory fn = this._MetaNode!.FunctoryType.ToFunctory(this._Functitect, this._MetaNode.NodeConfiguration?.NextParamName);

		INode n = this._NodeFactory.Create(this._Logger, this.Id, this._Context);

		INode[] promisedArgs = this._MetaNode.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();

		INode[] decisions = this._MetaNode!.NodeEdge!.Nexts!.Select(v => this.BuildDecision(metaNodeMapper, v)).ToArray();

		INodeEdge e = new MultusNodeEdge { Edges = decisions };

		n
			.SetFunctory(fn)
			.AddArg(this._MetaNode.Args.ToArray())
			.AddArg(promisedArgs)
			.SetNodeEdge(e);

		if(this._MetaNode.NodeConfiguration?.RequiresResult is true)
		{
			n.RequireResult();
		}

		return n;
	}

	protected INode BuildDecision(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
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
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		Func<IArgs, Func<IMsg>> decisionFn = 
			(p) => 
				() => SMsgFactory.Create<bool>(((p.First() as Msg<string>)!.GetData()).Equals(mn.NodeConfiguration!.Key));

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
	public MetaHashBranchBuilder(
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