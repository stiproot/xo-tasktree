namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFunctoryInvoker"/>
[ExcludeFromCodeCoverage]
public abstract class BaseFunctoryInvoker : IFunctoryInvoker
{
	protected Type? _ServiceType;

	/// <inheritdoc />
	public Type? ServiceType => this._ServiceType;

	/// <inheritdoc />
	public virtual IFunctoryInvoker SetServiceType(Type serviceType)
	{
		this._ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
		return this;
	}

	/// <summary>
	///   Value that will become this strategies output <see cref="IMsg"/> `ParamName`.
	///   ie. the next functory parameter name that this output needs to be used for.
	/// </summary>
	/// <remarks>
	///   Required if the result of this functory is to be "fed" into a functory one step up the call chain.
	/// </remarks>
	protected string? _NextParamName;

	/// <inheritdoc />
	public virtual IFunctoryInvoker SetNextParamName(string? nextParamName = null)
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
	protected virtual T As<T>()
		=> (T)(IFunctoryInvoker)this;

	/// <inheritdoc />
	public virtual ISyncFunctoryInvoker AsSync()
		=> this.As<ISyncFunctoryInvoker>();

	/// <inheritdoc />
	public virtual IAsyncFunctoryInvoker AsAsync()
		=> this.As<IAsyncFunctoryInvoker>();
}
