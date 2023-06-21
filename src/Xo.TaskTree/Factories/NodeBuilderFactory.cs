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
	public INodeBuilder Create(NodeTypes nodeType) => this.Create(nodeType, this._functitect, this._nodeFactory, this._msgFactory);

	/// <inheritdoc />
	public INodeBuilder Create(string id) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, id: id);

	/// <inheritdoc />
	public INodeBuilder Create(ILogger logger, string id) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

	/// <inheritdoc />
	public INodeBuilder Create(ILogger logger) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, logger);

	/// <inheritdoc />
	public INodeBuilder Create(IWorkflowContext context) => this.Create(NodeTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, context: context);

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
			NodeTypes.Linked => new LinkedBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Binary => new BinaryBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Pool => new PoolBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			NodeTypes.Hash => new HashBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
			_ => throw new NotSupportedException()
		};
	}
}
