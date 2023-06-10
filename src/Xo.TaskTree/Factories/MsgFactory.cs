namespace Xo.TaskTree.Factories;

/// <inheritdoc cref="IMsgFactory"/>
public class MsgFactory : IMsgFactory
{
	/// <inheritdoc />
	public IMsg Create<T>(
		T data,
		string? paramName = null
	) => paramName == null ? new Msg<T>(data) : new Msg<T>(data, paramName);
}
