namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IFunctory"/>
[ExcludeFromCodeCoverage]
public abstract class BaseFunctory : IFunctory
{
	protected Type? _ServiceType;

	/// <inheritdoc />
	public Type? ServiceType => this._ServiceType;

	/// <inheritdoc />
	public virtual IFunctory SetServiceType(Type serviceType)
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
	public virtual IFunctory SetNextParamName(string? nextParamName = null)
	{
		this._NextParamName = nextParamName;
		return this;
	}

	/// <inheritdoc />
	public virtual IMsg SafeGet(
		IDictionary<string, IMsg> pairs,
		string key
	)
	{
		if (!pairs.TryGetValue(key, out IMsg? value)) throw new KeyNotFoundException(key);
		return value;
	}

	/// <inheritdoc />
	public virtual T Cast<T>(IMsg engineMessage) => (T)engineMessage;

	/// <inheritdoc />
	protected virtual T As<T>()
		=> (T)(IFunctory)this;

	/// <inheritdoc />
	public virtual ISyncFunctory AsSync()
		=> this.As<ISyncFunctory>();

	/// <inheritdoc />
	public virtual IAsyncFunctory AsAsync()
		=> this.As<IAsyncFunctory>();
}
