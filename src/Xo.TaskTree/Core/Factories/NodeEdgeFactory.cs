namespace Xo.TaskTree.Core;

public static class NodeEdgeFactory
{
	public static IMonariusNodeEdge CreateMonarius(INode edge) 
		=> new MonariusNodeEdge(edge); 

	public static IBinariusNodeEdge CreateBinarius(
		INode? edge1,
		INode? edge2
	) 
		=> new BinariusNodeEdge(edge1, edge2); 

	public static IMultusNodeEdge CreateMultus(IList<INode> edges) 
		=> new MultusNodeEdge(edges); 
}
