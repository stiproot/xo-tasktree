namespace Xo.TaskTree.Abstractions;

public class MetaHashBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
{
	public virtual IMetaBranchBuilder Validate(IMetaNode metaNode)
	{
		metaNode.ThrowIfNull();

		return this;
	}

	public INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode metaNode
	)
	{
		IAsyncFn fn = metaNode!.ServiceType.ToFn(this._FnFactory, metaNode.NodeConfiguration.NextParamName);

		INode[] promisedArgs = metaNode.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		metaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode[] decisions = metaNode!.NodeEdge!.Nexts!.Select(v => this.BuildDecision(metaNodeMapper, v)).ToArray();

		INodeEdge e = NodeEdgeFactory.Create(NodeEdgeTypes.Multus).Add(decisions);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(metaNode.NodeConfiguration)
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
		if (mn is null) throw new InvalidOperationException();

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory, mn.NodeConfiguration.NextParamName);
		INode[] promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(mn.NodeConfiguration)
			.AddFn(fn)
			.Build();

		if (mn.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		Func<IArgs, IMsg?> decisionFn = DecisionFnFactory.Create(mn.NodeConfiguration.ControllerType, true, mn.NodeConfiguration.Key);

		var decisionEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(n);

		var decisionNode = this._NodeBuilderFactory
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
		IWorkflowContext? workflowContext = null
	) : base(
		nodeBuilderFactory,
		fnFactory,
		logger,
		workflowContext
	)
	{
	}
}