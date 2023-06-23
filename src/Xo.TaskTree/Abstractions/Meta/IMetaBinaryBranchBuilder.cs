namespace Xo.TaskTree.Abstractions;

public interface IMetaBinaryBranchBuilder : IMetaBranchBuilder
{
	IMetaBinaryBranchBuilder Init(IMetaNode metaNode);
}