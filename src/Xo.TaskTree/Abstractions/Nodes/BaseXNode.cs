namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="INode"/>
public abstract class BaseXNode : IXNode
{
	protected ILogger? _Logger;
	protected readonly IMsgFactory _MsgFactory;

	protected IAsyncFunctory? _AsyncFunctory;
	protected ISyncFunctory? _SyncFunctory;
	protected IWorkflowContext? _Context;

	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;

	protected INodeEdge? _NodeEdge;
	protected IController? _Controller;
	protected IInvoker? _Invoker;
	protected INodevaluator _Nodevaluator = new ParallelNodeEvaluator();

	protected readonly IList<IMsg> _Params = new List<IMsg>();
	protected readonly List<INode> _PromisedParams = new List<INode>();
	protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	/// <inheritdoc />
	public string Id { get; init; } = $"{Guid.NewGuid()}";
	public bool RequiresResult { get; init; } = false;
	public bool IsSync => this._SyncFunctory != null;
	public IFunctory Functory => this._AsyncFunctory is not null ? (IFunctory)this._AsyncFunctory! : (IFunctory)this._SyncFunctory!;

	/// <inheritdoc />
	public virtual async Task<IMsg?[]> RunAsync(CancellationToken cancellationToken)
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

		// Are there any async operations, in the form of INodes, that need to run in order to provide params to our functory?
		if (!this._PromisedParams.Any()) return;

		var results = await this._Nodevaluator.RunAsync(this._PromisedParams, cancellationToken);

		// We are only interested in adding non-null results to our params 
		// If the result (IMsg) is null the Task was void.
		IEnumerable<IMsg> nonNullResults = results.Where(p => p is not null && p.HasParam).ToList()!;

		// Let's add the results to our list of params, for our functory.
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

		var paramDic = this._Params.ToDictionary(p => p.ParamName!);

		var result = this.IsSync
				? this._SyncFunctory!.CreateFunc(paramDic, this._Context)()
				: await this._AsyncFunctory!.CreateFunc(paramDic, this._Context)();

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
			return await this._Invoker!.Invoke(this._NodeEdge, result.ToArray(), cancellationToken);
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

	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public BaseXNode(
			IMsgFactory msgFactory,
			ILogger? logger = null,
			string? id = null,
			IWorkflowContext? context = null
	)
	{
		this._MsgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
		this._Logger = logger;

		if (id is not null) this.Id = id;
		this._Context = context;
	}
}
