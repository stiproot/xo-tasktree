namespace Xo.TaskTree.Abstractions;

public interface INodeConfigurationBuilder
{
	Type ServiceType { get; }
	INodeConfigurationBuilder SetId(string id);
	INodeConfigurationBuilder AddContext(IWorkflowContext? workflowContext);
	INodeConfigurationBuilder AddArg(params INode[] args);
	INodeConfigurationBuilder AddArg(params IMetaNode[] args);
	INodeConfigurationBuilder AddArg(params IMsg?[] args);
	INodeConfigurationBuilder AddArg(params Func<IWorkflowContext, IMsg>[] args);
	INodeConfigurationBuilder AddArg<T>(
			T data,
			string paramName
	);
	INodeConfigurationBuilder MatchArg<T>(T arg);
	INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null);
	INodeConfigurationBuilder MatchArgs<T>(T arg);
	INodeConfigurationBuilder RequireResult();
	INodeConfigurationBuilder IgnorePromisedResults();
	INodeConfigurationBuilder AddServiceType(Type serviceType);
	INodeConfigurationBuilder NextParam(string nextParamName);
	INodeConfigurationBuilder Key(string key);
	INodeConfigurationBuilder ControllerType(ControllerTypes controllerType);
	INodeConfiguration Configuration();
}