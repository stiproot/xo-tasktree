namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="ISyncFn"/>
[ExcludeFromCodeCoverage]
public abstract class BaseSyncFn : BaseFn, ISyncFn
{
	/// <inheritdoc />
	public abstract IMsg? Invoke(
		IArgs args,
		IWorkflowContext? context = null
	);
}
