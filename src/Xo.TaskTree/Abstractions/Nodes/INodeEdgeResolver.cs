namespace Xo.TaskTree.Abstractions;

public interface INodeEdgeResolver
{
	Task<IMsg[]> Resolve(
		INodeEdge nodeEdge, 
		IMsg[] msgs,
		CancellationToken cancellationToken
	);
}