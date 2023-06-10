namespace Xo.TaskTree.Abstractions;

public interface ILinkedBranchNode : IBranchNode
{
	ILinkedBranchNode SetNext(INode node);
	// ILinkedBranchNode SetNext<T>(bool requiresResult = true);
}