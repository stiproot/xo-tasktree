namespace Xo.TaskTree.Abstractions;

public interface INode
{
	INodeConfiguration NodeConfiguration { get; init; }
	INodeResolver Resolver { get; init; }
	INodeEdge? NodeEdge { get; init; }
	IController? Controller { get; init; }
	IFn Fn { get; init; }
	Func<Exception, Task>? AsyncExceptionHandler { get; init; }
	Action<Exception>? ExceptionHandler { get; init; }
	Task<IMsg[]> Resolve(CancellationToken cancellationToken);
	void Validate();
}
