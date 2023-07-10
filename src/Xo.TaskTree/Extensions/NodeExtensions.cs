namespace Xo.TaskTree.Core;

internal static class NodeExtensions
{
	public static INode AddArg(this INode @this,
		IList<IMsg?> args
	)
	{
		@this.NodeConfiguration.Args.AddRange(args.NonNull());
		return @this;
	}
}