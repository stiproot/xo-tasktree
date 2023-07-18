namespace Xo.TaskTree.Core;

/// <inheritdoc cref="INodeEvaluator"/>
public class LoopNodeEvaluator : INodeEvaluator
{
	/// <inheritdoc />
	public async Task<IList<IMsg>> RunAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	)
	{
		var results = new List<IMsg>();

		foreach (var node in nodes)
		{
			IMsg?[] result = await node.Run(cancellationToken);

			if (result is null) continue;

			results.AddRange(result.NonNull());
		}

		return results;
	}
}
