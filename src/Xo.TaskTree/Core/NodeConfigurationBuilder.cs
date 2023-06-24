namespace Xo.TaskTree.Core;

public class NodeConfigurationBuilder : INodeConfigurationBuilder
{
    private readonly INodeConfiguration _config = new NodeConfiguration();
    private Type? _functoryType;

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

    public INodeConfigurationBuilder Key(string key)
    {
        this._config.Key = key;
        return this;
    }

    public INodeConfigurationBuilder AddArg<T>(
        T data, 
        string paramName
    )
    {
        var msg = new Msg<T>(data, paramName);

        this._config.Args.Add(msg);

        return this;
    }

    public INodeConfigurationBuilder MatchArg<T>(T arg)
    {
        // todo: how to get param name in this context?...
        string paramName = this._functoryType!.GetMethods().First().GetParameters().First().Name!;

        var msg = new Msg<T>(arg, paramName);

        this._config.Args.Add(msg);

        return this;
    }

    public INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null) 
    {
        var arg = new MetaNode(typeof(T));

        if(configure is not null)
        {
            var configBuilder = new NodeConfigurationBuilder();
            configure(configBuilder);
            var config = configBuilder.Build();

            arg.NodeConfiguration = config;
        }

        this._config.PromisedArgs.Add(arg);

        return this;
    }

    // todo: delay build operation, or just reference config directly -> Build() is misleading.
    public INodeConfiguration Build() => this._config;

    public NodeConfigurationBuilder() { }
    public NodeConfigurationBuilder(Type functoryType) => this._functoryType = functoryType;
    
}