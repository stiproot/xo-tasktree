namespace Xo.TaskTree.Abstractions;

public interface INode
{
	INodeConfiguration NodeConfiguration { get; }
	INode SetNodeConfiguration(INodeConfiguration nodeConfiguration);
	INode SetNodeEdge(INodeEdge? nodeEdge);
	INode SetResolver(INodeEdgeResolver resolver);
	INode SetController(IController? controller);
	INode SetNodevaluator(INodeEvaluator nodeEvaluator);
	INode SetFn(IAsyncFn fn);
	INode SetFn(ISyncFn fn);
	INode SetExceptionHandler(Func<Exception, Task> handler);
	INode SetExceptionHandler(Action<Exception> handler);
	Task<IMsg[]> Run(CancellationToken cancellationToken);
	void Validate();
}
