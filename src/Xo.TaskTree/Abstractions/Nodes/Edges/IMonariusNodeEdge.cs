namespace Xo.TaskTree.Abstractions;

public interface IMonariusNodeEdge : INodeEdge
{
	public INode Edge { get; }
}