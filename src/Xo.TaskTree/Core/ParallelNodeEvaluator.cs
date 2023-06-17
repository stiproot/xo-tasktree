namespace Xo.TaskTree.Core;

/// <inheritdoc cref="INodevaluator"/>

public class ParallelNodeEvaluator : INodevaluator
{
	/// <inheritdoc />
	public async Task<IList<IMsg?>> RunAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	)
	{
		var promisedArgs = nodes.Select(p => p.Run(cancellationToken));
		var continuation = Task.WhenAll(promisedArgs);
		var results = await continuation;
		return results;
	}
}
