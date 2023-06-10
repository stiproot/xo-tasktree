namespace Xo.TaskTree.Core;

/// <summary>
///   The functory adapter that wraps a functory... this could be the output of the <see cref="IFunctitect"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class SyncFunctoryAdapter : BaseSyncFunctory
{
	private readonly Func<IDictionary<string, IMsg>, Func<IMsg?>>? _functory;
	private readonly Func<IWorkflowContext, Func<IMsg?>>? _contextFunctory;

	public SyncFunctoryAdapter(Func<IDictionary<string, IMsg>, Func<IMsg?>> functory)
		=> this._functory = functory ?? throw new ArgumentNullException(nameof(functory));

	public SyncFunctoryAdapter(Func<IWorkflowContext, Func<IMsg?>> functory)
		=> this._contextFunctory = functory ?? throw new ArgumentNullException(nameof(functory));

	public override Func<IMsg?> CreateFunc(
		IDictionary<string, IMsg> args,
		IWorkflowContext? context = null
	)
		=> this._functory is not null
		? this._functory(args)
		: this._contextFunctory!(context!);
}