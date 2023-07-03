namespace Xo.TaskTree.Abstractions;

public interface IMetaNode
{
	Type FunctoryType { get; init; }
	MetaNodeTypes NodeType { get; set; }
	IMetaNodeEdge? NodeEdge { get; set; }
	INodeConfiguration NodeConfiguration { get; init; }
}