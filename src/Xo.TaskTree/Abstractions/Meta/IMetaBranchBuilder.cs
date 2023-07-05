namespace Xo.TaskTree.Abstractions;

public interface IMetaBranchBuilder
{
	IMetaBranchBuilder Validate();
	IMetaBranchBuilder Init(IMetaNode metaNode);
	INode Build(IMetaNodeMapper metaNodeMapper);
}