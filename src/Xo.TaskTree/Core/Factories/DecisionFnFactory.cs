namespace Xo.TaskTree.Factories;

internal static class DecisionFnFactory
{
	public static Func<IArgs, IMsg?> Create(
		ControllerTypes? controllerType,
		bool resolveTo = true,
		string? value = null
	)
	{
		return controllerType switch
		{
			ControllerTypes.True => p => p.First()!.SetControlMsg(SMsgFactory.Create<bool>(p.First()!.Data<bool>() == resolveTo)),
			ControllerTypes.IsNotNull => p => p.First()!.SetControlMsg(SMsgFactory.Create<bool>(p.First()!.HasData == resolveTo)),
			ControllerTypes.Equals => p => p.First()!.SetControlMsg(SMsgFactory.Create<bool>(p.First()!.Data<string>().Equals(value) == resolveTo)),
			_ => throw new NotSupportedException()
		};
	}
}
