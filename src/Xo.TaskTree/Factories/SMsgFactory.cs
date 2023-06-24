namespace Xo.TaskTree.Factories;

public static class SMsgFactory
{
	/// <inheritdoc />
	public static IMsg Create<T>(
		T data,
		string? paramName = null
	) 
		=> paramName == null ? new Msg<T>(data) : new Msg<T>(data, paramName);
}
