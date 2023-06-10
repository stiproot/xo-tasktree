namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFunctory"/>
[ExcludeFromCodeCoverage]
public abstract class BaseAsyncFunctory : BaseFunctory, IAsyncFunctory
{
	/// <inheritdoc />
	public abstract Func<Task<IMsg?>> CreateFunc(
		IDictionary<string, IMsg> args,
		IWorkflowContext? context = null
	);
}
