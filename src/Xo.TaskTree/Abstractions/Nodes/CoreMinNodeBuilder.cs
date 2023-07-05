namespace Xo.TaskTree.Abstractions;

public abstract class CoreNodeBuilder : BaseNodeBuilder, ICoreNodeBuilder
{
	public ICoreNodeBuilder Configure(Action<INodeConfigurationBuilder> configure)
	{
		INodeConfigurationBuilder builder;

		if (this._NodeConfiguration is not null) builder = new NodeConfigurationBuilder(this._NodeConfiguration);
		else builder = new NodeConfigurationBuilder();

		configure(builder);

		this._NodeConfiguration = builder.Build();

		return this;
	}

	public ICoreNodeBuilder Configure(INodeConfiguration nodeConfiguration)
	{
		this._NodeConfiguration = nodeConfiguration;
		return this;
	}

	public ICoreNodeBuilder AddNodeEdge(INodeEdge nodeEdge)
	{
		this._NodeEdge = nodeEdge;
		return this;
	}

	/// <inheritdoc />
	public virtual bool HasParam(string paramName) => this._NodeConfiguration.Args.Any(p => p.ParamName == paramName);

	public virtual IFnFactory FnFactory => this._FnFactory;

	/// <inheritdoc />
	public Type? ServiceType
	{
		get
		{
			if (this._AsyncFn is not null) return (this._AsyncFn as IFn)!.ServiceType!;
			if (this._SyncFn is not null) return (this._SyncFn as IFn)!.ServiceType!;
			return null;
		}
	}

	// /// <inheritdoc />
	//public ICoreNodeBuilder RequireResult(bool requiresResult = true)
	//{
	//this.RequiresResult = requiresResult;
	//return this;
	//}

	// // /// <inheritdoc />
	// public ICoreNodeBuilder AddContext(IWorkflowContext? context)
	// {
	// this._Context = context;
	// return this;
	// }

	/// <inheritdoc />
	public ICoreNodeBuilder AddFn(IAsyncFn fn)
	{
		this._AsyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFn(ISyncFn fn)
	{
		this._SyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn)
	{
		this._AsyncFn = new AsyncFnAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFn(Func<IArgs, IMsg?> fn)
	{
		this._SyncFn = new SyncFnAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public ICoreNodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn)
	{
		this._SyncFn = new SyncFnAdapter(fn);
		return this;
	}

	public ICoreNodeBuilder AddInvoker(IInvoker invoker)
	{
		this._Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
		return this;
	}

	public ICoreNodeBuilder AddController(IController controller)
	{
		this._Controller = controller ?? throw new ArgumentNullException(nameof(controller));

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
		INode n = this._NodeFactory.Create(this._Logger)
			.SetNodeConfiguration(this._NodeConfiguration)
			.SetController(this._Controller)
			.SetInvoker(this._Invoker)
			.SetNodeEdge(this._NodeEdge)
			.SetNodevaluator(this._Nodevaluator);

		if (this._AsyncFn is not null) n.SetFn(this._AsyncFn);
		if (this._SyncFn is not null) n.SetFn(this._SyncFn);
		if (this._ExceptionHandlerAsync is not null) n.SetExceptionHandler(this._ExceptionHandlerAsync);
		if (this._ExceptionHandler is not null) n.SetExceptionHandler(this._ExceptionHandler);

		// if (this._Params.Any()) n.AddArg(this._Params.ToArray());
		// if (this._PromisedParams.Any()) n.AddArg(this._PromisedParams.ToArray());
		// if (this._ContextParams.Any()) n.AddArg(this._ContextParams.ToArray());
		// n.RequireResult(this.RequiresResult);

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="NodeBuilder"/>. 
	/// </summary>
	public CoreNodeBuilder(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		ILogger? logger = null
	) : base(
			fnFactory,
			nodeFactory,
			logger
	)
	{
	}
}