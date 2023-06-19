namespace Xo.TaskTree.Abstractions;

public interface INodeConfigurationBuilder
{
    INodeConfigurationBuilder RequireResult();
    INodeConfigurationBuilder NextParam(string nextParamName);

    INodeConfigurationBuilder AddArg<T>();
    INodeConfigurationBuilder AddArg<T>(string paramName);
    INodeConfigurationBuilder MatchArg<T>(T arg);
    INodeConfigurationBuilder AddArg<T>(
        T data, 
        string paramName
    );

    INodeConfiguration Build();
}