namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeBuilderFactory"/>
public class NodeBuilderFactory : INodeBuilderFactory
{
    private readonly INodeFactory _nodeFactory;
    private readonly IFnFactory _fnFactory;

    public NodeBuilderFactory(
        INodeFactory nodeFactory,
        IFnFactory fnFactory
    )
    {
        this._nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
        this._fnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
    }

    /// <inheritdoc />
    public ICoreNodeBuilder Create()
        => this.Create(NodeBuilderTypes.Default, this._fnFactory, this._nodeFactory);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(NodeBuilderTypes nodeType)
        => this.Create(nodeType, this._fnFactory, this._nodeFactory);

    /// <inheritdoc />
    public TBuilder Create<TBuilder>(NodeBuilderTypes nodeType)
        => (TBuilder)this.Create(nodeType, this._fnFactory, this._nodeFactory);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger logger)
        => this.Create(NodeBuilderTypes.Default, this._fnFactory, this._nodeFactory, logger);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(string id)
        => this.Create(NodeBuilderTypes.Default, this._fnFactory, this._nodeFactory, id: id);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger logger, string id)
        => this.Create(NodeBuilderTypes.Default, this._fnFactory, this._nodeFactory, logger, id);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(IWorkflowContext context)
        => this.Create(NodeBuilderTypes.Default, this._fnFactory, this._nodeFactory, context: context);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger? logger, IWorkflowContext? workflowContext)
        => this.Create(NodeBuilderTypes.Default, this._fnFactory, this._nodeFactory, logger, context:workflowContext);

    private ICoreNodeBuilder Create(
        NodeBuilderTypes nodeType,
        IFnFactory fnFactory,
        INodeFactory nodeFactory,
        ILogger? logger = null,
        string? id = null,
        IWorkflowContext? context = null
    )
    {
        return nodeType switch
        {
            // NodeBuilderTypes.Default => new NodeBuilder(fnFactory, nodeFactory, logger, id, context),
            NodeBuilderTypes.Default => new NodeBuilder(fnFactory, nodeFactory, logger),

            // NodeBuilderTypes.Linked => new LinkedBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.Binary => new BinaryBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.Pool => new PoolBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.Hash => new HashBranchBuilder(fnFactory, nodeFactory, logger, id, context),

            // NodeBuilderTypes.DefaultMetaBranch => new MetaBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.BinaryMetaBranch => new MetaBinaryBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.BranchMetaBranch => new MetaBranchBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.HashMetaBranch => new MetaHashBranchBuilder(fnFactory, nodeFactory, logger, id, context),
            // NodeBuilderTypes.PathMetaBranch => new MetaPathBranchBuilder(fnFactory, nodeFactory, logger, id, context),

            _ => throw new NotSupportedException()
        };
    }
}
