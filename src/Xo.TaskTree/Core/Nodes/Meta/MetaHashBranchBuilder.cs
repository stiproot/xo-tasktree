namespace Xo.TaskTree.Abstractions;

public class MetaHashBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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
		IAsyncFn fn = this._MetaNode!.ServiceType.ToFn(this._FnFactory, this._MetaNode.NodeConfiguration?.NextParamName);

		INode[] promisedArgs = this._MetaNode.NodeConfiguration!.MetaPromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		this._MetaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode[] decisions = this._MetaNode!.NodeEdge!.Nexts!.Select(v => this.BuildDecision(metaNodeMapper, v)).ToArray();

		INodeEdge e = new MultusNodeEdge { Edges = decisions };

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(this._MetaNode.NodeConfiguration)
			.AddFn(fn)
			.AddNodeEdge(e)
			.Build();

		return n;
	}

	protected INode BuildDecision(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory, mn.NodeConfiguration?.NextParamName);
		INode[] promisedArgs = mn.NodeConfiguration!.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(mn.NodeConfiguration)
			.AddFn(fn)
			.Build();
		
		if(mn.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		Func<IArgs, IMsg?> decisionFn = 
			(p) => SMsgFactory.Create<bool>(((p.First() as Msg<string>)!.GetData()).Equals(mn.NodeConfiguration!.Key));

		var decisionEdge = new MonariusNodeEdge().Add(n);

		var decisionNode  = this._NodeBuilderFactory
			.Create() 
			.Configure(c => c.RequireResult())
			.AddFn(decisionFn)
			.AddController(new TrueController())
			.AddNodeEdge(decisionEdge)
			.Build();
	
		return decisionNode;
	}

	public MetaHashBranchBuilder(
		INodeBuilderFactory nodeBuilderFactory,
		IFnFactory fnFactory,
		ILogger? logger = null,
		IWorkflowContext? context = null
	) : base(
			nodeBuilderFactory,
			fnFactory, 
			logger, 
			context
	)
	{
	}
}