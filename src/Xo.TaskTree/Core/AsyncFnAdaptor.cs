namespace Xo.TaskTree.Core;

/// <summary>
///   The fn adapter that wraps a fn... this could be the output of the <see cref="IFnFactory"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class AsyncFnAdaptor : BaseAsyncFn
{
	private readonly Func<IArgs, Task<IMsg?>> _func;

	public AsyncFnAdaptor(Func<IArgs, Task<IMsg?>> fn)
		=> this._func = fn ?? throw new ArgumentNullException(nameof(fn));

	public override Task<IMsg?> Invoke(
		IArgs args,
		IWorkflowContext? context = null
	)
		=> this._func(args);
}