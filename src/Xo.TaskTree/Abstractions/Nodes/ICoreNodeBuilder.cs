namespace Xo.TaskTree.Abstractions;

public interface ICoreNodeBuilder
{
	ICoreNodeBuilder RequireResult(bool requiresResult = true);
	ICoreNodeBuilder AddContext(IWorkflowContext? context);
	ICoreNodeBuilder AddFunctory(IAsyncFunctory functory);
	ICoreNodeBuilder AddFunctory(ISyncFunctory functory);
	ICoreNodeBuilder AddFunctory(Func<IDictionary<string, IMsg>, Func<Task<IMsg?>>> fn);
	ICoreNodeBuilder AddFunctory(Func<IDictionary<string, IMsg>, Func<IMsg?>> fn);
	ICoreNodeBuilder AddFunctory(Func<IWorkflowContext, Func<IMsg?>> fn);
	ICoreNodeBuilder AddArg(params IMsg[] msgs);
	ICoreNodeBuilder AddArg(params INode[] nodes);
	ICoreNodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs);
	ICoreNodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	ICoreNodeBuilder SetExceptionHandler(Action<Exception> handler);
	INode Build();
}