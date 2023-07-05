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

	public static INode AddArg(this INode @this,
		IMsg arg
	)
	{
		@this.NodeConfiguration.Args.Add(arg);
		return @this;
	}

	public static bool HasParam(this INode @this,
		string paramName
	)
		=> @this.NodeConfiguration.Args.Any(p => p.ParamName == paramName);
}