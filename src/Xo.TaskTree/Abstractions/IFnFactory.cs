namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Builder for constructing a fn around a service that is provided as <see cref="System.Type"/>.
///   The service is retrieved using <see cref="IServiceCollection"/>, and therefore must be registered.
///   The method to invoke can be either sync or async.
///   WARNING: Reflection is used for the construction of a fn factory, this builder should therefore only be used when necessary :)
/// </summary>
public interface IFnFactory
{
	/// <summary>
	///   Builds a fn around a service provided as a generic type param.
	/// </summary>
	/// <typeparam name="T">The type of the service that a fn will be built around.</param>
	/// <param name="nextParamName">The name of the parameter of the next fn that the result of this fn should be fed into as an argument.</param>
	/// <returns><see cref="IFn"/></returns>
	IFn Build<T>(string? nextParamName = null);

	/// <summary>
	///   Builds a fn around a service provided as <see cref="System.Type"/> and method name. 
	///   The result of method can be used as a argument for the next fn by providing `nextParamName`.
	/// </summary>
	/// <param name="serviceType">The <see cref="System.Type"/> of the service that contains the, sync or async, method to be invoked.</param>
	/// <param name="methodName">The name of the method to be invoked.</param>
	/// <param name="nextParamName">The name of the parameter of the next fn that the result of this fn should be fed into as an argument.</param>
	/// <param name="staticArgs">Static arguments that will be passed to the method to be invoked.</param>
	/// <returns><see cref="IFn"/></returns>
	IFn Build(
		Type serviceType,
		string? methodName = null,
		string? nextParamName = null
	);
}
