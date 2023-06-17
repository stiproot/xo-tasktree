namespace Xo.TaskTree.Core;

public class BinariusNodeEdge : INodeEdge, IBinariusNodeEdge
{
    public NodeEdgeTypes NodeEdgeType => NodeEdgeTypes.Binarius;
    public INode? Edge1 { get; internal set; }
    public INode? Edge2 { get; internal set; }

    public INodeEdge Add(params INode?[] nodes)
    {
        this.Edge1 = nodes[0];
        this.Edge2 = nodes[1];
        return this;
    }
}