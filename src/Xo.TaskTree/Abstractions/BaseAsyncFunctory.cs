namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFunctoryInvoker"/>
[ExcludeFromCodeCoverage]
public abstract class BaseAsyncFunctoryInvoker : BaseFunctoryInvoker, IAsyncFunctoryInvoker
{
	/// <inheritdoc />
	public abstract Task<IMsg?> InvokeFunc(
		IArgs args,
		IWorkflowContext? context = null
	);
}
