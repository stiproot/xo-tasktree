namespace Xo.TaskTree.Core;

public static class EnumerableExtensions
{
	public static T? Second<T>(this T?[] @this)
	{
		if (@this is null) throw new InvalidOperationException();
		if (@this.Length < 2) throw new InvalidOperationException();

		return @this[1];
	}
}
