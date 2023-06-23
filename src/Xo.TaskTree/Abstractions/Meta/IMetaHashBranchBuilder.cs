namespace Xo.TaskTree.Abstractions;

public interface IMetaHashBranchBuilder : IMetaBranchBuilder
{
	IMetaHashBranchBuilder Init(IMetaNode metaNode);
}