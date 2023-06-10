namespace Xo.TaskTree.Core;

public class BinariusNodeEdge : INodeEdge
{
    protected INode? _Edge1;
    protected INode? _Edge2;

    public INodeEdge Add(params INode[] nodes)
    {
        this._Edge1 = nodes[0];
        this._Edge2 = nodes[1];
        return this;
    }
}