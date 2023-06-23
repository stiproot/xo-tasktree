namespace Xo.TaskTree.Abstractions;

public interface IMetaBranchBuilder : IBranchBuilder
{
	IMetaBranchBuilder Init(IMetaNode metaNode);
	INode Build(IMetaNodeMapper metaNodeMapper);
}