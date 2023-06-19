namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class BinaryBranchBuilderTests : BaseBranchNodeTests
{
	public BinaryBranchBuilderTests(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		IWorkflowContextFactory workflowContextFactory,
		INodeBuilderFactory nodeBuilderFactory
	) : base(functitect, nodeFactory, msgFactory, workflowContextFactory, nodeBuilderFactory) { }

	[Fact]
	public async Task BinaryBranchUsingFunctitectInternallyTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var context = this._WorkflowContextFactory.Create();
		

		// The output of this Node will be used as a param for the Task produced by the functory contained in Node n2, "flag2" of type bool.
		var n1 = this._NodeFactory
			.Create(context)
			.SetFunctory(this._Functitect.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.AddArg(this._MsgFactory.Create<string>("some string", "args"));

		var binary = this._NodeBuilderFactory.Binary(context)
			.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService, string>("blah blah", requiresResult: false)
			.AddFalse<Mocked.IY_InStr_AsyncService, string>("branch2", requiresResult: false)
			.AddFunctory(new SyncFunctoryAdapter(p => () => this._MsgFactory.Create((p["flag2"] as BaseMsg<bool>)!.GetData())).SetNextParamName("flag3").AsSync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.AddArg(this._MsgFactory.Create<object>(new object(), "args2"))
			.AddArg(n1)
			.Build();

		// Act
		var msg = await binary.Run(cancellationToken);
		var i = (msg as BaseMsg<int>)!.GetData();

		// Assert
		Assert.Equal(1, i);
	}
}
