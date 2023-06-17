namespace Xo.TaskTree.Abstractions;

public interface IBinariusNodeEdge
{
    public INode? Edge1 { get; }
    public INode? Edge2 { get; }
}