namespace Xo.TaskTree.Core;

public class NodeEdgeResolver : INodeEdgeResolver
{
	public Task<IMsg?[]> Resolve(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	)
	{
		return nodeEdge.NodeEdgeType switch
		{
			NodeEdgeTypes.Monarius => ResolveMonariusNodeEdge(nodeEdge, msgs, cancellationToken),
			NodeEdgeTypes.Binarius => ResolveBinariusNodeEdge(nodeEdge, msgs, cancellationToken),
			NodeEdgeTypes.Multus => ResolveMultusNodeEdge(nodeEdge, msgs, cancellationToken),
			_ => throw new InvalidOperationException()
		};
	}

	private static async Task<IMsg?[]> ResolveMonariusNodeEdge(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	)
	{
		var edge = (nodeEdge as IMonariusNodeEdge)?.Edge;

		if(edge is null) throw new InvalidOperationException();

		if(edge.RequiresResult) edge.AddArg(msgs);

		return await edge.Run(cancellationToken);
	}

	private static async Task<IMsg?[]> ResolveBinariusNodeEdge(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	)
	{
		var binariusNodeEdge = (nodeEdge as IBinariusNodeEdge)!;

		var c = Task.WhenAll(
			binariusNodeEdge.Edge1!.AddArg(msgs).Run(cancellationToken),
			binariusNodeEdge.Edge2!.AddArg(msgs).Run(cancellationToken)
		);

		var r = await c;

		return r[0].Concat(r[1]).ToArray();
	}

	private static async Task<IMsg?[]> ResolveMultusNodeEdge(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	)
	{
		var multusNodeEdge = (nodeEdge as IMultusNodeEdge)!;

		// todo: non-nullable to-list extension method...

		var c = Task.WhenAll(multusNodeEdge.Edges.Select(e => e!.AddArg(msgs).Run(cancellationToken)));

		var r = await c;

		return r.SelectMany(a => a).ToArray();
	}
}