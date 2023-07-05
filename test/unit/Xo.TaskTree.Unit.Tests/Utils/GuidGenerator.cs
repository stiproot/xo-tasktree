namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public static class GuidGenerator
{
	public static string NewGuidAsString() => Guid.NewGuid().ToString();
}
