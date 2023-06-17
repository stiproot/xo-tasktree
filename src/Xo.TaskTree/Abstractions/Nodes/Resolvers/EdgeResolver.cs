namespace Xo.TaskTree.Abstractions;

public class EdgeResolver : IEdgeResolver
{
	public Task<IMsg?[]> Resolve(
		INodeEdge nodeEdge,
		IMsg?[] msgs
	)
	{
		return nodeEdge.NodeEdgeType switch
		{
			NodeEdgeTypes.Monarius =>  
			NodeEdgeTypes.Binarius => throw new NotImplementedException(),
			NodeEdgeTypes.Multus => throw new NotImplementedException(),
			_ => throw new InvalidOperationException()
		};
	}
}