namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="ISyncFunctoryInvoker"/>
[ExcludeFromCodeCoverage]
public abstract class BaseSyncFunctoryInvoker : BaseFunctoryInvoker, ISyncFunctoryInvoker
{
	/// <inheritdoc />
	public abstract IMsg? InvokeFunc(
		IArgs args,
		IWorkflowContext? context = null
	);
}
