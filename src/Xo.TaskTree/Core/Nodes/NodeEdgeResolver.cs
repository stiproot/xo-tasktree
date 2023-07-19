namespace Xo.TaskTree.Core;

public class NodeEdgeResolver : INodeEdgeResolver
{
	public Task<IMsg[]> Resolve(
		INodeEdge nodeEdge,
		IMsg[] msgs,
		CancellationToken cancellationToken
	)
	{
		return nodeEdge.NodeEdgeType switch
		{
			NodeEdgeTypes.Monarius => ResolveMonariusNodeEdge(nodeEdge, msgs, cancellationToken),
			NodeEdgeTypes.Binarius => ResolveBinariusNodeEdge(nodeEdge, msgs, cancellationToken),
			NodeEdgeTypes.Multus => ResolveMultusNodeEdge(nodeEdge, msgs, cancellationToken),
			_ => throw new NotSupportedException()
		};
	}

	private static async Task<IMsg[]> ResolveMonariusNodeEdge(
		INodeEdge nodeEdge,
		IMsg[] msgs,
		CancellationToken cancellationToken
	)
	{
		var edge = (nodeEdge as IMonariusNodeEdge)?.Edge;

		if(edge is null) throw new InvalidOperationException();

		if(edge.NodeConfiguration.RequiresResult) edge.AddArg(msgs);

		return await edge.Resolve(cancellationToken);
	}

	private static async Task<IMsg[]> ResolveBinariusNodeEdge(
		INodeEdge nodeEdge,
		IMsg[] msgs,
		CancellationToken cancellationToken
	)
	{
		var edge1 = (nodeEdge as IBinariusNodeEdge)?.Edge1;
		var edge2 = (nodeEdge as IBinariusNodeEdge)?.Edge2;

		if(edge1 is null && edge2 is null) throw new InvalidOperationException();

		if(edge1 is not null && edge1.NodeConfiguration.RequiresResult) edge1.AddArg(msgs);
		if(edge2 is not null && edge2.NodeConfiguration.RequiresResult) edge2.AddArg(msgs);

		if(edge1 is not null && edge2 is not null)
		{
			var c = Task.WhenAll(
				edge1!.Resolve(cancellationToken),
				edge2!.Resolve(cancellationToken)
			);

			var r = await c;

			// todo: do something about this...
			return r[0].Concat(r[1]).ToArray();
		}

		if(edge1 is not null) return await edge1!.Resolve(cancellationToken);

		return await edge2!.Resolve(cancellationToken);
	}

	private static async Task<IMsg[]> ResolveMultusNodeEdge(
		INodeEdge nodeEdge,
		IMsg[] msgs,
		CancellationToken cancellationToken
	)
	{
		var edges = (nodeEdge as IMultusNodeEdge)?.Edges;

		if(edges is null || edges.Any() is false) throw new InvalidOperationException();

		foreach(var e in edges)
		{
			if(e is not null && e.NodeConfiguration.RequiresResult) e.AddArg(msgs);
		}

		var c = Task.WhenAll(edges.Select(e => e!.AddArg(msgs).Resolve(cancellationToken)));

		var r = await c;

		return r.SelectMany(a => a).ToArray();
	}
}