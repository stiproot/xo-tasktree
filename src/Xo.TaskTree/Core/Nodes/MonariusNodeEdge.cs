namespace Xo.TaskTree.Core;

public class MonariusNodeEdge : INodeEdge
{
    protected INode? _Edge;

    public INodeEdge Add(params INode[] nodes)
    {
        this._Edge = nodes[0];
        return this;
    }
}