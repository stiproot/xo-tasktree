//namespace Xo.TaskTree.Unit.Tests;

//[ExcludeFromCodeCoverage]
//public class LinkedBranchNodeTests : BaseBranchTests
//{
	//public LinkedBranchNodeTests(
		//IFunctitect functitect,
		//INodeFactory nodeFactory,
		//IMsgFactory msgFactory,
		//IWorkflowContextFactory workflowContextFactory,
		//INodeBuilderFactory nodeBuilderFactory
	//) : base(functitect, nodeFactory, msgFactory, workflowContextFactory, nodeBuilderFactory) { }

	//[Fact]
	//public async Task LinkedBranchNodeTest()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();

		//var linked = this._NodeFactory.Create()
										//.SetFunctory(this._Functitect.BuildAsyncFunctory<Mocked.IY_InStr_OutConstInt_AsyncService>())
										//.SetExceptionHandler(Substitute.For<Action<Exception>>())
										//.AddArg(this._MsgFactory.Create<string>("blah blah", "args"));

		//// The output of this Node will be used as a param for the Task produced by the functory contained in Node n2, "flag2" of type bool.
		//var n1 = this._NodeFactory.Create()
										//.SetFunctory(this._Functitect.Build<Mocked.IY_InStr_OutBool_AsyncService>(nextParamName: "flag2").AsAsync())
										//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										//.AddArg(this._MsgFactory.Create<string>("some string", "args"));

		//// The output of this Node will be used as a param for the Task produced by the functory contained in Node n3, "args3" of type string.
		//// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's functory factory.
		//var n2 = this._NodeFactory.Linked()
										//.SetNext(linked)
										//.SetFunctory(this._Functitect.BuildAsyncFunctory<Mocked.IY_InObjBool_OutStr_AsyncService>())
										//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										//.AddArg(this._MsgFactory.Create<object>(new object(), "args2"))
										//.AddArg(n1);

		//// Act
		//var msg = await n2.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task LinkedBranchBuilderTest()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();

		//var p0 = this._NodeBuilderFactory.Create()
										//.AddFunctory<Mocked.IY_InObjBool_OutStr_AsyncService>(nextParamName: "args3")
										//.AddArg(new object())
										//.AddArg(true)
										//.Build();

		//var linked = this._NodeBuilderFactory.Create()
										//.AddFunctory<Mocked.IY_InStr_OutConstInt_AsyncService>()
										//.RequireResult()
										//.Build();

		//var root = this._NodeBuilderFactory.Linked()
										//.SetNext(linked)
										//.AddFunctory<Mocked.IY_InStrBool_OutStr_AsyncService>()
										//.AddArg(true)
										//.AddArg(p0)
										//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}
//}
