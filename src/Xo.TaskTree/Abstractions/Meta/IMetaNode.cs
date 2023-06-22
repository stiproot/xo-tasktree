namespace Xo.TaskTree.Abstractions;

public interface IMetaNode
{
    Type FunctoryType { get; init; }
    MetaNodeTypes NodeType { get; set; }
    IMetaNodeEdge? NodeEdge { get; set; }
    List<IMetaNode> PromisedArgs { get; init; }
    List<IMsg> Args { get; init; }
    INodeConfiguration? NodeConfiguration { get; set; }
}