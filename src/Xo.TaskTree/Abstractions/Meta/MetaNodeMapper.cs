namespace Xo.TaskTree.Abstractions;

public class MetaNodeMapper : IMetaNodeMapper
{
    protected readonly INodeBuilderFactory _NodeBuilderFactory;

    public INode Map(IMetaNode source)
    {
        return source.NodeType switch
        {
            MetaNodeTypes.Binary => this.Create<IMetaBinaryBranchBuilder>(source.NodeType).Init(source).Build(),
            _ => throw new InvalidOperationException()
        };
    }

    private TBuilder Create<TBuilder>(MetaNodeTypes nodeType) => (TBuilder)this.Create(nodeType);

    private INodeBuilder Create(MetaNodeTypes nodeType)
    {
        return nodeType switch
        {
            MetaNodeTypes.Binary => this._NodeBuilderFactory.Create(NodeTypes.Binary),
            _ => throw new NotSupportedException()
        };
    }
}
