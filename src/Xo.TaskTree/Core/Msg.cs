namespace Xo.TaskTree.Core;

/// <inheritdoc cref="BaseMsg"/>
/// <typeparam name="T">The type of the data that this msg will contain.</typeparam>
public class Msg<T> : BaseMsg<T>
{
	/// <summary>
	///   Initializes a new instance.
	/// </summary>
	/// <param name="data">The instance of some type that this msg will contain.</param>
	public Msg(T data) : base(data) 
	{
	}

	/// <summary>
	///   Initializes a new instance.
	/// </summary>
	/// <param name="data">The instance of some type that this msg will contain.</param>
	/// <param name="paramName">The name of the parameter that the data contained by this msg should be used for as an argument.</param>
	public Msg(
		T data,
		string paramName
	) : base(data, paramName)
	{ 
	}
}
