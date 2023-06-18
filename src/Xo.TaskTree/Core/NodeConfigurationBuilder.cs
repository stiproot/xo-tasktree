namespace Xo.TaskTree.Core;

public class NodeConfigurationBuilder : INodeConfigurationBuilder
{
    private readonly INodeConfiguration _config = new NodeConfiguration();

    public INodeConfigurationBuilder RequireResult()
    {
        this._config.RequiresResult = true;
        return this;
    }

    public INodeConfigurationBuilder NextParam(string nextParamName)
    {
        this._config.NextParamName = nextParamName;
        return this;
    }

    public INodeConfigurationBuilder AddArg<T>()
    {
        // todo: create IMsg, match to param...
        return this;
    }

    public INodeConfigurationBuilder AddArg<T>(string paramName)
    {
        // todo: create IMsg...
        return this;
    }

    public INodeConfigurationBuilder AddArg<T>(
        T data, 
        string paramName
    )
    {
        // todo: use factory?...
        var msg = new Msg<T>(data, paramName);
        this._config.Args.Add(msg);
        return this;
    }

    public INodeConfiguration Build() => this._config;
}