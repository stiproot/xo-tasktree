namespace Xo.TaskTree.Core;

public static class ObjectExtensions
{
	public static void ThrowIfNull(this object? @this)
	{
		if (@this is null) throw new InvalidOperationException();
	}
}