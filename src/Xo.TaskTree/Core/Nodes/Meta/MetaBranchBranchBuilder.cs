namespace Xo.TaskTree.Abstractions;

public class MetaBranchBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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
		IAsyncFn fn = metaNode!.ServiceType.ToFn(this._FnFactory);
		INode[] promisedArgs = metaNode.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		metaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode[] ns = metaNode!.NodeEdge!.Nexts!.Select(v => this.BuildNext(metaNodeMapper, v)).ToArray();

		INodeEdge e = NodeEdgeFactory.Create(NodeEdgeTypes.Multus).Add(ns);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.AddFn(fn)
			.AddNodeEdge(e)
			.Build();

		return n;
	}

	protected INode BuildNext(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	)
	{
		if (mn is null) throw new InvalidOperationException();

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory);

		INode[] promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.AddFn(fn)
			.Build();

		return n;
	}

	public MetaBranchBranchBuilder(
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