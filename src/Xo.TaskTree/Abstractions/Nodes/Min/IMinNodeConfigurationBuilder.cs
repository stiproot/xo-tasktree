namespace Xo.TaskTree.Abstractions;

public interface IMinNodeConfigurationBuilder
{
	IMinNodeConfigurationBuilder SetId(string id);
	IMinNodeConfigurationBuilder SetContext(IWorkflowContext? context);
	IMinNodeConfigurationBuilder AddArg(params IMinNode[] args);
	IMinNodeConfigurationBuilder AddArg(params IMsg?[] args);
	IMinNodeConfigurationBuilder AddArg(params Func<IWorkflowContext, IMsg>[] args);
	IMinNodeConfigurationBuilder AddArg<T>(
			T data,
			string paramName
	);
	IMinNodeConfigurationBuilder MatchArg<T>(T arg);
	IMinNodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null);
	IMinNodeConfigurationBuilder RequireResult();
	IMinNodeConfigurationBuilder AddServiceType(Type serviceType);
	IMinNodeConfigurationBuilder NextParam(string nextParamName);
	IMinNodeConfigurationBuilder Key(string key);
	IMinNodeConfiguration Build();
}