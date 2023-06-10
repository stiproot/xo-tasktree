namespace Xo.TaskTree.Abstractions;

public interface IPoolBranchNodeBuilder : IBranchNodeBuilder
{
	IPoolBranchNodeBuilder AddNext(INode node);
	IPoolBranchNodeBuilder AddNext(params INode[] node);
	IPoolBranchNodeBuilder AddNext<T>(bool requiresResult = true);
}