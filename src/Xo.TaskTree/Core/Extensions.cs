namespace Xo.TaskTree.Core;

internal static class Extensions
{
    public static IMsg?[] ToArray(this IMsg? @this)
    {
        if(@this is null) return new IMsg?[0];

        return new IMsg?[1]{ @this };
    }
}

internal static class LambdaExtensions
{
    public static INodeConfiguration? Build(this Action<INodeConfigurationBuilder>? @this)
    {
        if(@this is null) return null;

        var builder = new NodeConfigurationBuilder();
        @this(builder);
        return builder.Build();
    }

    public static INode BuildNode(this Action<IFlowBuilder> @this)
    {
        var builder = new FlowBuilder();
        @this(builder);
        return builder.Build();
    }
}

internal static class MetaExtensions
{
    public static IMetaNode Configure(this IMetaNode @this, 
        INodeConfiguration? configuration
    )
    {
        if(configuration is null) return @this;

        @this.NodeConfiguration = configuration;

        // todo: should this not be removed?... as it is in config...
        @this.PromisedArgs.AddRange(configuration.PromisedArgs);
        @this.Args.AddRange(configuration.Args);

        return @this;
    }
}

internal static class TypeExtensions
{
    /* todo: this is sexy... but benchmarks need to be done... */
    public static IMetaNode ToNode(this Type @this, 
        Action<INodeConfigurationBuilder>? configure = null,
        MetaNodeTypes nodeType = MetaNodeTypes.Default
    )
        => new MetaNode(@this) { NodeType = nodeType }.Configure(configure.Build());
}