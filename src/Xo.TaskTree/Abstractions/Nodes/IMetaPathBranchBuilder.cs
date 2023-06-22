namespace Xo.TaskTree.Abstractions;

public interface IMetaPathBranchBuilder : IBranchBuilder
{
	IMetaPathBranchBuilder Init(IMetaNode metaNode);
}