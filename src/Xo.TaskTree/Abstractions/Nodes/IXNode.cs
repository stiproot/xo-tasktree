namespace Xo.TaskTree.Abstractions;

public interface IXNode
{
	INodeConfiguration NodeConfiguration { get; init; }
	INodeEdge? NodeEdge { get; init; }
	INodeEdgeResolver NodeEdgeResolver { get; init; }
	IController? Controller { get; init; }
	INodeEvaluator NodeEvaluator { get; init; }
	IAsyncFn? AsyncFn { get; set; }
	ISyncFn SyncFn { get; init; }
	Func<Exception, Task>? AsyncExceptionHandler { get; init; }
	Func<Exception>? ExceptionHandler { get; init; }
	Task<IMsg[]> Run(CancellationToken cancellationToken);
	void Validate();
}
