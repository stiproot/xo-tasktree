namespace Xo.TaskTree.Abstractions;

/// <inheritdoc cref="IMsg"/>
/// <typeparam name="T">The value or reference type of the data this msg houses.</typeparam>
public abstract class BaseMsg<T> : IMsg
{
	// todo: keep this?...
	protected bool _IsPendingMatch = false;

	protected readonly Type _Type;
	protected readonly T _Data;

	/// <inheritdoc />
	public string? ParamName { get; set; }

	/// <inheritdoc />
	public virtual bool IsValueType => this._Type.IsValueType;

	/// <inheritdoc />
	public virtual T GetData() => this._Data ?? throw new TypeAccessException(nameof(this._Data));

	/// <inheritdoc />
	public virtual Type GetDataType() => this._Type ?? throw new TypeAccessException(nameof(this._Type));

	/// <inheritdoc />
	public virtual object ObjectData => (object)this.GetData()!;

	/// <inheritdoc />
	public IMsg SetParam(string paramName)
	{
		this.ParamName = paramName ?? throw new ArgumentNullException(nameof(paramName));
		return this;
	}

	/// <inheritdoc />
	public virtual bool HasParam => this.ParamName is not null;

	/// <inheritdoc />
	public virtual bool HasData => this._Data is not null;

	/// <summary>
	///   Initializes a new instance.
	/// </summary>
	/// <param name="data">The instance of some type that this msg will contain.</param>
	public BaseMsg(T data)
	{
		this._Data = data ?? throw new ArgumentNullException(nameof(data));
		this._Type = this._Data.GetType();
	}

	/// <summary>
	///   Initializes a new instance.
	/// </summary>
	/// <param name="data">The instance of some type that this msg will contain.</param>
	/// <param name="paramName">The name of the parameter that the data contained by this msg should be used for as an argument.</param>
	public BaseMsg(
		T data,
		string paramName
	)
	{
		this._Data = data ?? throw new ArgumentNullException(nameof(data));
		this._Type = this._Data.GetType();
		this.ParamName = paramName ?? throw new ArgumentNullException(nameof(paramName));
	}
}
