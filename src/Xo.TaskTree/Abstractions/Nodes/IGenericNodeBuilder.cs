namespace Xo.TaskTree.Abstractions;

public interface IGenericNodeBuilder
{
	bool HasParam(string paramName);
	IGenericNodeBuilder AddFunctory(
		Type serviceType, 
		string? nextParamName = null
	);
	IGenericNodeBuilder AddFunctory<TService, TArg>(TArg arg, string? nextParamName = null);
	IGenericNodeBuilder AddArg<TArg>(TArg arg);
	IGenericNodeBuilder AddArg<TArgData>(
		TArgData data,
		string paramName
	);
	IGenericNodeBuilder AddArg<TService, TArg>(TArg arg);
	IGenericNodeBuilder AddArg<TService>();
	IGenericNodeBuilder AddArg(
		Type serviceType,
		IMsg[]? serviceTypeArgs = null
	);
}