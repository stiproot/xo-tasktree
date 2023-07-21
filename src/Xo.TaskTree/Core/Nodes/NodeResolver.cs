namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public class NodeResolver : INodeResolver
{
	protected readonly IArgResolver _ArgResolver;
	protected readonly INodeEdgeResolver _EdgeResolver;

	/// <inheritdoc />
	public virtual async Task<IMsg[]> ResolveAsync(
		INode node,
		CancellationToken cancellationToken
	)
	{
		cancellationToken.ThrowIfCancellationRequested();

		node.Validate();

		IList<IMsg> results = await this._ArgResolver.ResolveAsync(node.NodeConfiguration.PromisedArgs, cancellationToken);

		if(node.NodeConfiguration.IgnoresPromisedResults is false)
		{
			foreach (var r in results)
			{
				node.NodeConfiguration.Args.Add(r);
			}
		}

		if(node.NodeConfiguration.WorkflowContext is not null)
		{
			foreach (var f in node.NodeConfiguration.ContextArgs)
			{
				node.NodeConfiguration.Args.Add(f(node.NodeConfiguration.WorkflowContext));
			}
		}

		try
		{
			var result = node.Fn.IsSync
				? node.Fn!.Invoke(node.NodeConfiguration.Args.AsArgs(), node.NodeConfiguration.WorkflowContext)
				: await node.Fn!.InvokeAsync(node.NodeConfiguration.Args.AsArgs(), node.NodeConfiguration.WorkflowContext);

			if (result is not null && node.NodeConfiguration.WorkflowContext is not null)
			{
				node.NodeConfiguration.WorkflowContext.AddMsg(node.NodeConfiguration.Id, result);
			}

			if(node.Controller is not null)
			{
				bool bit = node.Controller.Control(result?.ControlMsg);

				if(bit is false) return Array.Empty<IMsg>();
			}

			if(node.NodeEdge is not null)
			{
				return await this._EdgeResolver.Resolve(node.NodeEdge, result.ToArray(), cancellationToken);
			}

			return result.ToArray();
		}
		catch (Exception ex)
		{
			if (node.AsyncExceptionHandler is not null)
			{
				await node.AsyncExceptionHandler(ex);
			}

			if (node.ExceptionHandler is not null)
			{
				node.ExceptionHandler(ex);
			}

			throw;
		}
	}

	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public NodeResolver(
		IArgResolver argResolver,
		INodeEdgeResolver nodeEdgeResolver,
		ILogger? logger = null
	)
	{
		this._ArgResolver = argResolver ?? throw new ArgumentNullException(nameof(argResolver));
		this._EdgeResolver = nodeEdgeResolver ?? throw new ArgumentNullException(nameof(nodeEdgeResolver));
	}
}
