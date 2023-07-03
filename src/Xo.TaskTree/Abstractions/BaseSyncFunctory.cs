namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="ISyncFunctory"/>
[ExcludeFromCodeCoverage]
public abstract class BaseSyncFunctory : BaseFunctory, ISyncFunctory
{
	/// <inheritdoc />
	public abstract IMsg? CreateFunc(
		IArgs args,
		IWorkflowContext? context = null
	);
}
