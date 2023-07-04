namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public abstract class BaseMinNode : IMinNode
{
	protected ILogger? _Logger;
	protected IAsyncFn? _AsyncFn;
	protected ISyncFn? _SyncFn;
	// protected IWorkflowContext? _Context;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected INodeEdge? _NodeEdge;
	protected IMinNodeConfiguration _NodeConfiguration;
	protected IController? _Controller;
	protected IInvoker _Invoker = new Invoker(new NodeEdgeResolver());
	protected INodevaluator _Nodevaluator = new ParallelNodeEvaluator();

	// protected readonly IList<IMsg> _Params = new List<IMsg>();
	// protected readonly List<INode> _PromisedParams = new List<INode>();
	// protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	/// <inheritdoc />
	// public string Id { get; internal set; } = $"{Guid.NewGuid()}";

	/// <inheritdoc />
	public IMinNodeConfiguration NodeConfiguration => this._NodeConfiguration;

	/// <inheritdoc />
	public INodeEdge? NodeEdge => this._NodeEdge;
	/// <inheritdoc />
	public IFn Fn => this._AsyncFn is not null ? (IFn)this._AsyncFn! : (IFn)this._SyncFn!;

	/// <inheritdoc />
	// public bool HasParam(string paramName) => this._Params.Any(p => p.ParamName == paramName);

	/// <inheritdoc />
	public int ArgCount() => throw new NotImplementedException(); // this._Params.Count() + this._PromisedParams.Count() + this._ContextParams.Count();

	/// <inheritdoc />
	// public bool RequiresResult { get; internal set; } = false;

	// public bool IgnoresPromisedResults { get; internal set; } = false;


	/// <inheritdoc />
	protected bool _IsSync => this._SyncFn != null;

	/// <inheritdoc />
	public IMinNode SetNodeConfiguration(IMinNodeConfiguration nodeConfiguration)
	{
		this._NodeConfiguration = nodeConfiguration ?? throw new ArgumentNullException(nameof(nodeConfiguration));
		return this;
	}

	/// <inheritdoc />
	public IMinNode SetNodeEdge(INodeEdge nodeEdge)
	{
		this._NodeEdge = nodeEdge ?? throw new ArgumentNullException(nameof(nodeEdge));
		return this;
	}

	public IMinNode SetInvoker(IInvoker invoker)
	{
		this._Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
		return this;
	}

	public IMinNode SetController(IController controller)
	{
		this._Controller = controller ?? throw new ArgumentNullException(nameof(controller));

		return this;
	}

	/// <inheritdoc />
	public IMinNode SetNodevaluator(INodevaluator nodevaluator)
	{
		this._Nodevaluator = nodevaluator ?? throw new ArgumentNullException(nameof(nodevaluator));
		return this;
	}

	/// <inheritdoc />
	public IMinNode RunNodesInLoop()
	{
		this.SetNodevaluator(new LoopNodeEvaluator());
		return this;
	}

	/// <inheritdoc />
	public IMinNode SetFn(IAsyncFn fn)
	{
		this._AsyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	// /// <inheritdoc />
	// public INode SetFn(Func<IArgs, Task<IMsg?>> fn)
	// {
		// this._AsyncFn = new AsyncFnAdaptor(fn);
		// return this;
	// }

	/// <inheritdoc />
	public IMinNode SetFn(ISyncFn fn)
	{
		this._SyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	// /// <inheritdoc />
	// public INode SetFn(Func<IArgs, IMsg?> fn)
	// {
		// this._SyncFn = new SyncFnAdapter(fn);
		// return this;
	// }

	// /// <inheritdoc />
	// public INode SetFn(Func<IWorkflowContext, IMsg?> fn)
	// {
		// this._SyncFn = new SyncFnAdapter(fn);
		// return this;
	// }

	// /// <inheritdoc />
	// public INode SetContext(IWorkflowContext? context)
	// {
		// this._Context = context;
		// return this;
	// }

	// /// <inheritdoc />
	// public INode SetId(string id)
	// {
		// this.Id = id;
		// return this;
	// }

	// /// <inheritdoc />
	// public INode SetLogger(ILogger logger)
	// {
		// this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		// return this;
	// }

	///// <inheritdoc />
	//public INode AddArg(params INode[] nodes)
	//{
		//foreach (var h in nodes)
		//{
			//this._PromisedParams.Add(h);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg(params IMsg?[] msgs)
	//{
		//foreach (var m in msgs)
		//{
			//if(m is null)
			//{
				//continue;
			//}
			//this._Params.Add(m);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg<T>(
			//T data,
			//string paramName
	//)
	//{
		//if (data is null || paramName is null) throw new InvalidOperationException("Null values cannot be passed into AddArg<T>...");

		//// this._Params.Add(this._MsgFactory.Create<T>(data, paramName));
		//this._Params.Add(SMsgFactory.Create<T>(data, paramName));

		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs)
	//{
		//foreach (var p in contextArgs)
		//{
			//this._ContextParams.Add(p);
		//}

		//return this;
	//}

	/// <inheritdoc />
	public IMinNode SetExceptionHandler(Func<Exception, Task> handler)
	{
		this._ExceptionHandlerAsync = handler;

		return this;
	}

	/// <inheritdoc />
	public IMinNode SetExceptionHandler(Action<Exception> handler)
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

		// Are there any params that need to be extracted from the shared context?
		if (!this.NodeConfiguration.ContextArgs.Any())
		{
			return;
		}

		if (this.NodeConfiguration.WorkflowContext == null)
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

	///// <inheritdoc />
	//public virtual INode RequireResult(bool requiresResult = true)
	//{
		//this.RequiresResult = requiresResult;
		//return this;
	//}

	//public INode IgnorePromisedResults(bool ignorePromisedResults = true)
	//{
		//this.IgnoresPromisedResults = ignorePromisedResults;
		//return this;
	//}

	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public BaseMinNode(
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		this._Logger = logger;
		// if (id is not null) this.Id = id;
		// this._Context = context;
	}
}
