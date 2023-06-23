namespace Xo.TaskTree.Abstractions;

public interface IMetaPathBranchBuilder : IMetaBranchBuilder
{
	IMetaPathBranchBuilder Init(IMetaNode metaNode);
}