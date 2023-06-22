namespace Xo.TaskTree.Abstractions;

public interface IMetaBinaryBranchBuilder : IBranchBuilder
{
	IMetaBinaryBranchBuilder Init(IMetaNode metaNode);
}