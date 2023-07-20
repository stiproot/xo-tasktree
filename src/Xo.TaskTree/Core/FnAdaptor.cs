namespace Xo.TaskTree.Core;

/// <summary>
///   The fn adapter that wraps a fn... this could be the output of the <see cref="IFnFactory"/>'s core `Build` method, or an anonymous func.
/// </summary>
public sealed class FnAdaptor : BaseFn
{
	private readonly Func<IArgs, Task<IMsg?>>? _asyncFn;
	private readonly Func<IArgs, IMsg?>? _fn;
	private readonly Func<IWorkflowContext, IMsg?>? _contextFn;

	public FnAdaptor(Func<IArgs, Task<IMsg?>> fn)
		=> this._asyncFn = fn ?? throw new ArgumentNullException(nameof(fn));

	public FnAdaptor(Func<IArgs, IMsg?> fn)
		=> this._fn = fn ?? throw new ArgumentNullException(nameof(fn));

	public FnAdaptor(Func<IWorkflowContext, IMsg?> fn)
		=> this._contextFn = fn ?? throw new ArgumentNullException(nameof(fn));

	public override Task<IMsg?> InvokeAsync(
		IArgs args,
		IWorkflowContext? workflowContext = null
	)
		=> this._asyncFn!(args);

	public override IMsg? Invoke(
		IArgs args,
		IWorkflowContext? workflowContext = null
	)
		=> this._fn is not null
		? this._fn(args)
		: this._contextFn!(workflowContext!);
}