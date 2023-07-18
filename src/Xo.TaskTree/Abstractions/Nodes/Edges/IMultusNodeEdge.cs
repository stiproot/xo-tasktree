namespace Xo.TaskTree.Abstractions;

public interface IMultusNodeEdge
{
	IList<INode?> Edges { get; }
}