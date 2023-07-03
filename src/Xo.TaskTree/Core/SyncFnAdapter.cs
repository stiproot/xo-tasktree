namespace Xo.TaskTree.Core;

/// <summary>
///   The functory adapter that wraps a functory... this could be the output of the <see cref="IFnFactory"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class SyncFnAdapter : BaseSyncFn
{
	private readonly Func<IArgs, IMsg?>? _functory;
	private readonly Func<IWorkflowContext, IMsg?>? _contextFn;

	public SyncFnAdapter(Func<IArgs, IMsg?> functory)
		=> this._functory = functory ?? throw new ArgumentNullException(nameof(functory));

	public SyncFnAdapter(Func<IWorkflowContext, IMsg?> functory)
		=> this._contextFn = functory ?? throw new ArgumentNullException(nameof(functory));

	public override IMsg? Invoke(
		IArgs args,
		IWorkflowContext? context = null
	)
		=> this._functory is not null
		? this._functory(args)
		: this._contextFn!(context!);
}