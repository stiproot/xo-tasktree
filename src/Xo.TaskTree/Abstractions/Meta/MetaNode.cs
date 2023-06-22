namespace Xo.TaskTree.Core;

public class MetaNode : IMetaNode
{
    public Type FunctoryType { get; init; }
    public MetaNodeTypes NodeType { get; set; }
    public IMetaNodeEdge? NodeEdge { get; set; }
    public List<IMetaNode> PromisedArgs { get; init; } = new();
    public List<IMsg> Args { get; init; } = new();
    public INodeConfiguration? NodeConfiguration { get; set; }

    public MetaNode(Type functoryType) => this.FunctoryType = functoryType;
}