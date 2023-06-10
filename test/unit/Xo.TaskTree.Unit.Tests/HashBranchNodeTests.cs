namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class HashBranchNodeTests : BaseBranchNodeTests
{
	public HashBranchNodeTests(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		IWorkflowContextFactory workflowContextFactory,
		INodeBuilderFactory nodeBuilderFactory
	) : base(functitect, nodeFactory, msgFactory, workflowContextFactory, nodeBuilderFactory) { }

	[Fact]
	public async Task HashBranchNodeTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();

		var branch1 = this._NodeFactory.Create()
										.SetFunctory(this._Functitect.Build(typeof(Mocked.IY_InStr_OutConstInt_AsyncService), nameof(Mocked.IY_InStr_OutConstInt_AsyncService.GetConstIntAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._MsgFactory.Create<string>("blah blah", "args"));

		var branch2 = this._NodeFactory.Create()
										.SetFunctory(this._Functitect.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>());

		// The output of this Node will be used as a param for the Task produced by the functory contained in Node n2, "flag2" of type bool.
		var n1 = this._NodeFactory.Create()
										.SetFunctory(this._Functitect.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._MsgFactory.Create<string>("some string", "args"));

		// The output of this Node will be used as a param for the Task produced by the functory contained in Node n3, "args3" of type string.
		// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's functory factory.
		var n2 = this._NodeFactory.Create()
										.SetFunctory(this._Functitect.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._MsgFactory.Create<object>(new object(), "args2"))
										.AddArg(n1);

		var hash = this._NodeFactory.Hash()
			.AddNext(nameof(branch1), branch1)
			.AddNext(nameof(branch2), branch2)
			.AddArg(this._MsgFactory.Create<bool>(true, "flag3"))
			.AddArg(n2)
			.SetFunctory(args =>
			{
				var msg = args["args3"] as BaseMsg<string>;
				string data = msg!.GetData();
				if (data == string.Empty) return () => this._MsgFactory.Create(nameof(branch1));
				throw new NotSupportedException("We should not reach this point");
			})
			.SetExceptionHandler(Substitute.For<Action<Exception>>());

		// Act
		var msg = await hash.Run(cancellationToken);
		var i = (msg as BaseMsg<int>)!.GetData();

		// Assert
		Assert.Equal(1, i);
	}

	[Fact]
	public async Task HashBranchNodeBuilderTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();

		var branch1 = this._NodeBuilderFactory.Create()
										.AddFunctory<Mocked.IY_InStr_OutConstInt_AsyncService>()
										.AddArg("blah blah")
										.Build();

		var branch2 = this._NodeFactory.Create()
										.SetFunctory(this._Functitect.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>());

		// The output of this Node will be used as a param for the Task produced by the functory contained in Node n2, "flag2" of type bool.
		var n1 = this._NodeBuilderFactory.Create()
										.AddFunctory<Mocked.IY_InStr_OutBool_AsyncService>(nextParamName: "flag2")
										.AddArg(this._MsgFactory.Create<string>("some string", "args"))
										.Build();

		// The output of this Node will be used as a param for the Task produced by the functory contained in Node n3, "args3" of type string.
		// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's functory factory.
		var n2 = this._NodeBuilderFactory.Create()
										.AddFunctory<Mocked.IY_InObjBool_OutStr_AsyncService>(nextParamName: "args3")
										.AddArg(this._MsgFactory.Create<object>(new object(), "args2"))
										.AddArg(n1)
										.Build();

		var hash = this._NodeBuilderFactory.Hash()
			.AddNext(nameof(branch1), branch1)
			.AddNext(nameof(branch2), branch2)
			.AddArg(this._MsgFactory.Create<bool>(true, "flag3"))
			.AddArg(n2)
			.AddFunctory(args =>
			{
				var msg = args["args3"] as BaseMsg<string>;
				string data = msg!.GetData();
				if (data == string.Empty) return () => this._MsgFactory.Create(nameof(branch1));
				throw new NotSupportedException("We should not reach this point");
			})
			.Build();

		// Act
		var msg = await hash.Run(cancellationToken);
		var i = (msg as BaseMsg<int>)!.GetData();

		// Assert
		Assert.Equal(1, i);
	}
}
