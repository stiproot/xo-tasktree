namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFn"/>
[ExcludeFromCodeCoverage]
public abstract class BaseFn : IFn
{
	protected Type? _ServiceType;

	/// <inheritdoc />
	public Type? ServiceType => this._ServiceType;

	/// <inheritdoc />
	public virtual IFn SetServiceType(Type serviceType)
	{
		this._ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
		return this;
	}

	/// <summary>
	///   Value that will become this strategies output <see cref="IMsg"/> `ParamName`.
	///   ie. the next fn parameter name that this output needs to be used for.
	/// </summary>
	/// <remarks>
	///   Required if the result of this fn is to be "fed" into a fn one step up the call chain.
	/// </remarks>
	protected string? _NextParamName;

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
	protected virtual T As<T>()
		=> (T)(IFn)this;

	/// <inheritdoc />
	public virtual ISyncFn AsSync()
		=> this.As<ISyncFn>();

	/// <inheritdoc />
	public virtual IAsyncFn AsAsync()
		=> this.As<IAsyncFn>();
}
