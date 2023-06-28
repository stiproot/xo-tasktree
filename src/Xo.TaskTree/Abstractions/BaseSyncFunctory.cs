namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="ISyncFunctory"/>
[ExcludeFromCodeCoverage]
public abstract class BaseSyncFunctory : BaseFunctory, ISyncFunctory
{
	/// <inheritdoc />
	public abstract Func<IMsg?> CreateFunc(
		IArgs args,
		IWorkflowContext? context = null
	);
}
