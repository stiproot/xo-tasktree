namespace Xo.TaskTree.Core;

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