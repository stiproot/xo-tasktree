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
	{
		return source.NodeType switch
		{
			MetaNodeTypes.Default => this.Create<IMetaBranchBuilder>(source.NodeType).Init(source).Build(this),
			MetaNodeTypes.Binary => this.Create<IMetaBinaryBranchBuilder>(source.NodeType).Init(source).Build(this),
			MetaNodeTypes.Hash => this.Create<IMetaHashBranchBuilder>(source.NodeType).Init(source).Build(this),
			MetaNodeTypes.Branch => this.Create<IMetaBinaryBranchBuilder>(source.NodeType).Init(source).Build(this),
			MetaNodeTypes.Path => this.Create<IMetaPathBranchBuilder>(source.NodeType).Init(source).Build(this),
			_ => throw new InvalidOperationException()
		};
	}

	private TBuilder Create<TBuilder>(MetaNodeTypes nodeType) => (TBuilder)this.Create(nodeType);

	private IMetaBranchBuilder Create(MetaNodeTypes nodeType)
	{
		return nodeType switch
		{
			MetaNodeTypes.Default => new MetaBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Binary => new MetaBinaryBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Hash => new MetaHashBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Branch => new MetaBranchBranchBuilder(this._NodeBuilderFactory, this._FnFactory),
			MetaNodeTypes.Path => new MetaPathBranchBuilder(this._NodeBuilderFactory, this._FnFactory),

			// MetaNodeTypes.Default => this._NodeBuilderFactory.Create(NodeBuilderTypes.DefaultMetaBranch),
			// MetaNodeTypes.Binary => this._NodeBuilderFactory.Create(NodeBuilderTypes.BinaryMetaBranch),
			// MetaNodeTypes.Hash => this._NodeBuilderFactory.Create(NodeBuilderTypes.HashMetaBranch),
			// MetaNodeTypes.Branch => this._NodeBuilderFactory.Create(NodeBuilderTypes.BranchMetaBranch),
			// MetaNodeTypes.Path => this._NodeBuilderFactory.Create(NodeBuilderTypes.PathMetaBranch),

			_ => throw new NotSupportedException()
		};
	}
}
