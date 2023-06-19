namespace Xo.TaskTree.Abstractions;

public class BinaryBranchBuilder : BaseNodeBuilder, IBinaryBranchBuilder
{
	protected INode? _True;
	protected INode? _False;
	protected IBinaryBranchNodePathResolver? _PathResolver;

	public virtual IBinaryBranchBuilder AddTrue<TTrue>(bool requiresResult = true)
	{
		this._True = this.Build(typeof(TTrue));

		if (requiresResult) this._True.RequireResult();

		return this;
	}

	public virtual IBinaryBranchBuilder AddTrue<TTrue, TArgs>(
		TArgs args,
		bool requiresResult = true
	)
	{
		this._True = this.Build(typeof(TTrue));

		this.MatchArgToNodesFunctory<TArgs>(this._True, args);

		if (requiresResult) this._True.RequireResult();

		return this;
	}

	public virtual IBinaryBranchBuilder AddFalse<TFalse>(bool requiresResult = true)
	{
		this._False = this.Build(typeof(TFalse));

		if (requiresResult) this._False.RequireResult();

		return this;
	}

	public virtual IBinaryBranchBuilder AddFalse<TFalse, TArgs>(
		TArgs args,
		bool requiresResult = true
	)
	{
		this._False = this.Build(typeof(TFalse));

		this.MatchArgToNodesFunctory<TArgs>(this._False, args);

		if (requiresResult) this._False.RequireResult();

		return this;
	}

	public virtual IBinaryBranchBuilder AddTrue(INode node)
	{
		this._True = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public virtual IBinaryBranchBuilder AddFalse(INode node)
	{
		this._False = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public virtual IBinaryBranchBuilder AddPathResolver(Func<IMsg?, bool> pathResolver)
	{
		this._PathResolver = new BinaryBranchNodePathResolverAdapter(pathResolver);
		return this;
	}

	public virtual IBinaryBranchBuilder AddPathResolver(IBinaryBranchNodePathResolver pathResolver)
	{
		this._PathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
		return this;
	}

	public virtual IBinaryBranchBuilder AddIsNotNullPathResolver()
	{
		this._PathResolver = new NotNullBinaryBranchNodePathResolver();
		return this;
	}

	public virtual IBinaryBranchBuilder IsNotNull<TService, TArg>(TArg arg)
	{
		this.AddFunctory<TService, TArg>(arg: arg);
		return this.AddIsNotNullPathResolver();
	}

	public virtual IBinaryBranchBuilder IsNotNull<TService>()
	{
		this.AddFunctory<TService>();
		return this.AddIsNotNullPathResolver();
	}

	public override IBinaryBranchNode Build()
	{
		var n = this.BuildBase() as IBinaryBranchNode;

		n!
			.AddTrue(this._True)
			.AddFalse(this._False)
			.AddPathResolver(this._PathResolver);

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public BinaryBranchBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeTypes.Binary;
}