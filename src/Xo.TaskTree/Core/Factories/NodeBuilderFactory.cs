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
	public INodeBuilder Create(ILogger? logger = null)
		=> this.Create(this._fnFactory, this._nodeFactory, logger);

	private INodeBuilder Create(
			IFnFactory fnFactory,
			INodeFactory nodeFactory,
			ILogger? logger = null
	)
		=> new NodeBuilder(fnFactory, nodeFactory, logger);
}
