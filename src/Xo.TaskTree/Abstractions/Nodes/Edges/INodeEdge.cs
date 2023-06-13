namespace Xo.TaskTree.Abstractions;

public interface INodeEdge
{
    INodeEdge Add(params INode[] nodes);
}