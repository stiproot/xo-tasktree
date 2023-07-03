namespace Xo.TaskTree.Core;

public class MetaNode : IMetaNode
{
    public Type FunctoryType { get; init; }
    public MetaNodeTypes NodeType { get; set; } = MetaNodeTypes.Default;
    public IMetaNodeEdge? NodeEdge { get; set; }
    public INodeConfiguration NodeConfiguration { get; init; } = new NodeConfiguration();

    public MetaNode(Type functoryType) => this.FunctoryType = functoryType;
}