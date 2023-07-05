namespace Xo.TaskTree.Abstractions;

public class Invoker : IInvoker
{
	protected readonly INodeEdgeResolver _NodeEdgeResolver;

	public Invoker(INodeEdgeResolver nodeEdgeResolver) => this._NodeEdgeResolver = nodeEdgeResolver ?? throw new ArgumentNullException(nameof(nodeEdgeResolver));


	public Task<IMsg?[]> Invoke(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	) 
		=> this._NodeEdgeResolver.Resolve(nodeEdge, msgs, cancellationToken);
}