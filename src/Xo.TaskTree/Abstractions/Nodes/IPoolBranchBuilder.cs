namespace Xo.TaskTree.Abstractions;

public interface IPoolBranchBuilder : IBranchBuilder
{
	IPoolBranchBuilder AddNext(INode node);
	IPoolBranchBuilder AddNext(params INode[] node);
	IPoolBranchBuilder AddNext<T>(bool requiresResult = true);
}