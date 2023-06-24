namespace Xo.TaskTree.Abstractions;

public abstract class BaseNodeBuilder
{
	protected readonly IFunctitect _Functitect;
	protected readonly INodeFactory _NodeFactory;
	protected ILogger? _Logger;
	protected IAsyncFunctory? _AsyncFunctory;
	protected ISyncFunctory? _SyncFunctory;
	protected IWorkflowContext? _Context;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected readonly IList<IMsg> _Params = new List<IMsg>();
	protected readonly List<INode> _PromisedParams = new List<INode>();
	protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	public string Id { get; internal set; } = Guid.NewGuid().ToString();
	public bool RequiresResult { get; internal set; } = false;

	public BaseNodeBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		this._Functitect = functitect ?? throw new ArgumentNullException(nameof(functitect));
		this._NodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._Logger = logger;
		if (id is not null) this.Id = id;
		this._Context = context;
	}
}