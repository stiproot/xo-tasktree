namespace Xo.TaskTree.Abstractions;

public class MetaPathBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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

		this._MetaNode!.NodeEdge.ThrowIfNull();

		this._MetaNode!.NodeEdge!.Next.ThrowIfNull();

		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		this.Validate();

		return this.Build(metaNodeMapper, this._MetaNode!);
	}

	protected INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode mn
	) 
	{
		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory);

		INode[] promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		ICoreNodeBuilder n = this._NodeBuilderFactory
			.Create(this._Logger)
			.Configure(mn.NodeConfiguration)
			.AddFn(fn);

		if(mn.NodeEdge is not null) n.AddNodeEdge(new MonariusNodeEdge { Edge = this.Build(metaNodeMapper, mn.NodeEdge.Next!)});
	
		return n.Build();
	}

	public MetaPathBranchBuilder(
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