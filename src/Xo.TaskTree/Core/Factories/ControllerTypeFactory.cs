namespace Xo.TaskTree.Core;

public static class ControllerTypeFactory
{
	public static IController Create(ControllerTypes? type)
	{
		return type switch
		{
			// ControllerTypes.True => new TrueController(),
			// ControllerTypes.IsNotNull => new IsNotNullController(),
			_ => new TrueController() 
		};
	}
}
