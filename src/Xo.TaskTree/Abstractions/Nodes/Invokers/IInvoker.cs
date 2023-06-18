namespace Xo.TaskTree.Abstractions;

public interface IInvoker
{
	Task<IMsg?[]> Invoke(
		INodeEdge nodeEdge, 
		IMsg?[] msgs,
		CancellationToken cancellationToken
	);
}