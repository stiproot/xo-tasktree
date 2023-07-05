namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeFactory"/>
public class NodeFactory : INodeFactory
{
	public INode Create(
		ILogger? logger = null,
		INodeConfiguration? nodeConfiguration = null
	)
		=> new Node(logger, nodeConfiguration);
}
