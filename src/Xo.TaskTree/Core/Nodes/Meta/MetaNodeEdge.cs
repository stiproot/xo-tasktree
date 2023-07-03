namespace Xo.TaskTree.Core;

public class MetaNodeEdge : IMetaNodeEdge
{
	public IMetaNode? True { get; set; }
	public IMetaNode? False { get; set; }
	public IMetaNode? Next { get; set; }
	public List<IMetaNode?>? Nexts { get; set; }
}