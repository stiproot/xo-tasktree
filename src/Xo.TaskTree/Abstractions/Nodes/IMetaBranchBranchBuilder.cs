namespace Xo.TaskTree.Abstractions;

public interface IMetaBranchBranchBuilder : IBranchBuilder
{
	IMetaBranchBranchBuilder Init(IMetaNode metaNode);
}