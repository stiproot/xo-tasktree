namespace Xo.TaskTree.Abstractions;

public class NodeBuilder : GenericNodeBuilder
{
	/// <summary>
	///   Initializes a new instance of <see cref="NodeBuilder"/>. 
	/// </summary>
	public NodeBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeBuilderTypes.Default;
}