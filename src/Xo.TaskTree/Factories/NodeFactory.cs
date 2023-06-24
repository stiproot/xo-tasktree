namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="INodeFactory"/>
public class NodeFactory : INodeFactory
{
	/// <inheritdoc />
	public INode Create() => this.Create(null, null, null);

	/// <inheritdoc />
	public INode Create(string id) => this.Create(null, id: id, null);

	/// <inheritdoc />
	public INode Create(ILogger logger, string id) => this.Create(logger, id, null);

	/// <inheritdoc />
	public INode Create(ILogger logger) => this.Create(logger, null, null);

	/// <inheritdoc />
	public INode Create(IWorkflowContext context) => this.Create(null, null, context: context);

	public INode Create(
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
		=> new Node(logger, id, context);
}
