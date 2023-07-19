namespace Xo.TaskTree.Core;

/// <inheritdoc cref="IArgResolver"/>
public class ParallelArgResolver : IArgResolver
{
	/// <inheritdoc />
	public async Task<IList<IMsg>> ResolveAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	)
	{
		IEnumerable<Task<IMsg[]>> promisedArgs = nodes.Select(p => p.Resolve(cancellationToken));

		var continuation = await Task.WhenAll(promisedArgs);

		var results = continuation.SelectMany(r => r).NonNull().ToList();

		return results;
	}
}
