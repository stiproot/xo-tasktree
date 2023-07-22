namespace Xo.TaskTree.Core;

public class MonariusNodeEdge : IMonariusNodeEdge
{
	public NodeEdgeTypes NodeEdgeType => NodeEdgeTypes.Monarius;
	public INode Edge { get; internal set; }

	public MonariusNodeEdge(INode edge) => this.Edge = edge;
}