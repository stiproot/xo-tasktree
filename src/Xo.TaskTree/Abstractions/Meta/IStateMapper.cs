namespace Xo.TaskTree.Abstractions;

public interface ITypeMapper<TSource, TTarget>
{
    TTarget Map(TSource source);
}

public interface IMetaNodeMapper : ITypeMapper<IMetaNode, INode>
{
}

public class MetaNodeMapper : IMetaNodeMapper
{
    protected readonly INodeBuilderFactory _NodeBuilderFactory = null!;

    public INode Map(IMetaNode source)
    {
        var builder = source.NodeType switch 
        {
            MetaNodeTypes.Binary => this.Create<IBinaryBranchBuilder>(source.NodeType),
            _ => throw new InvalidOperationException()
        };

        throw new NotImplementedException();
    }

    private TBuilder Create<TBuilder>(MetaNodeTypes nodeType) where TBuilder : IBranchBuilder
    {
        throw new NotImplementedException();
    }
}
