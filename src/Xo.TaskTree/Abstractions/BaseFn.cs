namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFn"/>
[ExcludeFromCodeCoverage]
public abstract class BaseFn : IFn
{
	protected Type? _ServiceType;
	protected string? _NextParamName;

	/// <inheritdoc />
	public Type? ServiceType => this._ServiceType;

	/// <inheritdoc />
	public IFn SetServiceType(Type serviceType)
	{
		this._ServiceType = serviceType;
		return this;
	}

	/// <inheritdoc />
	public virtual IFn SetNextParamName(string? nextParamName = null)
	{
		this._NextParamName = nextParamName;
		return this;
	}

	/// <inheritdoc />
	public virtual IMsg SafeGet(
		IArgs pairs,
		string key
	)
	{
		var msg = pairs[key];

		if (msg is null) throw new KeyNotFoundException(key);

		return msg;
	}

	/// <inheritdoc />
	public virtual T Cast<T>(IMsg engineMessage) => (T)engineMessage;

	/// <inheritdoc />
	public virtual async Task<IMsg?> InvokeAsync(
		IArgs args,
		IWorkflowContext? workflowContext = null
	)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public virtual IMsg? Invoke(
		IArgs args,
		IWorkflowContext? workflowContext = null
	)
	{
		throw new NotImplementedException();
	}
}
