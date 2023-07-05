namespace Xo.TaskTree.Core;

/// <inheritdoc cref="INodevaluator"/>
public class ParallelNodeEvaluator : INodevaluator
{
	/// <inheritdoc />
	public async Task<IList<IMsg>> RunAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	)
	{
		IEnumerable<Task<IMsg[]>> promisedArgs = nodes.Select(p => p.Run(cancellationToken));

		var continuation = await Task.WhenAll(promisedArgs);

		var results = continuation.SelectMany(r => r).NonNull().ToList();

		return results;
	}
}
