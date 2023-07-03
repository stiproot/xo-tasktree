namespace Xo.TaskTree.Core;

public class MetaNodeMapper : IMetaNodeMapper
{
	protected readonly INodeBuilderFactory _NodeBuilderFactory;

	public MetaNodeMapper(INodeBuilderFactory nodeBuilderFactory)
			=> this._NodeBuilderFactory = nodeBuilderFactory ?? throw new ArgumentNullException(nameof(nodeBuilderFactory));

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

	private ICoreNodeBuilder Create(MetaNodeTypes nodeType)
	{
		return nodeType switch
		{
			MetaNodeTypes.Default => this._NodeBuilderFactory.Create(NodeBuilderTypes.DefaultMetaBranch),
			MetaNodeTypes.Binary => this._NodeBuilderFactory.Create(NodeBuilderTypes.BinaryMetaBranch),
			MetaNodeTypes.Hash => this._NodeBuilderFactory.Create(NodeBuilderTypes.HashMetaBranch),
			MetaNodeTypes.Branch => this._NodeBuilderFactory.Create(NodeBuilderTypes.BranchMetaBranch),
			MetaNodeTypes.Path => this._NodeBuilderFactory.Create(NodeBuilderTypes.PathMetaBranch),
			_ => throw new NotSupportedException()
		};
	}
}
