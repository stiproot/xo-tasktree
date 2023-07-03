namespace Xo.TaskTree.Core;

internal static class MsgExtensions
{
	public static IMsg?[] ToArray(this IMsg? @this)
	{
		if (@this is null) return new IMsg?[0];

		return new IMsg?[1] { @this };
	}

	public static IArgs AsArgs(this IList<IMsg> @this) => new Args(@this);
	public static IArgs AsArgs(this List<IMsg> @this) => new Args(@this);
	public static IArgs AsArgs(this IReadOnlyCollection<IMsg> @this) => new Args(@this.ToList());
}