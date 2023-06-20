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

    public INodeConfigurationBuilder MatchArg<T>(T arg)
    {
        // todo: how to get param name in this context?...

        var msg = new Msg<T>(arg);

        this._config.Args.Add(msg);

        return this;
    }

    public INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null) 
    {
        var arg = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.PromisedArgMatch };

        // todo: process configure...

        this._config.PromisedArgs.Add(arg);

        return this;
    }

    // todo: delay build operation, or just reference config directly -> Build() is misleading.
    public INodeConfiguration Build() => this._config;
}