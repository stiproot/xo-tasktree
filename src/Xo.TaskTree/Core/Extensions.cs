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
    public static INodeConfiguration Build(this Action<INodeConfigurationBuilder> @this)
    {
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
        INodeConfiguration configuration
    )
    {
        @this.NodeConfiguration = configuration;

        // todo: should this not be removed?... as it is in config...
        @this.PromisedArgs.AddRange(configuration.PromisedArgs);
        @this.Args.AddRange(configuration.Args);

        return @this;
    }
}