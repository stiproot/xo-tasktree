namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public abstract class BaseNode : INode
{
	protected ILogger? _Logger;
	protected IAsyncFunctory? _AsyncFunctory;
	protected ISyncFunctory? _SyncFunctory;
	protected IWorkflowContext? _Context;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected INodeEdge? _NodeEdge;
	protected IController? _Controller;
	protected IInvoker _Invoker = new Invoker(new NodeEdgeResolver());
	protected INodevaluator _Nodevaluator = new ParallelNodeEvaluator();
	protected readonly IList<IMsg> _Params = new List<IMsg>();
	protected readonly List<INode> _PromisedParams = new List<INode>();
	protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	/// <inheritdoc />
	public string Id { get; internal set; } = $"{Guid.NewGuid()}";

	/// <inheritdoc />
	public INodeEdge? NodeEdge => this._NodeEdge;

	/// <inheritdoc />
	public bool HasParam(string paramName) => this._Params.Any(p => p.ParamName == paramName);

	/// <inheritdoc />
	public int ArgCount() => this._Params.Count() + this._PromisedParams.Count() + this._ContextParams.Count();

	/// <inheritdoc />
	public bool RequiresResult { get; internal set; }

	/// <inheritdoc />
	public IFunctory Functory => this._AsyncFunctory is not null ? (IFunctory)this._AsyncFunctory! : (IFunctory)this._SyncFunctory!;

	/// <inheritdoc />
	public bool IsSync => this._SyncFunctory != null;

	/// <inheritdoc />
	public INode SetNodeEdge(INodeEdge nodeEdge)
	{
		this._NodeEdge = nodeEdge ?? throw new ArgumentNullException(nameof(nodeEdge));
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
	public INode SetFunctory(IAsyncFunctory functory)
	{
		this._AsyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		return this;
	}

	/// <inheritdoc />
	public INode SetFunctory(Func<IReadOnlyList<IMsg>, Func<Task<IMsg?>>> fn)
	{
		this._AsyncFunctory = new AsyncFunctoryAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public INode SetFunctory(ISyncFunctory functory)
	{
		this._SyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		return this;
	}

	/// <inheritdoc />
	public INode SetFunctory(Func<IReadOnlyList<IMsg>, Func<IMsg?>> fn)
	{
		this._SyncFunctory = new SyncFunctoryAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public INode SetFunctory(Func<IWorkflowContext, Func<IMsg>> fn)
	{
		this._SyncFunctory = new SyncFunctoryAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public INode SetContext(IWorkflowContext? context)
	{
		this._Context = context;
		return this;
	}

	/// <inheritdoc />
	public INode SetId(string id)
	{
		this.Id = id;
		return this;
	}

	/// <inheritdoc />
	public INode SetLogger(ILogger logger)
	{
		this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		return this;
	}

	/// <inheritdoc />
	public INode AddArg(params INode[] nodes)
	{
		foreach (var h in nodes)
		{
			this._PromisedParams.Add(h);
		}
		return this;
	}

	/// <inheritdoc />
	public INode AddArg(params IMsg?[] msgs)
	{
		foreach (var m in msgs)
		{
			if(m is null)
			{
				continue;
			}
			this._Params.Add(m);
		}
		return this;
	}

	/// <inheritdoc />
	public INode AddArg<T>(
			T data,
			string paramName
	)
	{
		if (data is null || paramName is null) throw new InvalidOperationException("Null values cannot be passed into AddArg<T>...");

		// this._Params.Add(this._MsgFactory.Create<T>(data, paramName));
		this._Params.Add(SMsgFactory.Create<T>(data, paramName));

		return this;
	}

	/// <inheritdoc />
	public INode AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs)
	{
		foreach (var p in contextArgs)
		{
			this._ContextParams.Add(p);
		}

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
			return await this.ResolveFunctory(cancellationToken);
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

		if (this._AsyncFunctory is null && this._SyncFunctory is null)
		{
			this._Logger?.LogError($"Node validation failed.");
			throw new InvalidOperationException("Strategy factory has not been set.");
		}

		this._Logger?.LogError($"Node.Validate - end.");
	}

	/// <inheritdoc />
	public virtual async Task ResolvePromisedParams(CancellationToken cancellationToken)
	{
		this._Logger?.LogTrace($"Node.ResolvePromisedParams - running param nodes.");

		if (!this._PromisedParams.Any()) return;

		var results = await this._Nodevaluator.RunAsync(this._PromisedParams, cancellationToken);

		// todo: double check this logic...
		IEnumerable<IMsg> nonNullResults = results.Where(p => p is not null && p.HasParam).ToList()!;

		foreach (var r in nonNullResults)
		{
			this._Params.Add(r);
		}
	}

	/// <inheritdoc />
	public virtual void AddContextParamResultsToParams()
	{
		this._Logger?.LogTrace($"Node.AddContextParamResultsToParams - start.");

		// Are there any params that need to be extracted from the shared context?
		if (!this._ContextParams.Any())
		{
			return;
		}

		if (this._Context == null)
		{
			throw new InvalidOperationException("Context has not been provided");
		}

		foreach (var f in this._ContextParams)
		{
			this._Params.Add(f(this._Context));
		}

		this._Logger?.LogTrace($"Node.AddContextParamResultsToParams - end.");
	}

	/// <inheritdoc />
	public virtual async Task<IMsg?[]> ResolveFunctory(CancellationToken cancellationToken)
	{
		this._Logger?.LogTrace($"BaseNode.ResolveFunctory - starting...");

		// todo: remove guid...
		// var paramDic = this._Params.ToDictionary(p => p.ParamName ?? Guid.NewGuid().ToString());

		var result = this.IsSync
				? this._SyncFunctory!.CreateFunc(this._Params.AsReadOnly(), this._Context)()
				: await this._AsyncFunctory!.CreateFunc(this._Params.AsReadOnly(), this._Context)();

		if (result is not null && this._Context is not null)
		{
			this._Context.AddMsg(this.Id, result);
		}

		// todo: do we really want all these 'ToArray'? Should Functory not return an array?

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
	public virtual async Task HandleException(Exception ex)
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

	public INode SetInvoker(IInvoker invoker)
	{
		this._Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
		return this;
	}

	public INode SetController(IController controller)
	{
		this._Controller = controller ?? throw new ArgumentNullException(nameof(controller));

		return this;
	}

	/// <inheritdoc />
	public virtual INode RequireResult(bool requiresResult = true)
	{
		this.RequiresResult = requiresResult;
		return this;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public BaseNode(
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		this._Logger = logger;
		if (id is not null) this.Id = id;
		this._Context = context;
	}
}
