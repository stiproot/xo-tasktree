namespace Xo.TaskTree.Abstractions;

public interface IGenericNodeBuilder
{
	bool HasParam(string paramName);
	ICoreNodeBuilder AddFunctory(
		Type serviceType, 
		string? nextParamName = null
	);
	ICoreNodeBuilder AddFunctory<TService, TArg>(TArg arg, string? nextParamName = null);
	ICoreNodeBuilder AddArg<TArg>(TArg arg);
	ICoreNodeBuilder AddArg<TArgData>(
		TArgData data,
		string paramName
	);
	ICoreNodeBuilder AddArg<TService, TArg>(TArg arg);
	ICoreNodeBuilder AddArg<TService>();
	ICoreNodeBuilder AddArg(
		Type serviceType,
		IMsg[]? serviceTypeArgs = null
	);
}