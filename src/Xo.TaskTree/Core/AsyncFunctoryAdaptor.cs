namespace Xo.TaskTree.Core;

/// <summary>
///   The functory adapter that wraps a functory... this could be the output of the <see cref="IFunctitect"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class AsyncFunctoryAdaptor : BaseAsyncFunctory
{
	private readonly Func<IReadOnlyList<IMsg>, Func<Task<IMsg?>>> _functory;

	public AsyncFunctoryAdaptor(Func<IReadOnlyList<IMsg>, Func<Task<IMsg?>>> functory)
		=> this._functory = functory ?? throw new ArgumentNullException(nameof(functory));

	public override Func<Task<IMsg?>> CreateFunc(
		IReadOnlyList<IMsg> args,
		IWorkflowContext? context = null
	)
		=> this._functory(args);
}