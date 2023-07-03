namespace Xo.TaskTree.Abstractions;

public abstract class BaseNodeBuilder
{
	protected readonly IFnFactory _FnFactory;
	protected readonly INodeFactory _NodeFactory;
	protected ILogger? _Logger;
	protected IAsyncFn? _AsyncFn;
	protected ISyncFn? _SyncFn;
	protected IWorkflowContext? _Context;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected readonly IList<IMsg> _Params = new List<IMsg>();
	protected readonly List<INode> _PromisedParams = new List<INode>();
	protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	public string Id { get; internal set; } = Guid.NewGuid().ToString();
	public bool RequiresResult { get; internal set; } = false;

	public BaseNodeBuilder(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		this._FnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._NodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._Logger = logger;
		if (id is not null) this.Id = id;
		this._Context = context;
	}
}