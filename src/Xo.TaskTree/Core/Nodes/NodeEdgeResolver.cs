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
		var edge1 = (nodeEdge as IBinariusNodeEdge)?.Edge1;
		var edge2 = (nodeEdge as IBinariusNodeEdge)?.Edge2;

		if(edge1 is null && edge2 is null) throw new InvalidOperationException();

		if(edge1 is not null && edge1.RequiresResult)
		{
			// todo: match param here... match and clone...

			if(edge1.ArgCount() is 0)
			{
				if(msgs.Length is 1 && edge1.Functory.ServiceType is not null)
				{
					var paramName = edge1.Functory.ServiceType!
						.GetMethods()
						.First()
						.GetParameters()
						.First()
						.Name;
					
					msgs[0]!.SetParam(paramName!);
				}
				edge1.AddArg(msgs);
			}
			else
			{
				edge1.AddArg(msgs);
			}
		}
		
		if(edge2 is not null && edge2.RequiresResult) edge2.AddArg(msgs);

		// todo: in the case of a binary node, we actually do not want the "false" response, just the "true-path" result.
		if(edge1 is not null && edge2 is not null)
		{
			var c = Task.WhenAll(
				edge1!.Run(cancellationToken),
				edge2!.Run(cancellationToken)
			);

			var r = await c;

			// todo: do something about this...
			return r[0].Concat(r[1]).ToArray();
		}

		if(edge1 is not null) return await edge1!.Run(cancellationToken);

		return await edge2!.Run(cancellationToken);
	}

	private static async Task<IMsg?[]> ResolveMultusNodeEdge(
		INodeEdge nodeEdge,
		IMsg?[] msgs,
		CancellationToken cancellationToken
	)
	{
		var edges = (nodeEdge as IMultusNodeEdge)?.Edges;

		if(edges is null || edges.Any() is false) throw new InvalidOperationException();

		foreach(var e in edges)
		{
			if(e is not null && e.RequiresResult) e.AddArg(msgs);
		}

		var c = Task.WhenAll(edges.Select(e => e!.AddArg(msgs).Run(cancellationToken)));

		var r = await c;

		return r.SelectMany(a => a).ToArray();
	}
}