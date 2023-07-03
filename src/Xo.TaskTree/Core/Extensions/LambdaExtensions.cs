namespace Xo.TaskTree.Core;

internal static class LambdaExtensions
{
    public static INodeConfiguration? Build(this Action<INodeConfigurationBuilder>? @this,
        Type functoryType
    )
    {
        if(@this is null) return null;

        var builder = new NodeConfigurationBuilder(functoryType);

        @this(builder);

        return builder.Build();
    }

    public static INodeConfiguration? SafeBuild(this Action<INodeConfigurationBuilder>? @this,
        Type functoryType
    )
    {
        var builder = new NodeConfigurationBuilder(functoryType);

        if(@this is not null) @this(builder);

        return builder.Build();
    }
}