namespace Xo.TaskTree.Abstractions;

public interface IController
{
	bool Control(IMsg? msg);
}