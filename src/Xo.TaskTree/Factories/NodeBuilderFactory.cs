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
    public ICoreNodeBuilder Create()
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, this._msgFactory);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(NodeBuilderTypes nodeType)
        => this.Create(nodeType, this._functitect, this._nodeFactory, this._msgFactory);

    /// <inheritdoc />
    public TBuilder Create<TBuilder>(NodeBuilderTypes nodeType)
        => (TBuilder)this.Create(nodeType, this._functitect, this._nodeFactory, this._msgFactory);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger logger)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, logger);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(string id)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, id: id);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger logger, string id)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, logger, id);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(IWorkflowContext context)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, this._msgFactory, context: context);

    private ICoreNodeBuilder Create(
        NodeBuilderTypes nodeType,
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
            NodeBuilderTypes.Default => new NodeBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.Linked => new LinkedBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.Binary => new BinaryBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.Pool => new PoolBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.Hash => new HashBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.DefaultMetaBranch => new MetaBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.BinaryMetaBranch => new MetaBinaryBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.BranchMetaBranch => new MetaBranchBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            NodeBuilderTypes.HashMetaBranch => new MetaHashBranchBuilder(functitect, nodeFactory, msgFactory, logger, id, context),
            _ => throw new NotSupportedException()
        };
    }
}
