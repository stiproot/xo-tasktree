namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public class Node : INode
{
	protected ILogger? _Logger;
	protected IAsyncFn? _AsyncFn;
	protected ISyncFn? _SyncFn;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected INodeEdge? _NodeEdge;
	protected INodeConfiguration? _NodeConfiguration;
	protected IController? _Controller;
	protected IInvoker _Invoker = new Invoker(new NodeEdgeResolver());
	protected INodevaluator _Nodevaluator = new ParallelNodeEvaluator();

	/// <inheritdoc />
	public INodeConfiguration NodeConfiguration => this._NodeConfiguration!;

	/// <inheritdoc />
	public INodeEdge? NodeEdge => this._NodeEdge;
	/// <inheritdoc />
	public IFn Fn => this._AsyncFn is not null ? (IFn)this._AsyncFn! : (IFn)this._SyncFn!;

	/// <inheritdoc />
	public int ArgCount() => throw new NotImplementedException();

	/// <inheritdoc />
	protected bool _IsSync => this._SyncFn != null;

	/// <inheritdoc />
	public INode SetNodeConfiguration(INodeConfiguration nodeConfiguration)
	{
		this._NodeConfiguration = nodeConfiguration ?? throw new ArgumentNullException(nameof(nodeConfiguration));
		return this;
	}

	/// <inheritdoc />
	public INode SetNodeEdge(INodeEdge? nodeEdge)
	{
		this._NodeEdge = nodeEdge;
		return this;
	}

	public INode SetInvoker(IInvoker invoker)
	{
		this._Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
		return this;
	}

	public INode SetController(IController? controller)
	{
		this._Controller = controller;

		return this;
	}

	/// <inheritdoc />
	public INode SetNodevaluator(INodevaluator nodevaluator)
	{
		this._Nodevaluator = nodevaluator ?? throw new ArgumentNullException(nameof(nodevaluator));
		return this;
	}

	/// <inheritdoc />
	public INode RunNodesInLoop()
	{
		this.SetNodevaluator(new LoopNodeEvaluator());
		return this;
	}

	/// <inheritdoc />
	public INode SetFn(IAsyncFn fn)
	{
		this._AsyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public INode SetFn(ISyncFn fn)
	{
		this._SyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public INode SetExceptionHandler(Func<Exception, Task> handler)
	{
		this._ExceptionHandlerAsync = handler;

		return this;
	}

	/// <inheritdoc />
	public INode SetExceptionHandler(Action<Exception> handler)
	{
		this._ExceptionHandler = handler;

		return this;
	}

	/// <inheritdoc />
	public virtual async Task<IMsg?[]> Run(CancellationToken cancellationToken)
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

		var results = await this._Nodevaluator.RunAsync(this.NodeConfiguration.PromisedArgs, cancellationToken);

		IEnumerable<IMsg> nonNullResults = results.Where(p => p is not null).ToList()!;

		if(this.NodeConfiguration.IgnoresPromisedResults) return;

		foreach (var r in nonNullResults)
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
	protected virtual async Task<IMsg?[]> ResolveFn(CancellationToken cancellationToken)
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
			var bit = this._Controller.Control(result);

			if(bit is false) return result.ToArray();
		}

		if(this._NodeEdge is not null)
		{
			return await this._Invoker.Invoke(this._NodeEdge, result.ToArray(), cancellationToken);
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
	public Node(
		ILogger? logger = null,
		INodeConfiguration? nodeConfiguration = null
	)
	{
		this._Logger = logger;
		this._NodeConfiguration = nodeConfiguration;
	}
}
