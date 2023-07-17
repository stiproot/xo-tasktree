namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class WorkflowContextTests
{
	private readonly IWorkflowContextFactory _workflowContextFactory;

	public WorkflowContextTests(IWorkflowContextFactory workflowContextFactory)
		=> this._workflowContextFactory = workflowContextFactory ?? throw new ArgumentNullException(nameof(workflowContextFactory));

	[Fact]
	public void WorkflowContextReturnsResultOfType()
	{
		// ARRANGE...
		var key = GuidGenerator.NewGuidAsString();
		var value = DateTime.UtcNow;
		var workflowContext = this._workflowContextFactory.Create();
		workflowContext.AddData<DateTime>(key, value);

		// ACT...
		var result = workflowContext.GetMsgData<DateTime>(key);

		// ASSERT...
		Assert.IsType<DateTime>(result);
		Assert.Equal(value, result);
	}

	[Fact]
	public void WorkflowContextProvidedInvalidKeyThrowsInvalidOperationException()
	{
		// ARRANGE...
		var key = GuidGenerator.NewGuidAsString();
		var workflowContext = this._workflowContextFactory.Create();

		// ACT / ASSERT...
		Assert.Throws<InvalidOperationException>(() => workflowContext.GetMsg(key));
	}

	[Fact]
	public void WorkflowContext_ProvidedInvalidKeyForReferenceType_ThrowsInvalidOperationException()
	{
		// ARRANGE...
		var key = GuidGenerator.NewGuidAsString();
		var workflowContext = this._workflowContextFactory.Create();

		// ACT / ASSERT...
		Assert.Throws<InvalidOperationException>(() => workflowContext.GetMsgData<object>(key));
	}

	[Fact]
	public void WorkflowContext_ProvidedNullKey_ThrowsInvalidOperationException()
	{
		// ARRANGE...
		var workflowContext = this._workflowContextFactory.Create();

		// ACT / ASSERT...
		Assert.Throws<ArgumentNullException>(() => workflowContext.GetMsgs(null!));
	}

	[Fact]
	public void WorkflowContext_ReturnsMultipleResults()
	{
		// ARRANGE...
		var key = GuidGenerator.NewGuidAsString();
		var value = DateTime.UtcNow;
		var key2 = GuidGenerator.NewGuidAsString();
		var value2 = DateTime.UtcNow;

		var workflowContext = this._workflowContextFactory.Create();
		workflowContext.AddData<DateTime>(key, value);
		workflowContext.AddData<DateTime>(key2, value2);

		// ACT...
		var results = workflowContext.GetMsgs(key, key2);

		// ASSERT...
		Assert.Equal(2, results.Count());
	}

	[Fact]
	public void WorkflowContextReturnsMultipleKeyResultPairs()
	{
		// ARRANGE...
		var key = GuidGenerator.NewGuidAsString();
		var value = DateTime.UtcNow;
		var key2 = GuidGenerator.NewGuidAsString();
		var value2 = DateTime.UtcNow;

		var workflowContext = this._workflowContextFactory.Create();
		workflowContext.AddData<DateTime>(key, value);
		workflowContext.AddData<DateTime>(key2, value2);

		// ACT...
		var results = workflowContext.GetKeyMsgPairs(key, key2);

		// ASSERT...
		Assert.Equal(2, results.Count());
		Assert.Equal(key, results.First().Item1);
		Assert.Equal(key2, results.ElementAt(1).Item1);
		Assert.Equal(value, (results.First().Item2 as Msg<DateTime>)!.GetData());
		Assert.Equal(value2, (results.ElementAt(1).Item2 as Msg<DateTime>)!.GetData());
	}

	[Fact]
	public void WorkflowContextReturnsAllKeyResultPairs()
	{
		// ARRANGE...
		var key1 = GuidGenerator.NewGuidAsString();
		var value1 = DateTime.UtcNow;
		var key2 = GuidGenerator.NewGuidAsString();
		var value2 = DateTime.UtcNow;
		var key3 = GuidGenerator.NewGuidAsString();
		var value3 = DateTime.UtcNow;

		// ACT...
		var workflowContext = this._workflowContextFactory.Create();
		workflowContext.AddData(key1, value1);
		workflowContext.AddData(key2, value2);
		workflowContext.AddData(key3, value3);

		// ASSERT...
		var results = workflowContext.GetAllKeyMsgPairs();
		Assert.Equal(3, results.Count());
		var firstPair = results.FirstOrDefault(r => r.Item1.Equals(key1));
		Assert.Equal(key1, firstPair.Item1);
		Assert.Equal(value1, (firstPair.Item2 as Msg<DateTime>)!.GetData());
		var secondPair = results.FirstOrDefault(r => r.Item1.Equals(key2));
		Assert.Equal(key2, secondPair.Item1);
		Assert.Equal(value2, (secondPair.Item2 as Msg<DateTime>)!.GetData());
		var thirdPair = results.FirstOrDefault(r => r.Item1.Equals(key3));
		Assert.Equal(key3, thirdPair.Item1);
		Assert.Equal(value3, (thirdPair.Item2 as Msg<DateTime>)!.GetData());
	}
}