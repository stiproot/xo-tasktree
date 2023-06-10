namespace Xo.TaskTree.Core;

/// <inheritdoc cref="INodevaluator"/>
public class LoopNodeEvaluator : INodevaluator
{
	/// <inheritdoc />
	public async Task<IList<IMsg?>> RunAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	)
	{
		var results = new List<IMsg?>();
		foreach (var node in nodes)
		{
			var result = await node.Run(cancellationToken);

			if (result is null) continue;

			results.Add(result);
		}
		return results;
	}
}
