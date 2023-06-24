namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeBuilderFactory"/>
public class NodeBuilderFactory : INodeBuilderFactory
{
    private readonly INodeFactory _nodeFactory;
    private readonly IFunctitect _functitect;

    public NodeBuilderFactory(
        INodeFactory nodeFactory,
        IFunctitect functitect
    )
    {
        this._nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
        this._functitect = functitect ?? throw new ArgumentNullException(nameof(functitect));
    }

    /// <inheritdoc />
    public ICoreNodeBuilder Create()
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(NodeBuilderTypes nodeType)
        => this.Create(nodeType, this._functitect, this._nodeFactory);

    /// <inheritdoc />
    public TBuilder Create<TBuilder>(NodeBuilderTypes nodeType)
        => (TBuilder)this.Create(nodeType, this._functitect, this._nodeFactory);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger logger)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, logger);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(string id)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, id: id);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(ILogger logger, string id)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, logger, id);

    /// <inheritdoc />
    public ICoreNodeBuilder Create(IWorkflowContext context)
        => this.Create(NodeBuilderTypes.Default, this._functitect, this._nodeFactory, context: context);

    private ICoreNodeBuilder Create(
        NodeBuilderTypes nodeType,
        IFunctitect functitect,
        INodeFactory nodeFactory,
        ILogger? logger = null,
        string? id = null,
        IWorkflowContext? context = null
    )
    {
        return nodeType switch
        {
            NodeBuilderTypes.Default => new NodeBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.Linked => new LinkedBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.Binary => new BinaryBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.Pool => new PoolBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.Hash => new HashBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.DefaultMetaBranch => new MetaBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.BinaryMetaBranch => new MetaBinaryBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.BranchMetaBranch => new MetaBranchBranchBuilder(functitect, nodeFactory, logger, id, context),
            NodeBuilderTypes.HashMetaBranch => new MetaHashBranchBuilder(functitect, nodeFactory, logger, id, context),
            _ => throw new NotSupportedException()
        };
    }
}
