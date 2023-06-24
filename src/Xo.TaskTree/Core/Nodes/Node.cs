namespace Xo.TaskTree.Core;

/// <inheritdoc cref="INode"/>
public sealed class Node : BaseNode
{
	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public Node(
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(logger, id, context) 
	{ 
	}
}
