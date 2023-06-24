namespace Xo.TaskTree.Abstractions;

public class BinaryBranchBuilder : CoreNodeBuilder, IBinaryBranchBuilder
{
	protected INode? _TrueNode;
	protected INode? _FalseNode;
	protected Type? _TrueType;
	protected Type? _FalseType;
	protected Action<INodeConfigurationBuilder>? _ConfigureTrue;
	protected Action<INodeConfigurationBuilder>? _ConfigureFalse;

	public virtual IBinaryBranchBuilder AddTrue<TTrue>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this._True = this.Build(typeof(TTrue));
		// if (requiresResult) this._True.RequireResult();

		this._TrueType = typeof(TTrue);

		this._ConfigureTrue = configure;

		return this;
	}

	public virtual IBinaryBranchBuilder AddTrue<TTrue, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	)
	{
		// this._True = this.Build(typeof(TTrue));
		// this.MatchArgToNodesFunctory<TArgs>(this._True, args);
		// if (requiresResult) this._True.RequireResult();

		this._TrueType = typeof(TTrue);
		this._ConfigureTrue = configure;

		return this;
	}

	 public virtual IBinaryBranchBuilder AddTrue(INode node)
	 {
		this._TrueNode = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	 }

	public virtual IBinaryBranchBuilder AddFalse<TFalse>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this._False = this.Build(typeof(TFalse));
		// if (requiresResult) this._False.RequireResult();

		this._FalseType = typeof(TFalse);

		return this;
	}

	public virtual IBinaryBranchBuilder AddFalse<TFalse, TArg>(
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

	public virtual IBinaryBranchBuilder AddFalse(INode node)
	{
		this._FalseNode = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public virtual IBinaryBranchBuilder IsNotNull<TService, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	)
	{
		// this.AddFunctory<TService, TArg>(arg: arg);
		// return this.AddIsNotNullPathResolver();

		// todo: ...

		return this;
	}

	public virtual IBinaryBranchBuilder IsNotNull<TService>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this.AddFunctory<TService>();
		// return this.AddIsNotNullPathResolver();

		// todo: ...

		return this;
	}

	public override INode Build()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public BinaryBranchBuilder(
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