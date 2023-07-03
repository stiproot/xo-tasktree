//namespace Xo.TaskTree.Unit.Tests;

//[ExcludeFromCodeCoverage]
//public class BinaryBranchBuilderTests : BaseBranchTests
//{
	//public BinaryBranchBuilderTests(
		//IFnFactory fnFactory,
		//INodeFactory nodeFactory,
		//IMsgFactory msgFactory,
		//IWorkflowContextFactory workflowContextFactory,
		//INodeBuilderFactory nodeBuilderFactory
	//) : base(fnFactory, nodeFactory, msgFactory, workflowContextFactory, nodeBuilderFactory) { }

	//[Fact]
	//public async Task Single()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();
		
		//// The output of this Node will be used as a param for the Task produced by the fn contained in Node n2, "flag2" of type bool.
		//var n1 = this._NodeFactory
			//.Create(context)
			//.SetFn(this._FnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
			//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			//.AddArg(this._MsgFactory.Create<string>("some string", "args"));

		//var binary = this._NodeBuilderFactory.Create<IBinaryBranchBuilder>(NodeBuilderTypes.Binary) 
			//.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService, string>("blah blah", requiresResult: false)
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("branch2", requiresResult: false)
			//.AddFn(new SyncFnAdapter(p => () => this._MsgFactory.Create((p["flag2"] as BaseMsg<bool>)!.GetData())).SetNextParamName("flag3").AsSync())
			//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			//.AddArg(this._MsgFactory.Create<object>(new object(), "args2"))
			//.AddArg(n1)
			//.Build();

		//// Act
		//var msg = await binary.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}
//}
