namespace Xo.TaskTree.Abstractions;

public interface INodeBuilder
{
	Type? ServiceType { get; }
	IFnFactory FnFactory { get; }
	bool HasParam(string paramName); 
	INodeBuilder Configure(Action<INodeConfigurationBuilder> configure);
	INodeBuilder Configure(INodeConfiguration nodeConfiguration);
	INodeBuilder AddNodeEdge(INodeEdge nodeEdge);
	INodeBuilder AddFn(IAsyncFn fn);
	INodeBuilder AddFn(ISyncFn fn);
	INodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn);
	INodeBuilder AddFn(Func<IArgs, IMsg?> fn);
	INodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn);
	INodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	INodeBuilder SetExceptionHandler(Action<Exception> handler);
	INodeBuilder AddController(IController controller);
	INode Build();
}