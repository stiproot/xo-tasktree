namespace Xo.TaskTree.Abstractions;

// todo: is this class necessary?
public class Invoker : IInvoker
{
	protected readonly INodeEdgeResolver _NodeEdgeResolver;

	public Invoker(INodeEdgeResolver nodeEdgeResolver) => this._NodeEdgeResolver = nodeEdgeResolver ?? throw new ArgumentNullException(nameof(nodeEdgeResolver));


	public Task<IMsg?[]> Invoke(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	)
	{
		return this._NodeEdgeResolver.Resolve(nodeEdge, msgs, cancellationToken);
	}
}