namespace Xo.TaskTree.Abstractions;

public interface INodeEdge
{
	NodeEdgeTypes NodeEdgeType { get; }
	INodeEdge Add(params INode?[] nodes);
}