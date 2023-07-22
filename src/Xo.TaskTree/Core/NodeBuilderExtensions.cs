namespace Xo.TaskTree.Core;

public static class NodeBuildeExtensions
{
	/// <inheritdoc />
	public static INodeBuilder AddFn<T>(
		this INodeBuilder @this,
		string? nextParamName = null
	)
	{
		IFn fn = @this.FnFactory.Build<T>(nextParamName).SetServiceType(typeof(T)); 

		@this.AddFn(fn);

		return @this;
	}

	public static IMsg? CloneMsg(IMsg? msg)
	{
		if (msg is null) return null;

		var clone = FnFactory.CreateMsg(msg.ObjectData, msg.ParamName);

		return clone;
	}
}
