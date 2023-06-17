namespace Xo.TaskTree.Abstractions;

public interface IInvoker
{
	Task<IMsg?> Invoke(IMsg? msg);
	Task<IMsg?> Invoke(
		INodeEdge nodeEdge, 
		IMsg? msg
	);
}