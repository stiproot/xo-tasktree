namespace Xo.TaskTree.Abstractions;

public interface ILinkedBranchBuilder : IBranchBuilder
{
	ILinkedBranchBuilder SetNext<T>(bool requiresResult = true);
	ILinkedBranchBuilder SetNext(INode node);
}