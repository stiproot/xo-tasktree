namespace Xo.TaskTree.Core;

/// <inheritdoc cref="IFnFactory"/>
public sealed class FnFactory : IFnFactory
{
	private readonly IServiceProvider _serviceProvider;
	private static readonly Type _msgType = typeof(Msg<>);
	private static readonly Type _taskType = typeof(Task);

	/// <summary>
	///   Initializes a new instance of <see cref="FnFactory"/>.
	/// </summary>
	/// <param name="serviceProvider">Service provider used to retrived registered services by their type.</param>
	public FnFactory(IServiceProvider serviceProvider)
		=> this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

	public IFn Build<T>(string? nextParamName = null) 
		=> this.Build(typeof(T), nextParamName: nextParamName);

	/// <inheritdoc />
	public IFn Build(
		Type serviceType,
		string? methodName = null,
		string? nextParamName = null
	)
	{
		var service = this.GetService(serviceType);

		var methodInfo = GetMethodInfo(serviceType, methodName);

		var parameters = methodInfo.GetParameters();

		Func<IArgs, Task<IMsg?>> fn = async (args) =>
			{
				ValidateMethod(args, methodInfo, parameters);

				var arguments = GetArguments(args, parameters);

				object? result = null;

				if (TypeInspector.MethodHasReturnTypeOfTask(methodInfo))
				{
					var task = (Task)methodInfo.Invoke(service, arguments)!;

					await task;

					if(methodInfo.ReturnType != _taskType) result = task.GetType().GetProperty("Result")?.GetValue(task);
				}
				else
				{
					result = methodInfo.Invoke(service, arguments);
				}

				return result is null ? null : CreateMsg(result, nextParamName);
			};

		return new FnAdaptor(fn!).SetServiceType(serviceType);
	}

	public IFn BuildAsyncFn<T>(string? methodName = null)
	{
		var serviceType = typeof(T);

		var service = this.GetService(serviceType);

		var methodInfo = GetMethodInfo(serviceType, methodName);

		var parameters = methodInfo.GetParameters();

		Func<IArgs, Task<IMsg?>> fn = async (args) =>
			{
				ValidateMethod(args, methodInfo, parameters);

				var arguments = GetArguments(args, parameters);

				object? result = null;

				var task = (Task)methodInfo.Invoke(service, arguments)!;

				await task;

				if(methodInfo.ReturnType != _taskType) result = task.GetType().GetProperty("Result")?.GetValue(task);

				return result == null ? null : CreateMsg(result, null);
			};

		// todo: clean this up...
		return new FnAdaptor(fn!).SetServiceType(serviceType: typeof(T));
	}

	private object? GetService(Type serviceType)
		=> this._serviceProvider.GetService(serviceType) ?? throw new InvalidOperationException($"Service not found for service type {serviceType.Name}");

	private static void ValidateMethod(
		in IArgs arguments,
		in MethodInfo methodInfo,
		in IEnumerable<ParameterInfo> parameters
	)
	{
		if (parameters.Count() == arguments.Count()) return;

		throw new ArgumentException(
			$"Invalid parameters for method {methodInfo.Name}. " +
			$"Arguments provided: {string.Join(",", arguments.Params())}, " +
			$"Parameters expected: {string.Join(",", parameters.Select(p => p.Name))}"
		);
	}

	public static MethodInfo GetMethodInfo(
		Type type,
		string? methodName = null
	)
		=> methodName switch
		{
			null => type.GetMethods().First(),
			_ => type.GetMethod(methodName!) ?? throw new InvalidOperationException($"{type.Name} does not have method of name {methodName}")
		};

	private static object[] GetArguments(
		IArgs arguments,
		IEnumerable<ParameterInfo> parameters
	)
	{
		if(parameters.Count() is 1) return arguments.ToObjArray();

		return arguments.ToObjArray(parameters);
	}

	public static IMsg CreateMsg(object result, string? nextParamName)
	{
		var resultType = result.GetType();

		var constructedType = _msgType.MakeGenericType(resultType);

		var arguments = nextParamName == null
			? new object[] { result }
			: new object[] { result, nextParamName };

		object? instance = Activator.CreateInstance(constructedType, arguments);

		return (IMsg)instance!;
	}
}