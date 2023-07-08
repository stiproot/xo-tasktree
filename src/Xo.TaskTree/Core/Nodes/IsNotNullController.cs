namespace Xo.TaskTree.Core;

public class IsNotNullController : IController
{
	public bool Control(IMsg? msg)
	{
		if (msg is null) return false;

		return msg.HasData;
	}
}