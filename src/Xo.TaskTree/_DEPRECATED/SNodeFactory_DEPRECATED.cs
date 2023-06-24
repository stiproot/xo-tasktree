///// <inheritdoc cref="INodeFactory"/>
//public static class SNodeFactory
//{
	//private static ILogger? _logger;

	//public static void Init(ILogger logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

	///// <inheritdoc />
	//public static INode Create() => Create(_logger);

	///// <inheritdoc />
	//public static INode Create(string id) => Create(_logger, id: id);

	///// <inheritdoc />
	//public static INode Create(IWorkflowContext context) => Create(_logger, context: context);

	//public static INode Create(
		//ILogger? logger = null,
		//string? id = null,
		//IWorkflowContext? context = null
	//)
		//=> new Node(logger, id, context);
//}
