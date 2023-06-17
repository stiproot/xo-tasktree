namespace Xo.TaskTree.Abstractions;

public interface IEdgeResolver
{
	Task<IMsg?[]> Resolve(
		INodeEdge nodeEdge, 
		IMsg?[] msgs
	);
}