namespace Xo.TaskTree.Abstractions;

public class MetaBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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
		this.Validate(metaNode);

		IAsyncFn fn = metaNode.ServiceType.ToFn(this._FnFactory, metaNode.NodeConfiguration.NextParamName);

		INode[] promisedArgs = metaNode.NodeConfiguration.MetaPromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		metaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INodeBuilder nb = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(metaNode.NodeConfiguration)
			.AddFn(fn);
		
		if(metaNode.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(metaNode.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			nb.AddNodeEdge(thenEdge);
		}

		return nb.Build();
	}

	public MetaBranchBuilder(
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
