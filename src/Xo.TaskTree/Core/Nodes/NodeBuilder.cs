namespace Xo.TaskTree.Abstractions;

public sealed class NodeBuilder : CoreNodeBuilder
{
	/// <summary>
	///   Initializes a new instance of <see cref="NodeBuilder"/>. 
	/// </summary>
	public NodeBuilder(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		ILogger? logger = null
		// string? id = null,
		// IWorkflowContext? context = null
	) : base(
			fnFactory, 
			nodeFactory,
			logger
			// id, 
			// context
	)
	{ 
	}
}