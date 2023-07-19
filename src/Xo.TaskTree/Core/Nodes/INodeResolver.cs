namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INodeResolver"/>
public interface INodeResolver
{
	/// <inheritdoc />
	Task<IMsg[]> ResolveAsync(
		INode node,
		CancellationToken cancellationToken
	);
}
