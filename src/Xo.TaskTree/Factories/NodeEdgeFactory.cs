namespace Xo.TaskTree.Core;

public static class NodeEdgeFactory
{
		public static INodeEdge Create(NodeEdgeTypes type)
		{
			return type switch
			{
				NodeEdgeTypes.Monarius => new MonariusNodeEdge(),
				NodeEdgeTypes.Binarius => new BinariusNodeEdge(),
				NodeEdgeTypes.Multus => new MultusNodeEdge(),
				_ => throw new NotSupportedException()
			};
		}
}
