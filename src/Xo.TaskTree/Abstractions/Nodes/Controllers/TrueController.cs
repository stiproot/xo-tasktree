namespace Xo.TaskTree.Abstractions;

public class TrueController : IController
{
	public bool Control(IMsg? msg)
	{
		if (msg is null) return false;

		bool data = (msg as Msg<bool>)!.GetData();

		return data;
	}
}