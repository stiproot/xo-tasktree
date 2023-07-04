namespace Xo.TaskTree.Abstractions;

public class MetaBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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
		IAsyncFn fn = this._MetaNode!.ServiceType.ToFn(this._FnFactory, this._MetaNode!.NodeConfiguration?.NextParamName);

		INode[] promisedArgs = this._MetaNode.NodeConfiguration!.MetaPromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		this._MetaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger, this._WorkflowContext)
			.Configure(this._MetaNode.NodeConfiguration)
			.AddFn(fn)
			.Build();
		
		if(this._MetaNode.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(this._MetaNode.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		return n;
	}

	public MetaBranchBuilder(
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
