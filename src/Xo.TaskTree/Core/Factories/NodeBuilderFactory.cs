namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeBuilderFactory"/>
public class NodeBuilderFactory : INodeBuilderFactory
{
    private readonly IFnFactory _fnFactory;
    private readonly INodeResolver _nodeResolver;

    public NodeBuilderFactory(
        IFnFactory fnFactory,
        INodeResolver nodeResolver
    )
    {
        this._fnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
        this._nodeResolver = nodeResolver ?? throw new ArgumentNullException(nameof(nodeResolver));
    }

    /// <inheritdoc />
    public INodeBuilder Create(ILogger? logger = null)
        => this.Create(this._fnFactory, this._nodeResolver, logger);

    private INodeBuilder Create(
        IFnFactory fnFactory,
        INodeResolver nodeResolver,
        ILogger? logger = null
    )
        => new NodeBuilder(fnFactory, nodeResolver, logger);
}
