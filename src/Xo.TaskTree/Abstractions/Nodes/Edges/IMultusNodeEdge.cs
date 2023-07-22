namespace Xo.TaskTree.Abstractions;

public interface IMultusNodeEdge : INodeEdge
{
	IList<INode?> Edges { get; }
}