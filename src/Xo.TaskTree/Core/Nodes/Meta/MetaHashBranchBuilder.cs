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
		IFn fn = metaNode!.ServiceType.ToFn(this._FnFactory, metaNode.NodeConfiguration.NextParamName);

		INode[] promisedArgs = metaNode.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		metaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode[] decisions = metaNode!.NodeEdge!.Nexts!.Select(v => this.BuildDecision(metaNodeMapper, v)).ToArray();

		INodeEdge e = NodeEdgeFactory.CreateMultus(decisions);

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

		IFn fn = mn.ServiceType.ToFn(this._FnFactory, mn.NodeConfiguration.NextParamName);
		INode[] promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INodeBuilder nb = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(mn.NodeConfiguration)
			.AddFn(fn);

		if (mn.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.CreateMonarius(thenNode);

			nb.AddNodeEdge(thenEdge);
		}

		Func<IArgs, IMsg?> decisionFn = DecisionFnFactory.Create(mn.NodeConfiguration.ControllerType, true, mn.NodeConfiguration.Key);

		var decisionEdge = NodeEdgeFactory.CreateMonarius(nb.Build());

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