namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Builder for constructing a functory around a service that is provided as <see cref="System.Type"/>.
///   The service is retrieved using <see cref="IServiceCollection"/>, and therefore must be registered.
///   The method to invoke can be either sync or async.
///   WARNING: Reflection is used for the construction of a functory factory, this builder should therefore only be used when necessary :)
/// </summary>
public interface IFunctitect
{
	/// <summary>
	///   Builds a functory around a service provided as a generic type param.
	/// </summary>
	/// <typeparam name="T">The type of the service that a functory will be built around.</param>
	/// <param name="nextParamName">The name of the parameter of the next functory that the result of this functory should be fed into as an argument.</param>
	/// <returns><see cref="IFunctory"/></returns>
	IFunctory Build<T>(string? nextParamName = null);

	/// <summary>
	///   Builds a functory around a service provided as a generic type param.
	/// </summary>
	/// <typeparam name="TService">The type of the service that a functory will be built around.</param>
	/// <typeparam name="TArg">The type of the argument that will be passed to the service.</param>
	/// <param name="arg">The argument that will be passed to the service.</param>
	/// <param name="nextParamName">The name of the parameter of the next functory that the result of this functory should be fed into as an argument.</param>
	/// <returns><see cref="IFunctory"/></returns>
	IFunctory Build<TService, TArg>(
		TArg arg,
		string? nextParamName = null
	);

	/// <summary>
	///   Builds a functory around a service provided as <see cref="System.Type"/> and method name. 
	///   The result of method can be used as a argument for the next functory by providing `nextParamName`.
	/// </summary>
	/// <param name="serviceType">The <see cref="System.Type"/> of the service that contains the, sync or async, method to be invoked.</param>
	/// <param name="methodName">The name of the method to be invoked.</param>
	/// <param name="nextParamName">The name of the parameter of the next functory that the result of this functory should be fed into as an argument.</param>
	/// <param name="staticArgs">Static arguments that will be passed to the method to be invoked.</param>
	/// <returns><see cref="IFunctory"/></returns>
	IFunctory Build(
		Type serviceType,
		string? methodName = null,
		string? nextParamName = null,
		object[]? staticArgs = null
	);

	/// <summary>
	///   Builds a functory around a service provided as <see cref="System.Type"/> and method name. 
	///   The result of method can be used as a argument for the next functory by providing `nextParamName`.
	/// </summary>
	/// <typeparam name="T">The type of the service that a functory will be built around.</param>
	/// <param name="methodName">The name of the method to be invoked.</param>
	/// <returns><see cref="IFunctory"/></returns>
	IAsyncFunctory BuildAsyncFunctory<T>(
		string? methodName = null
	);

	/// <summary>
	///   Builds a functory around a service provided as <see cref="System.Type"/> and method name. 
	///   The result of method can be used as a argument for the next functory by providing `nextParamName`.
	/// </summary>
	/// <typeparam name="T">The type of the service that a functory will be built around.</param>
	/// <param name="methodName">The name of the method to be invoked.</param>
	/// <returns><see cref="IFunctory"/></returns>
	ISyncFunctory BuildSyncFunctory<T>(
		string? methodName = null
	);
}
