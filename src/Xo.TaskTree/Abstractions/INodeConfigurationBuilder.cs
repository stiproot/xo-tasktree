namespace Xo.TaskTree.Abstractions;

public interface INodeConfigurationBuilder
{
    INodeConfigurationBuilder RequireResult();
    INodeConfigurationBuilder NextParam(string nextParamName);

    INodeConfigurationBuilder AddArg<T>();
    INodeConfigurationBuilder AddArg<T>(string paramName);
    INodeConfigurationBuilder AddArg<T>(
        T data, 
        string paramName
    );
    INodeConfigurationBuilder MatchArg<T>(T arg);
    INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null);

    INodeConfiguration Build();
}