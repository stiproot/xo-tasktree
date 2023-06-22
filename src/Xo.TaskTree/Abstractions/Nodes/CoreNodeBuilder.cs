namespace Xo.TaskTree.Abstractions;

public class CoreNodeBuilder : ICoreNodeBuilder
{
	protected ILogger? _Logger;
	protected IAsyncFunctory? _AsyncFunctory;
	protected ISyncFunctory? _SyncFunctory;
	protected IWorkflowContext? _Context;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;

	protected readonly IList<IMsg> _Params = new List<IMsg>();
	protected readonly List<INode> _PromisedParams = new List<INode>();
	protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	public string Id { get; internal set; } = Guid.NewGuid().ToString();
	public bool RequiresResult { get; internal set; } = false;

	/// <inheritdoc />
	public ICoreNodeBuilder RequireResult(bool requiresResult = true)
	{
		this.RequiresResult = requiresResult;
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddContext(IWorkflowContext? context)
	{
		this._Context = context;
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(IAsyncFunctory functory)
	{
		this._AsyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(ISyncFunctory functory)
	{
		this._SyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(Func<IDictionary<string, IMsg>, Func<Task<IMsg?>>> fn)
	{
		this._AsyncFunctory = new AsyncFunctoryAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(Func<IDictionary<string, IMsg>, Func<IMsg?>> fn)
	{
		this._SyncFunctory = new SyncFunctoryAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(Func<IWorkflowContext, Func<IMsg?>> fn)
	{
		this._SyncFunctory = new SyncFunctoryAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddArg(params IMsg[] msgs)
	{
		foreach (var m in msgs)
		{
			this._Params.Add(m);
		}
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddArg(params INode[] nodes)
	{
		foreach (var h in nodes)
		{
			this._PromisedParams.Add(h);
		}
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs)
	{
		foreach (var p in contextArgs)
		{
			this._ContextParams.Add(p);
		}
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder SetExceptionHandler(Func<Exception, Task> handler)
	{
		this._ExceptionHandlerAsync = handler;
		return this;
	}

	// /// <inheritdoc />
	public ICoreNodeBuilder SetExceptionHandler(Action<Exception> handler)
	{
		this._ExceptionHandler = handler;
		return this;
	}

	public virtual INode Build()
	{
		// todo: inject msg-factory?
		INode n = new Node(null, this._Logger, this.Id, this._Context);

		if (this._AsyncFunctory is not null) n.SetFunctory(this._AsyncFunctory);
		if (this._SyncFunctory is not null) n.SetFunctory(this._SyncFunctory);

		if (this._Params.Any()) n.AddArg(this._Params.ToArray());
		if (this._PromisedParams.Any()) n.AddArg(this._PromisedParams.ToArray());
		if (this._ContextParams.Any()) n.AddArg(this._ContextParams.ToArray());

		n.RequireResult(this.RequiresResult);

		if (this._ExceptionHandlerAsync is not null) n.SetExceptionHandler(this._ExceptionHandlerAsync);
		if (this._ExceptionHandler is not null) n.SetExceptionHandler(this._ExceptionHandler);

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BaseNodeBuilder"/>. 
	/// </summary>
	public CoreNodeBuilder(
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