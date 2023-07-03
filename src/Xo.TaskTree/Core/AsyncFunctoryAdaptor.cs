namespace Xo.TaskTree.Core;

/// <summary>
///   The functory adapter that wraps a functory... this could be the output of the <see cref="IFunctitect"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class AsyncFunctoryAdaptor : BaseAsyncFunctoryInvoker
{
	private readonly Func<IArgs, Task<IMsg?>> _functory;

	public AsyncFunctoryAdaptor(Func<IArgs, Task<IMsg?>> functory)
		=> this._functory = functory ?? throw new ArgumentNullException(nameof(functory));

	public override Task<IMsg?> InvokeFunc(
		IArgs args,
		IWorkflowContext? context = null
	)
		=> this._functory(args);
}