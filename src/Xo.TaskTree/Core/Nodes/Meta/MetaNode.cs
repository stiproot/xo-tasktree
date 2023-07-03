namespace Xo.TaskTree.Core;

public class MetaNode : IMetaNode
{
    public Type FnType { get; init; }
    public MetaNodeTypes NodeType { get; set; } = MetaNodeTypes.Default;
    public IMetaNodeEdge? NodeEdge { get; set; }
    public INodeConfiguration NodeConfiguration { get; init; } = new NodeConfiguration();

    public MetaNode(Type functoryType) => this.FnType = functoryType;
}