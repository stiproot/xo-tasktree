namespace Xo.TaskTree.Core;

public class NodeBuilder : BaseNodeBuilder, INodeBuilder
{
	/// <inheritdoc />
	public INodeBuilder Configure(Action<INodeConfigurationBuilder> configure)
	{
		INodeConfigurationBuilder builder;

		if (this._NodeConfiguration is not null) builder = new NodeConfigurationBuilder(this._NodeConfiguration);
		else builder = new NodeConfigurationBuilder();

		configure(builder);

		this._NodeConfiguration = builder.Build();

		return this;
	}

	/// <inheritdoc />
	public INodeBuilder Configure(INodeConfiguration nodeConfiguration)
	{
		this._NodeConfiguration = nodeConfiguration;
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddNodeEdge(INodeEdge nodeEdge)
	{
		this._NodeEdge = nodeEdge;
		return this;
	}

	/// <inheritdoc />
	public virtual bool HasParam(string paramName) 
		=> this._NodeConfiguration?.Args.Any(p => p.ParamName == paramName) is true;

	/// <inheritdoc />
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

	/// <inheritdoc />
	public INodeBuilder AddFn(IAsyncFn fn)
	{
		this._AsyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(ISyncFn fn)
	{
		this._SyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn)
	{
		this._AsyncFn = new AsyncFnAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(Func<IArgs, IMsg?> fn)
	{
		this._SyncFn = new SyncFnAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn)
	{
		this._SyncFn = new SyncFnAdapter(fn);
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddInvoker(IInvoker invoker)
	{
		this._Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddController(IController controller)
	{
		this._Controller = controller ?? throw new ArgumentNullException(nameof(controller));
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder SetExceptionHandler(Func<Exception, Task> handler)
	{
		this._ExceptionHandlerAsync = handler;
		return this;
	}

	// <inheritdoc />
	public INodeBuilder SetExceptionHandler(Action<Exception> handler)
	{
		this._ExceptionHandler = handler;
		return this;
	}

	// <inheritdoc />
	public virtual INode Build()
	{
		INode n = this._NodeFactory.Create(this._Logger)
			.SetNodeConfiguration(this._NodeConfiguration ?? throw new InvalidOperationException("Node configuration is required."))
			.SetController(this._Controller)
			.SetInvoker(this._Invoker)
			.SetNodeEdge(this._NodeEdge)
			.SetNodevaluator(this._Nodevaluator);

		if (this._AsyncFn is not null) n.SetFn(this._AsyncFn);
		if (this._SyncFn is not null) n.SetFn(this._SyncFn);
		if (this._ExceptionHandlerAsync is not null) n.SetExceptionHandler(this._ExceptionHandlerAsync);
		if (this._ExceptionHandler is not null) n.SetExceptionHandler(this._ExceptionHandler);

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="INodeBuilder"/>. 
	/// </summary>
	public NodeBuilder(
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