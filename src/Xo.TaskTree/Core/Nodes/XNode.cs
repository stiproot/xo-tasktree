namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public class XNode : IXNode
{
	protected ILogger? _Logger;
	protected IAsyncFn? _AsyncFn;
	protected ISyncFn? _SyncFn;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected INodeEdge? _NodeEdge;
	protected INodeConfiguration? _NodeConfiguration;
	protected IController? _Controller;
	protected INodeEdgeResolver _Resolver = new NodeEdgeResolver();
	protected INodeEvaluator _Nodevaluator = new ParallelNodeEvaluator();

	public INodeConfiguration NodeConfiguration { get; init; }
	public INodeEdge? NodeEdge { get; init; }
	public INodeEdgeResolver NodeEdgeResolver { get; init; }
	public IController? Controller { get; init; }
	public INodeEvaluator NodeEvaluator { get; init; }
	public IAsyncFn? AsyncFn { get; set; }
	public ISyncFn SyncFn { get; init; }
	public Func<Exception, Task>? AsyncExceptionHandler { get; init; }
	public Func<Exception>? ExceptionHandler { get; init; }

	protected bool _IsSync => this._SyncFn != null;

	/// <inheritdoc />
	public virtual async Task<IMsg[]> Run(CancellationToken cancellationToken)
	{
		this._Logger?.LogTrace($"Node.Run - start.");

		cancellationToken.ThrowIfCancellationRequested();

		this.Validate();

		await this.ResolvePromisedParams(cancellationToken);

		this.AddContextParamResultsToParams();

		try
		{
			return await this.ResolveFn(cancellationToken);
		}
		catch (Exception ex)
		{
			await HandleException(ex);
			throw;
		}
	}

	/// <inheritdoc />
	public virtual void Validate()
	{
		this._Logger?.LogTrace($"Node.Validate - start.");

		if (this._AsyncFn is null && this._SyncFn is null)
		{
			this._Logger?.LogError($"Node validation failed.");
			throw new InvalidOperationException("Strategy factory has not been set.");
		}

		this._Logger?.LogError($"Node.Validate - end.");
	}

	/// <inheritdoc />
	protected virtual async Task ResolvePromisedParams(CancellationToken cancellationToken)
	{
		this._Logger?.LogTrace($"Node.ResolvePromisedParams - running param nodes.");

		if (!this.NodeConfiguration.PromisedArgs.Any()) return;

		IList<IMsg> results = await this._Nodevaluator.RunAsync(this.NodeConfiguration.PromisedArgs, cancellationToken);

		if(this.NodeConfiguration.IgnoresPromisedResults) return;

		foreach (var r in results)
		{
			this.NodeConfiguration.Args.Add(r);
		}
	}

	/// <inheritdoc />
	protected virtual void AddContextParamResultsToParams()
	{
		this._Logger?.LogTrace($"Node.AddContextParamResultsToParams - start.");

		// Are there any params that need to be extracted from the shared workflowContext?
		if (!this.NodeConfiguration.ContextArgs.Any())
		{
			return;
		}

		if (this.NodeConfiguration.WorkflowContext is null)
		{
			throw new InvalidOperationException("Context has not been provided");
		}

		foreach (var f in this.NodeConfiguration.ContextArgs)
		{
			this.NodeConfiguration.Args.Add(f(this.NodeConfiguration.WorkflowContext));
		}

		this._Logger?.LogTrace($"Node.AddContextParamResultsToParams - end.");
	}

	/// <inheritdoc />
	protected virtual async Task<IMsg[]> ResolveFn(CancellationToken cancellationToken)
	{
		this._Logger?.LogTrace($"BaseNode.ResolveFn - starting...");

		var result = this._IsSync
				? this._SyncFn!.Invoke(this.NodeConfiguration.Args.AsArgs(), this.NodeConfiguration.WorkflowContext)
				: await this._AsyncFn!.Invoke(this.NodeConfiguration.Args.AsArgs(), this.NodeConfiguration.WorkflowContext);

		if (result is not null && this.NodeConfiguration.WorkflowContext is not null)
		{
			this.NodeConfiguration.WorkflowContext.AddMsg(this.NodeConfiguration.Id, result);
		}

		if(this._Controller is not null)
		{
			bool bit = this._Controller.Control(result?.ControlMsg);

			if(bit is false) return Array.Empty<IMsg>();
		}

		if(this._NodeEdge is not null)
		{
			return await this._Resolver.Resolve(this._NodeEdge, result.ToArray(), cancellationToken);
		}

		return result.ToArray();
	}

	/// <inheritdoc />
	protected virtual async Task HandleException(Exception ex)
	{
		if (this._ExceptionHandlerAsync != null)
		{
			await this._ExceptionHandlerAsync(ex);
		}

		if (this._ExceptionHandler != null)
		{
			this._ExceptionHandler(ex);
		}
	}


	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public XNode(
		ILogger? logger = null,
		INodeConfiguration? nodeConfiguration = null
	)
	{
		this._Logger = logger;
		this._NodeConfiguration = nodeConfiguration;
	}
}
