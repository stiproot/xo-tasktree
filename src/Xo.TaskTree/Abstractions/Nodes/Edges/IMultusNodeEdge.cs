namespace Xo.TaskTree.Abstractions;

public interface IMultusNodeEdge
{
    public IList<INode?> Edges { get; }
}