namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeBuilderFactory"/>
public class NodeBuilderFactory : INodeBuilderFactory
{
	private readonly IFunctitect _functitect;
	private readonly INodeFactory _nodeFactory;
	private readonly IMsgFactory _msgFactory;

	public NodeBuilderFactory(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory
	)
	{
		this._functitect = functitect ?? throw new ArgumentNullException(nameof(functitect));
		this._nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	/// <inheritdoc />
	public INodeBuilder Create() => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory);

	/// <inheritdoc />
	public INodeBuilder Create(string id) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, id: id);

	/// <inheritdoc />
	public INodeBuilder Create(ILogger logger, string id) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

	/// <inheritdoc />
	public INodeBuilder Create(ILogger logger) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, logger);

	/// <inheritdoc />
	public INodeBuilder Create(IWorkflowContext context) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, context: context);

	/// <inheritdoc />
	public IPoolBranchNodeBuilder Pool() => (IPoolBranchNodeBuilder)this.Create(NodeTypes.Pool, this._functitect, this._nodeFactory, this._msgFactory);

	/// <inheritdoc />
	public IPoolBranchNodeBuilder Pool(string id) => (IPoolBranchNodeBuilder)this.Create(NodeTypes.Pool, this._functitect, this._nodeFactory, this._msgFactory, id: id);

	/// <inheritdoc />
	public IPoolBranchNodeBuilder Pool(ILogger logger, string id) => (IPoolBranchNodeBuilder)this.Create(NodeTypes.Pool, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

	/// <inheritdoc />
	public IPoolBranchNodeBuilder Pool(ILogger logger) => (IPoolBranchNodeBuilder)this.Create(NodeTypes.Pool, this._functitect, this._nodeFactory, this._msgFactory, logger);

	/// <inheritdoc />
	public IPoolBranchNodeBuilder Pool(IWorkflowContext context) => (IPoolBranchNodeBuilder)this.Create(NodeTypes.Pool, this._functitect, this._nodeFactory, this._msgFactory, context: context);

	/// <inheritdoc />
	public ILinkedBranchNodeBuilder Linked() => (ILinkedBranchNodeBuilder)this.Create(NodeTypes.Linked, this._functitect, this._nodeFactory, this._msgFactory);

	/// <inheritdoc />
	public ILinkedBranchNodeBuilder Linked(string id) => (ILinkedBranchNodeBuilder)this.Create(NodeTypes.Linked, this._functitect, this._nodeFactory, this._msgFactory, id: id);

	/// <inheritdoc />
	public ILinkedBranchNodeBuilder Linked(ILogger logger, string id) => (ILinkedBranchNodeBuilder)this.Create(NodeTypes.Linked, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

	/// <inheritdoc />
	public ILinkedBranchNodeBuilder Linked(ILogger logger) => (ILinkedBranchNodeBuilder)this.Create(NodeTypes.Linked, this._functitect, this._nodeFactory, this._msgFactory, logger);

	/// <inheritdoc />
	public ILinkedBranchNodeBuilder Linked(IWorkflowContext context) => (ILinkedBranchNodeBuilder)this.Create(NodeTypes.Linked, this._functitect, this._nodeFactory, this._msgFactory, context: context);

	/// <inheritdoc />
	public IBinaryBranchNodeBuilder Binary() => (IBinaryBranchNodeBuilder)this.Create(NodeTypes.Binary, this._functitect, this._nodeFactory, this._msgFactory);

	/// <inheritdoc />
	public IBinaryBranchNodeBuilder Binary(string id) => (IBinaryBranchNodeBuilder)this.Create(NodeTypes.Binary, this._functitect, this._nodeFactory, this._msgFactory, id: id);

	/// <inheritdoc />
	public IBinaryBranchNodeBuilder Binary(ILogger logger, string id) => (IBinaryBranchNodeBuilder)this.Create(NodeTypes.Binary, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

	/// <inheritdoc />
	public IBinaryBranchNodeBuilder Binary(ILogger logger) => (IBinaryBranchNodeBuilder)this.Create(NodeTypes.Binary, this._functitect, this._nodeFactory, this._msgFactory, logger);

	/// <inheritdoc />
	public IBinaryBranchNodeBuilder Binary(IWorkflowContext context) => (IBinaryBranchNodeBuilder)this.Create(NodeTypes.Binary, this._functitect, this._nodeFactory, this._msgFactory, context: context);

	/// <inheritdoc />
	public IHashBranchNodeBuilder Hash() => (IHashBranchNodeBuilder)this.Create(NodeTypes.Hash, this._functitect, this._nodeFactory, this._msgFactory);

	/// <inheritdoc />
	public IHashBranchNodeBuilder Hash(string id) => (IHashBranchNodeBuilder)this.Create(NodeTypes.Hash, this._functitect, this._nodeFactory, this._msgFactory, id: id);

	/// <inheritdoc />
	public IHashBranchNodeBuilder Hash(ILogger logger, string id) => (IHashBranchNodeBuilder)this.Create(NodeTypes.Hash, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

	/// <inheritdoc />
	public IHashBranchNodeBuilder Hash(ILogger logger) => (IHashBranchNodeBuilder)this.Create(NodeTypes.Hash, this._functitect, this._nodeFactory, this._msgFactory, logger);

	/// <inheritdoc />
	public IHashBranchNodeBuilder Hash(IWorkflowContext context) => (IHashBranchNodeBuilder)this.Create(NodeTypes.Hash, this._functitect, this._nodeFactory, this._msgFactory, context: context);

	private INodeBuilder Create(
		NodeTypes nodeType,
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		return nodeType switch
		{
			NodeTypes.Default => new NodeBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Linked => new LinkedBranchNodeBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Binary => new BinaryBranchNodeBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Pool => new PoolBranchNodeBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Hash => new HashBranchNodeBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			_ => throw new NotSupportedException()
		};
	}
}
