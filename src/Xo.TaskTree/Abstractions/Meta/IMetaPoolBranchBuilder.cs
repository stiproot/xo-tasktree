namespace Xo.TaskTree.Abstractions;

public interface IMetaPoolBranchBuilder : IBranchBuilder
{
	IMetaPoolBranchBuilder Init(IMetaNode metaNode);
}