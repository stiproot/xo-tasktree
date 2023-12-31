namespace Xo.TaskTree.Core;

/// <summary>
///   <see cref="Type"/> utility. 
/// </summary>
public static class TypeInspector
{
	private readonly static Type TaskType = typeof(Task);
	private readonly static Type GenericTaskType = typeof(Task<>);

	/// <summary>
	///   Determines if a method returns a Task or Task{T}. 
	/// </summary>
	/// <param name="methodInfo"><see cref="MethodInfo"/></param>
	/// <returns><see cref="bool"/></returns>
	public static bool MethodHasReturnTypeOfTask(MethodInfo methodInfo)
	{
		var returnParameterType = methodInfo.ReturnParameter.ParameterType;
		return returnParameterType == TaskType || returnParameterType.BaseType == TaskType;
	}

	public static string MatchReturnTypeToParamType(
		in Type @out,
		in Type @in
	)
	{
		var outReturnType = @out
				.GetMethods()
				.First()
				.ReturnType;

		string? argReturnTypeName;
		if (outReturnType.IsGenericType && outReturnType.GetGenericTypeDefinition() == GenericTaskType)
		{
			Type genericArgument = outReturnType.GetGenericArguments()[0];
			argReturnTypeName = genericArgument.Name;
		}
		else
		{
			argReturnTypeName = outReturnType.Name;
		}

		var inParamName = @in
				.GetMethods()
				.First()
				.GetParameters()
				.First(p => p.ParameterType.Name == argReturnTypeName)
				.Name;

		return inParamName!;
	}

	public static string MatchTypeToParamType(
		INodeConfiguration nodeConfiguration,
		in Type @out,
		in Type @in
	)
	{
		string? argType = @out.Name;

		// todo: optimize this... store paramater metadata when the service type loaded...
		var inParamName = @in
				.GetMethods()
				.First()
				.GetParameters()
				.First(p => p.ParameterType.Name == argType && !nodeConfiguration.Args.Any(a => a.ParamName == p.Name))
				.Name;

		return inParamName!;
	}

	public static string MatchTypeToParamType(
		in Type @out,
		in Type @in
	)
	{
		string? argReturnTypeName = @out.Name;

		var inParamName = @in
				.GetMethods()
				.First()
				.GetParameters()
				.First(p => p.ParameterType.Name == argReturnTypeName)
				.Name;

		return inParamName!;
	}

	public static Type ReturnType(
		in Type @out
	)
	{
		return @out
				.GetMethods()
				.First()
				.ReturnType;
	}

	public static string FirstParam(in Type @in)
	{
		var inParamName = @in
				.GetMethods()
				.First()
				.GetParameters()
				.First()
				.Name;

		return inParamName!;
	}
}
