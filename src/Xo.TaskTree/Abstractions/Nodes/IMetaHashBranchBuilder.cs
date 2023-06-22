namespace Xo.TaskTree.Abstractions;

public interface IMetaHashBranchBuilder : IBranchBuilder
{
	IMetaHashBranchBuilder Init(IMetaNode metaNode);
}