namespace Xo.TaskTree.Abstractions;

public interface INodeBuilder
{
	IFnFactory FnFactory { get; }
	INodeBuilder Configure(Action<INodeConfigurationBuilder> configure);
	INodeBuilder Configure(INodeConfiguration nodeConfiguration);
	INodeBuilder AddNodeEdge(INodeEdge nodeEdge);
	INodeBuilder AddFn(IFn fn);
	INodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn);
	INodeBuilder AddFn(Func<IArgs, IMsg?> fn);
	INodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn);
	INodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	INodeBuilder SetExceptionHandler(Action<Exception> handler);
	INodeBuilder AddController(IController controller);
	INode Build();
}