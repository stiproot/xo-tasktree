namespace Xo.TaskTree.Core;

/// <inheritdoc cref="IArgResolver"/>
public class LoopArgResolver : IArgResolver
{
	/// <inheritdoc />
	public async Task<IList<IMsg>> ResolveAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	)
	{
		var results = new List<IMsg>();

		foreach (var node in nodes)
		{
			IMsg?[] result = await node.Resolve(cancellationToken);

			if (result is null) continue;

			results.AddRange(result.NonNull());
		}

		return results;
	}
}
