namespace Xo.TaskTree.Abstractions;

public abstract class CoreNodeBuilder : BaseNodeBuilder, ICoreNodeBuilder
{
	/// <inheritdoc />
	public virtual bool HasParam(string paramName) => this._Params.Any(p => p.ParamName == paramName);
	public virtual IFunctitect Functitect => this._Functitect;

	/// <inheritdoc />
	public Type? FunctoryType
	{
		get
		{
			if (this._AsyncFunctory is not null) return (this._AsyncFunctory as IFunctoryInvoker)!.ServiceType!;
			if (this._SyncFunctory is not null) return (this._SyncFunctory as IFunctoryInvoker)!.ServiceType!;
			return null;
		}
	}

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
	public ICoreNodeBuilder AddFunctory(IAsyncFunctoryInvoker functory)
	{
		this._AsyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(ISyncFunctoryInvoker functory)
	{
		this._SyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(Func<IArgs, Task<IMsg?>> fn)
	{
		this._AsyncFunctory = new AsyncFunctoryAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(Func<IArgs, IMsg?> fn)
	{
		this._SyncFunctory = new SyncFunctoryAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFunctory(Func<IWorkflowContext, IMsg?> fn)
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
		INode n = new Node(this._Logger, this.Id, this._Context);

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
	///   Initializes a new instance of <see cref="NodeBuilder"/>. 
	/// </summary>
	public CoreNodeBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(
			functitect, 
			nodeFactory,
			logger, 
			id, 
			context
	)
	{
	}
}