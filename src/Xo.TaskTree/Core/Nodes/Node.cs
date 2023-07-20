namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public class Node : INode
{
	// todo: !
	public INodeConfiguration NodeConfiguration { get; init; } = default!;
	public INodeResolver Resolver { get; init; } = default!;
	public INodeEdge? NodeEdge { get; init; }
	public IController? Controller { get; init; }
	public IFn Fn { get; init; } = default!;
	public Func<Exception, Task>? AsyncExceptionHandler { get; init; }
	public Action<Exception>? ExceptionHandler { get; init; }
	public bool IsSync => this.Fn is not null;

	/// <inheritdoc />
	public virtual async Task<IMsg[]> Resolve(CancellationToken cancellationToken)
		=> await this.Resolver.ResolveAsync(this, cancellationToken);

	/// <inheritdoc />
	public virtual void Validate()
	{
		if (this.Fn is null && this.Fn is null)
		{
			throw new InvalidOperationException("Strategy factory has not been set.");
		}
	}
}
