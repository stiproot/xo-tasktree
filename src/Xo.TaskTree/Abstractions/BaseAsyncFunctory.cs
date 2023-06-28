namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFunctory"/>
[ExcludeFromCodeCoverage]
public abstract class BaseAsyncFunctory : BaseFunctory, IAsyncFunctory
{
	/// <inheritdoc />
	public abstract Func<Task<IMsg?>> CreateFunc(
		IArgs args,
		IWorkflowContext? context = null
	);
}
