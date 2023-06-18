namespace Xo.TaskTree.Core;

internal static class Extensions
{
    public static IMsg?[] ToArray(this IMsg? @this)
    {
        if(@this is null) return new IMsg?[0];

        return new IMsg?[1]{ @this };
    }
}