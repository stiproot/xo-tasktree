namespace Xo.TaskTree.Abstractions;

public abstract class CoreBranchBuilder
{
	protected readonly INodeBuilderFactory _NodeBuilderFactory;
	protected readonly IFnFactory _FnFactory;
	protected readonly ILogger? _Logger;
	protected readonly IWorkflowContext? _WorkflowContext;

	public CoreBranchBuilder(
		INodeBuilderFactory nodeBuilderFactory,
		IFnFactory fnFactory,
		ILogger? logger = null,
		IWorkflowContext? workflowContext = null
	)
	{
		this._NodeBuilderFactory = nodeBuilderFactory ?? throw new ArgumentNullException(nameof(nodeBuilderFactory));
		this._FnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._Logger = logger;
		this._WorkflowContext = workflowContext;
	}
}