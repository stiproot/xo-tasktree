namespace Xo.TaskTree.Abstractions;

public interface IMetaBranchBuilder : IBranchBuilder
{
	IMetaBranchBuilder Validate();
	IMetaBranchBuilder Init(IMetaNode metaNode);
	INode Build(IMetaNodeMapper metaNodeMapper);
}