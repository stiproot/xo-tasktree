namespace Xo.TaskTree.Abstractions;

public class XBinaryBranchBuilder : BaseNodeBuilder, IXBinaryBranchBuilder
{
	protected Type? _TrueType;
	protected Type? _FalseType;
	protected INode? _TrueNode;
	protected INode? _FalseNode;

	public virtual IXBinaryBranchBuilder AddTrue<TTrue>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this._True = this.Build(typeof(TTrue));
		// if (requiresResult) this._True.RequireResult();

		this._TrueType = typeof(TTrue);

		return this;
	}

	public virtual IXBinaryBranchBuilder AddTrue<TTrue, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	)
	{
		// this._True = this.Build(typeof(TTrue));
		// this.MatchArgToNodesFunctory<TArgs>(this._True, args);
		// if (requiresResult) this._True.RequireResult();

		this._TrueType = typeof(TTrue);

		return this;
	}

	public virtual IXBinaryBranchBuilder AddFalse<TFalse>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this._False = this.Build(typeof(TFalse));
		// if (requiresResult) this._False.RequireResult();

		this._FalseType = typeof(TFalse);

		return this;
	}

	public virtual IXBinaryBranchBuilder AddFalse<TFalse, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	)
	{
		// this._False = this.Build(typeof(TFalse));
		// this.MatchArgToNodesFunctory<TArgs>(this._False, args);
		// if (requiresResult) this._False.RequireResult();

		this._FalseType = typeof(TFalse);

		return this;
	}

	public virtual IXBinaryBranchBuilder IsNotNull<TService, TArg>(TArg arg)
	{
		// this.AddFunctory<TService, TArg>(arg: arg);
		// return this.AddIsNotNullPathResolver();

		// todo: ...

		return this;
	}

	public virtual IXBinaryBranchBuilder IsNotNull<TService>()
	{
		// this.AddFunctory<TService>();
		// return this.AddIsNotNullPathResolver();

		// todo: ...

		return this;
	}

	public override IBinaryBranchNode Build()
	{
		// var n = this.BuildBase() as IBinaryBranchNode;

		// n!
			// .AddTrue(this._True)
			// .AddFalse(this._False)
			// .AddPathResolver(this._PathResolver);

		// return n;

		throw new NotImplementedException();
	}

	// public virtual IXBinaryBranchBuilder AddTrue(INode node)
	// {
		// this._True = node ?? throw new ArgumentNullException(nameof(node));
		// return this;
	// }

	// public virtual IXBinaryBranchBuilder AddFalse(INode node)
	// {
		// this._False = node ?? throw new ArgumentNullException(nameof(node));
		// return this;
	// }

	// public virtual IXBinaryBranchBuilder AddPathResolver(Func<IMsg?, bool> pathResolver)
	// {
		// this._PathResolver = new BinaryBranchNodePathResolverAdapter(pathResolver);
		// return this;
	// }

	// public virtual IXBinaryBranchBuilder AddPathResolver(IBinaryBranchNodePathResolver pathResolver)
	// {
		// this._PathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
		// return this;
	// }

	// public virtual IXBinaryBranchBuilder AddIsNotNullPathResolver()
	// {
		// this._PathResolver = new NotNullBinaryBranchNodePathResolver();
		// return this;
	// }

	/// <summary>
	///   Initializes a new instance of <see cref="XBinaryBranchBuilder"/>. 
	/// </summary>
	public XBinaryBranchBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeTypes.Binary;
}