namespace Xo.TaskTree.Core;

/// <summary>
///   The fn adapter that wraps a fn... this could be the output of the <see cref="IFnFactory"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class SyncFnAdapter : BaseSyncFn
{
	private readonly Func<IArgs, IMsg?>? _func;
	private readonly Func<IWorkflowContext, IMsg?>? _contextFn;

	public SyncFnAdapter(Func<IArgs, IMsg?> fn)
		=> this._func = fn ?? throw new ArgumentNullException(nameof(fn));

	public SyncFnAdapter(Func<IWorkflowContext, IMsg?> fn)
		=> this._contextFn = fn ?? throw new ArgumentNullException(nameof(fn));

	public override IMsg? Invoke(
		IArgs args,
		IWorkflowContext? context = null
	)
		=> this._func is not null
		? this._func(args)
		: this._contextFn!(context!);
}