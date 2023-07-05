namespace Xo.TaskTree.Abstractions;

public abstract class BaseNodeBuilder
{
	protected readonly IFnFactory _FnFactory;
	protected readonly INodeFactory _NodeFactory;
	protected ILogger? _Logger;
	protected IAsyncFn? _AsyncFn;
	protected ISyncFn? _SyncFn;
	protected INodeEdge? _NodeEdge;
	protected INodeConfiguration? _NodeConfiguration;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected IController? _Controller;
	protected IInvoker _Invoker = new Invoker(new NodeEdgeResolver());
	protected INodevaluator _Nodevaluator = new ParallelNodeEvaluator();

	public BaseNodeBuilder(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		ILogger? logger = null
	)
	{
		this._FnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._NodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._Logger = logger;
	}
}