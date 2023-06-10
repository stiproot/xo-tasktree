namespace Xo.TaskTree.Abstractions;

public interface ILinkedBranchNodeBuilder : IBranchNodeBuilder
{
	ILinkedBranchNodeBuilder SetNext<T>(bool requiresResult = true);
	ILinkedBranchNodeBuilder SetNext(INode node);
}