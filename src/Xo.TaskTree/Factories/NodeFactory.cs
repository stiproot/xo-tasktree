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
			// NodeTypes.Linked => new LinkedBranchNode(msgFactory, logger, id, context),
			// NodeTypes.Pool => new PoolBranchNode(msgFactory, logger, id, context),
			// NodeTypes.Binary => new BinaryBranchNode(msgFactory, logger, id, context),
			// NodeTypes.Hash => new HashBranchNode(msgFactory, logger, id, context),
			_ => throw new NotSupportedException()
		};
	}
}
