namespace Xo.TaskTree.Abstractions;

public interface IInvoker
{
	Task<IMsg?> Invoke(IMsg? msg);
}