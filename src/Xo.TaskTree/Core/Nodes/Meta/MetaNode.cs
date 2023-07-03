namespace Xo.TaskTree.Core;

public class MetaNode : IMetaNode
{
    public Type ServiceType { get; init; }
    public MetaNodeTypes NodeType { get; set; } = MetaNodeTypes.Default;
    public IMetaNodeEdge? NodeEdge { get; set; }
    public INodeConfiguration NodeConfiguration { get; init; } = new NodeConfiguration();

    public MetaNode(Type serviceType) => this.ServiceType = serviceType;
}