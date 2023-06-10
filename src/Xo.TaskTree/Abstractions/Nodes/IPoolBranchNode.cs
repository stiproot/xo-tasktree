namespace Xo.TaskTree.Abstractions;

public interface IPoolBranchNode : IBranchNode
{
	IPoolBranchNode AddNext(INode node);
	IPoolBranchNode AddNext(params INode[] node);
}