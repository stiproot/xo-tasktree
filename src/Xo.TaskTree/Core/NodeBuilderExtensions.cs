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

	/// <inheritdoc />
	public static INodeBuilder AddArg<TArg>(this INodeBuilder @this,
		TArg arg
	)
	{
		var type = @this.ServiceType;

		if (type is null) throw new InvalidOperationException("Unable to find fn type...");

		string paramName = MatchArgToTypeMethodParam<TArg>(type);

		var msg = SMsgFactory.Create<TArg>(arg, paramName);

		@this.AddArg(msg);

		return @this;
	}

	private static string MatchArgToTypeMethodParam<TArg>(Type type)
	{
		// todo: assumes first method...
		var method = type.GetMethods().First()!;

		var parameters = method.GetParameters();

		string? paramName = null;

		if (parameters.Length == 1) return parameters[0].Name!;

		var argType = typeof(TArg);
		paramName = parameters.Where(p => p.ParameterType == argType).Select(p => p.Name).FirstOrDefault();

		if (paramName is null) throw new InvalidOperationException($"Unable to find parameter name for type TArg:{argType.Name}...");

		return paramName;
	}

	public static IMsg? CloneMsg(IMsg? msg)
	{
		if (msg is null) return null;

		var clone = FnFactory.CreateMsg(msg.ObjectData, msg.ParamName);

		return clone;
	}
}
