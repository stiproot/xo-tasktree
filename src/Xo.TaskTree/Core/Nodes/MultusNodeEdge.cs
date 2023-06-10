namespace Xo.TaskTree.Core;

public class MultusNodeEdge : INodeEdge
{
    protected List<INode>? _Edges;

    public INodeEdge Add(params INode[] nodes)
    {
        this._Edges = nodes.ToList();
        return this;
    }
}