namespace Xo.TaskTree.Abstractions;

public class MetaNodeMapper : IMetaNodeMapper
{
    protected readonly INodeBuilderFactory _NodeBuilderFactory = null!;

    public INode Map(IMetaNode source)
    {
        return source.NodeType switch
        {
            MetaNodeTypes.Binary => this.Create<IBinaryBranchBuilder>(source.NodeType).Build(),
            _ => throw new InvalidOperationException()
        };
    }

    private TBuilder Create<TBuilder>(MetaNodeTypes nodeType) where TBuilder : IBranchBuilder
    {
        throw new NotImplementedException();
    }
}
