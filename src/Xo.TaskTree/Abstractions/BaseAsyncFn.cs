namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFn"/>
[ExcludeFromCodeCoverage]
public abstract class BaseAsyncFn : BaseFn, IAsyncFn
{
	/// <inheritdoc />
	public abstract Task<IMsg?> Invoke(
		IArgs args,
		IWorkflowContext? workflowContext = null
	);
}
