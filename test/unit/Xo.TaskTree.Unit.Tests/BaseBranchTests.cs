namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public abstract class BaseBranchTests
{
	protected readonly IFnFactory _FnFactory;
	protected readonly INodeFactory _NodeFactory;
	protected readonly IMsgFactory _MsgFactory;
	protected readonly IWorkflowContextFactory _WorkflowContextFactory;
	protected readonly INodeBuilderFactory _NodeBuilderFactory;
	protected CancellationToken CancellationTokenFactory() => new CancellationToken();

	public BaseBranchTests(
		IFnFactory functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		IWorkflowContextFactory workflowContextFactory,
		INodeBuilderFactory nodeBuilderFactory
	)
	{
		this._FnFactory = functitect ?? throw new ArgumentNullException(nameof(functitect));
		this._NodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._MsgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
		this._WorkflowContextFactory = workflowContextFactory ?? throw new ArgumentNullException(nameof(workflowContextFactory));
		this._NodeBuilderFactory = nodeBuilderFactory ?? throw new ArgumentNullException(nameof(nodeBuilderFactory));
	}
}
