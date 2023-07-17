namespace Xo.TaskTree.Core;

public class MetaNodeMapper : IMetaNodeMapper
{
	protected readonly INodeBuilderFactory _NodeBuilderFactory;
	protected readonly IFnFactory _FnFactory;

	public MetaNodeMapper(
		INodeBuilderFactory nodeBuilderFactory,
		IFnFactory fnFactory

	)
	{
		this._NodeBuilderFactory = nodeBuilderFactory ?? throw new ArgumentNullException(nameof(nodeBuilderFactory));
		this._FnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
	}

	public INode Map(IMetaNode source)
		=> this.Create(source.NodeType).Build(this, source);

	private IMetaBranchBuilder Create(MetaNodeTypes nodeType)
	{
		return nodeType switch
		{
			MetaNodeTypes.Default => new MetaBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Binary => new MetaBinaryBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Hash => new MetaHashBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Branch => new MetaBranchBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Path => new MetaPathBranchBuilder(this._NodeBuilderFactory, this._FnFactory),

			_ => throw new NotSupportedException()
		};
	}
}
