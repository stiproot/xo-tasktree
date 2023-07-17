namespace Xo.TaskTree.Abstractions;

public interface IMetaBranchBuilder
{
	IMetaBranchBuilder Validate(IMetaNode metaNode);

	INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode metaNode
	);
}