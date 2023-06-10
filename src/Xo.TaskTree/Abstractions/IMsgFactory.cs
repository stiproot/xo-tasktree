namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Factory for producing instances of <see cref="IMsg"/> implementations.
/// </summary>
public interface IMsgFactory
{
	/// <summary>
	///   Factory method for creating <see cref="Msg{T}"/>s.
	/// </summary>
	IMsg Create<T>(
		T data,
		string? paramName = null
	);
}
