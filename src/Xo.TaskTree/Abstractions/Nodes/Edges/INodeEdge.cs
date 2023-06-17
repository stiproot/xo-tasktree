namespace Xo.TaskTree.Abstractions;

public interface INodeEdge
{
    NodeEdgeTypes NodeEdgeType { get; }
	INodeEdge Add(params INode?[] nodes);
}

public interface IMonariusNodeEdge
{
    public INode? Edge { get; }
}

public interface IBinariusNodeEdge
{
    public INode? Edge1 { get; }
    public INode? Edge2 { get; }
}

public interface IMultusNodeEdge
{
    public IList<INode?> Edges { get; }
}