namespace Xo.TaskTree.Abstractions;

public interface IMetaBranchBuilder : IBranchBuilder
{
	INode Build(IMetaNodeMapper metaNodeMapper);
}