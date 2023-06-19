namespace Xo.TaskTree.Abstractions;

public class XBinaryBranchBuilder : BaseNodeBuilder, IXBinaryBranchBuilder
{
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
		IAsyncFunctory rootFunctory = this._Functitect
			.Build(this.__FunctoryType!)
			.SetServiceType(this.__FunctoryType!)
			.AsAsync();
		
		var rootNode = this._NodeFactory.Create(
			NodeTypes.Default,
			this._Logger,
			this.Id,
			this._Context
		);

		var configBuilder = new NodeConfigurationBuilder();
		this.__Configure!(configBuilder);
		var rootNodeConfig = configBuilder.Build();

		rootNode.SetFunctory(rootFunctory);

		if(rootNodeConfig.Args.Any()) rootNode.AddArg(rootNodeConfig.Args.ToArray());

		if(this._TrueType is not null)
		{
			IAsyncFunctory trueFunctory = this._Functitect
				.Build(this.__FunctoryType!)
				.SetServiceType(this.__FunctoryType!)
				.AsAsync();
			
			var trueNode = this._NodeFactory.Create(
				NodeTypes.Default,
				this._Logger
			);

			trueNode.SetContext(this._Context);

			var trueNodeConfigBuilder = new NodeConfigurationBuilder();
			this._ConfigureTrue!(trueNodeConfigBuilder);
			var trueNodeConfig = trueNodeConfigBuilder.Build();

			trueNode.SetFunctory(trueFunctory);

			if(trueNodeConfig.Args.Any()) trueNode.AddArg(trueNodeConfig.Args.ToArray());

			// todo: this is ridiculous...
			Func<IDictionary<string, IMsg>, Func<IMsg>> trueDecisionNodeFunctory = (p) => () => this._MsgFactory.Create<bool>(((p.First().Value) as Msg<bool>)!.GetData());
			var trueDecisionNodeEdge = new MonariusNodeEdge().Add(trueNode);
			var trueDecisionNode = this._NodeFactory
				.Create()
				.SetFunctory(trueDecisionNodeFunctory)
				.SetController(new TrueController())
				.SetNodeEdge(trueDecisionNodeEdge);
		
			var nodeEdge = new BinariusNodeEdge().Add(trueDecisionNode);;

			rootNode.SetNodeEdge(nodeEdge);
		}

		return rootNode;
	}

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