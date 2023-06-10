namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeFactory"/>
public class NodeFactory : INodeFactory
{
	private readonly IMsgFactory _msgFactory;

	public NodeFactory(IMsgFactory msgFactory) => this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));

	/// <inheritdoc />
	public INode Create() => this.Create(NodeTypes.Default, this._msgFactory);

	/// <inheritdoc />
	public INode Create(string id) => this.Create(NodeTypes.Default, this._msgFactory, id: id);

	/// <inheritdoc />
	public INode Create(ILogger logger, string id) => this.Create(NodeTypes.Default, this._msgFactory, logger, id);

	/// <inheritdoc />
	public INode Create(ILogger logger) => this.Create(NodeTypes.Default, this._msgFactory, logger);

	/// <inheritdoc />
	public INode Create(IWorkflowContext context) => this.Create(NodeTypes.Default, this._msgFactory, context: context);

	/// <inheritdoc />
	public IPoolBranchNode Pool() => (IPoolBranchNode)this.Create(NodeTypes.Pool, this._msgFactory);

	/// <inheritdoc />
	public IPoolBranchNode Pool(string id) => (IPoolBranchNode)this.Create(NodeTypes.Pool, this._msgFactory, id: id);

	/// <inheritdoc />
	public IPoolBranchNode Pool(ILogger logger, string id) => (IPoolBranchNode)this.Create(NodeTypes.Pool, this._msgFactory, logger, id);

	/// <inheritdoc />
	public IPoolBranchNode Pool(ILogger logger) => (IPoolBranchNode)this.Create(NodeTypes.Pool, this._msgFactory, logger);

	/// <inheritdoc />
	public IPoolBranchNode Pool(IWorkflowContext context) => (IPoolBranchNode)this.Create(NodeTypes.Pool, this._msgFactory, context: context);

	/// <inheritdoc />
	public ILinkedBranchNode Linked() => (ILinkedBranchNode)this.Create(NodeTypes.Linked, this._msgFactory);

	/// <inheritdoc />
	public ILinkedBranchNode Linked(string id) => (ILinkedBranchNode)this.Create(NodeTypes.Linked, this._msgFactory, id: id);

	/// <inheritdoc />
	public ILinkedBranchNode Linked(ILogger logger, string id) => (ILinkedBranchNode)this.Create(NodeTypes.Linked, this._msgFactory, logger, id);

	/// <inheritdoc />
	public ILinkedBranchNode Linked(ILogger logger) => (ILinkedBranchNode)this.Create(NodeTypes.Linked, this._msgFactory, logger);

	/// <inheritdoc />
	public ILinkedBranchNode Linked(IWorkflowContext context) => (ILinkedBranchNode)this.Create(NodeTypes.Linked, this._msgFactory, context: context);

	/// <inheritdoc />
	public IBinaryBranchNode Binary() => (IBinaryBranchNode)this.Create(NodeTypes.Binary, this._msgFactory);

	/// <inheritdoc />
	public IBinaryBranchNode Binary(string id) => (IBinaryBranchNode)this.Create(NodeTypes.Binary, this._msgFactory, id: id);

	/// <inheritdoc />
	public IBinaryBranchNode Binary(ILogger logger, string id) => (IBinaryBranchNode)this.Create(NodeTypes.Binary, this._msgFactory, logger, id);

	/// <inheritdoc />
	public IBinaryBranchNode Binary(ILogger logger) => (IBinaryBranchNode)this.Create(NodeTypes.Binary, this._msgFactory, logger);

	/// <inheritdoc />
	public IBinaryBranchNode Binary(IWorkflowContext context) => (IBinaryBranchNode)this.Create(NodeTypes.Binary, this._msgFactory, context: context);

	/// <inheritdoc />
	public IHashBranchNode Hash() => (IHashBranchNode)this.Create(NodeTypes.Hash, this._msgFactory);

	/// <inheritdoc />
	public IHashBranchNode Hash(string id) => (IHashBranchNode)this.Create(NodeTypes.Hash, this._msgFactory, id: id);

	/// <inheritdoc />
	public IHashBranchNode Hash(ILogger logger, string id) => (IHashBranchNode)this.Create(NodeTypes.Hash, this._msgFactory, logger, id);

	/// <inheritdoc />
	public IHashBranchNode Hash(ILogger logger) => (IHashBranchNode)this.Create(NodeTypes.Hash, this._msgFactory, logger);

	/// <inheritdoc />
	public IHashBranchNode Hash(IWorkflowContext context) => (IHashBranchNode)this.Create(NodeTypes.Hash, this._msgFactory, context: context);

	public INode Create(
		NodeTypes nodeType,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) => this.Create(nodeType, this._msgFactory, logger, id, context);

	private INode Create(
		NodeTypes nodeType,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		return nodeType switch
		{
			NodeTypes.Default => new Node(msgFactory, logger, id, context),
			NodeTypes.Linked => new LinkedBranchNode(msgFactory, logger, id, context),
			NodeTypes.Pool => new PoolBranchNode(msgFactory, logger, id, context),
			NodeTypes.Binary => new BinaryBranchNode(msgFactory, logger, id, context),
			NodeTypes.Hash => new HashBranchNode(msgFactory, logger, id, context),
			_ => throw new NotSupportedException()
		};
	}
}
