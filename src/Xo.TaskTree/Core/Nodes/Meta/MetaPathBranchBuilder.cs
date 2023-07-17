namespace Xo.TaskTree.Abstractions;

public class MetaPathBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
{
	public virtual IMetaBranchBuilder Validate(IMetaNode metaNode)
	{
		metaNode.ThrowIfNull();

		metaNode.NodeEdge.ThrowIfNull();

		metaNode.NodeEdge?.Next.ThrowIfNull();

		return this;
	}

	public INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode metaNode
	)
	{
		this.Validate(metaNode);

		return this.BuildPathStep(metaNodeMapper, metaNode);
	}

	protected INode BuildPathStep(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode mn
	)
	{
		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory);

		INode[] promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		INodeBuilder n = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(mn.NodeConfiguration)
			.AddFn(fn);

		if (mn.NodeEdge is not null) n.AddNodeEdge(NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(this.BuildPathStep(metaNodeMapper, mn.NodeEdge.Next!)));

		return n.Build();
	}

	public MetaPathBranchBuilder(
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