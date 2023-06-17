namespace Xo.TaskTree.Abstractions;

public class Invoker : IInvoker
{
	protected readonly IEdgeResolver _EdgeResolver;

	public Invoker(IEdgeResolver edgeResolver) => this._EdgeResolver = edgeResolver ?? throw new ArgumentNullException(nameof(edgeResolver));

	public Task<IMsg?> Invoke(IMsg? msg)
	{
		throw new NotImplementedException();
	}

	public Task<IMsg?> Invoke(
		INodeEdge nodeEdge,
		IMsg? msg
	)
	{
		throw new NotImplementedException();
	}
}