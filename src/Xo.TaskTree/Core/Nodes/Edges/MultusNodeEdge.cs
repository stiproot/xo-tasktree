namespace Xo.TaskTree.Core;

public class MultusNodeEdge : IMultusNodeEdge
{
	public NodeEdgeTypes NodeEdgeType => NodeEdgeTypes.Multus;
	public IList<INode?> Edges { get; internal set; } = new List<INode?>();

	public INodeEdge Add(params INode?[] nodes)
	{
		this.Edges = nodes.ToList();
		return this;
	}

	public MultusNodeEdge(IList<INode?> edges) => this.Edges = edges;
}