namespace Xo.TaskTree.Abstractions;

public class MetaNodeMapper : IMetaNodeMapper
{
    public INode Map(IMetaNode source)
    {
        return source.NodeType switch
        {
            MetaNodeTypes.Binary => this.Create<IMetaBinaryBranchBuilder>(source.NodeType).Init(source).Build(),
            _ => throw new InvalidOperationException()
        };
    }

    private TBuilder Create<TBuilder>(MetaNodeTypes nodeType) where TBuilder : IBranchBuilder
    {
        // todo: implement builder factory...
        throw new NotImplementedException();
    }
}
