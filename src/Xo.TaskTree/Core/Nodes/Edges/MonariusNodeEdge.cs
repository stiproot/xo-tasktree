namespace Xo.TaskTree.Core;

public class MonariusNodeEdge : INodeEdge, IMonariusNodeEdge
{
	public NodeEdgeTypes NodeEdgeType => NodeEdgeTypes.Monarius;
	public INode? Edge { get; internal set; }

	public INodeEdge Add(params INode?[] nodes)
	{
		this.Edge = nodes[0];
		return this;
	}
}