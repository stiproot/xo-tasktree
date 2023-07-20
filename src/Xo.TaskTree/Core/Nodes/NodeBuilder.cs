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

		this._NodeConfiguration = builder.Configuration();

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
			if (this._Fn is not null) return (this._Fn as IFn)!.ServiceType!;
			return null;
		}
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(IFn fn)
	{
		this._Fn = fn ?? throw new ArgumentNullException(nameof(fn));
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn)
	{
		this._Fn = new FnAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(Func<IArgs, IMsg?> fn)
	{
		this._Fn = new FnAdaptor(fn);
		return this;
	}

	/// <inheritdoc />
	public INodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn)
	{
		this._Fn = new FnAdaptor(fn);
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
		INode n = new Node
		{
			// todo: !
			NodeConfiguration = this._NodeConfiguration!,
			Resolver = this._Resolver,
			Controller = this._Controller,
			NodeEdge = this._NodeEdge,
			Fn = this._Fn,
			Fn = this._Fn,
			AsyncExceptionHandler = this._ExceptionHandlerAsync,
			ExceptionHandler = this._ExceptionHandler
		};

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="INodeBuilder"/>. 
	/// </summary>
	public NodeBuilder(
		IFnFactory fnFactory,
		INodeResolver nodeResolver,
		ILogger? logger = null
	) : base(
			fnFactory,
			nodeResolver,
			logger
	)
	{
	}
}