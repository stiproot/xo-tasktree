namespace Xo.TaskTree.Abstractions;

public class XBinaryBranchBuilder : BaseNodeBuilder, IXBinaryBranchBuilder
{
	protected IMetaNodeMapper _MetaNodeMapper;
	protected Type? _TrueType;
	protected Action<INodeConfigurationBuilder>? _ConfigureTrue;
	protected Type? _FalseType;
	protected INode? _TrueNode;
	protected INode? _FalseNode;

	public virtual IXBinaryBranchBuilder AddTrue<TTrue>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this._True = this.Build(typeof(TTrue));
		// if (requiresResult) this._True.RequireResult();

		this._TrueType = typeof(TTrue);
		this._ConfigureTrue = configure;

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
		this._ConfigureTrue = configure;

		return this;
	}

	 public virtual IXBinaryBranchBuilder AddTrue(INode node)
	 {
		this._TrueNode = node ?? throw new ArgumentNullException(nameof(node));
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

	public virtual IXBinaryBranchBuilder AddFalse(INode node)
	{
		this._FalseNode = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public virtual IXBinaryBranchBuilder IsNotNull<TService, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	)
	{
		// this.AddFunctory<TService, TArg>(arg: arg);
		// return this.AddIsNotNullPathResolver();

		// todo: ...

		return this;
	}

	public virtual IXBinaryBranchBuilder IsNotNull<TService>(Action<INodeConfigurationBuilder>? configure = null)
	{
		// this.AddFunctory<TService>();
		// return this.AddIsNotNullPathResolver();

		// todo: ...

		return this;
	}

	public override INodeBuilder AddFunctory<T>(string? nextParamName = null)
	{
		this.__FunctoryType = typeof(T);
		return this;
	}

	public override INode Build()
	{
		throw new NotImplementedException();
	}

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