//namespace Xo.TaskTree.Unit.Tests;

//[ExcludeFromCodeCoverage]
//public class BinaryBranchNodeTests : BaseBranchTests
//{
	//public BinaryBranchNodeTests(
		//IFnFactory functitect,
		//INodeFactory nodeFactory,
		//IMsgFactory msgFactory,
		//IWorkflowContextFactory workflowContextFactory,
		//INodeBuilderFactory nodeBuilderFactory
	//) : base(functitect, nodeFactory, msgFactory, workflowContextFactory, nodeBuilderFactory) { }

	//[Fact]
	//public async Task BinaryBranchTest()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var branch1 = this._NodeFactory.Create(context)
										//.SetFn(this._FnFactory.Build(typeof(Mocked.IY_InStr_OutConstInt_AsyncService), nameof(Mocked.IY_InStr_OutConstInt_AsyncService.GetConstIntAsync)).AsAsync())
										//.SetExceptionHandler(Substitute.For<Action<Exception>>())
										//.AddArg(this._MsgFactory.Create<string>("blah blah", "args"));

		//var branch2 = this._NodeFactory.Create(context)
										//.SetFn(this._FnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										//.SetExceptionHandler(Substitute.For<Action<Exception>>());
		//branch2.AddArg(this._MsgFactory.Create<string>(nameof(branch2)));

		//var n1 = this._NodeFactory.Create(context)
										//.SetFn(this._FnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										//.AddArg(this._MsgFactory.Create<string>("some string", "args"));

		//var binary = this._NodeFactory.Binary(context)
			//.AddTrue(branch1)
			//.AddFalse(branch2)
			//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			//.AddArg(this._MsgFactory.Create<object>(new object(), "args2"))
			//.AddArg(n1)
			//.SetFn(new SyncFnAdapter(p => () => this._MsgFactory.Create((p["flag2"] as BaseMsg<bool>)!.GetData())).SetNextParamName("flag3").AsSync());

		//// Act
		//var msg = await binary.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BinaryBranchUsingFnFactoryInternallyTest()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//// The output of this Node will be used as a param for the Task produced by the functory contained in Node n2, "flag2" of type bool.
		//var n1 = this._NodeFactory.Create(context)
										//.SetFn(this._FnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										//.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										//.AddArg(this._MsgFactory.Create<string>("some string", "args"));

		//var binary = this._NodeBuilderFactory.Binary(context)
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

	//[Fact]
	//public async Task BuildingBinaryNode()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//// The output of this Node will be used as a param for the Task produced by the functory contained in Node n2, "flag2" of type bool.
		//var p0 = this._NodeBuilderFactory.Create(context)
			//.AddFn<Mocked.IY_InStr_OutBool_AsyncService>(nextParamName: "flag2")
			//.AddArg("some string")
			//.Build();

		//var binary = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService, string>("blah blah", requiresResult: false)
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("branch2", requiresResult: false)
			//.AddFn(p => () => this._MsgFactory.Create((p["flag2"] as BaseMsg<bool>)!.GetData()))
			//.AddArg(new object(), "args2")
			//.AddArg(p0)
			//.Build();

		//// Act
		//var msg = await binary.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BuildingSingleBinaryNodeTree()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var binary = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService, string>("blah blah", requiresResult: false)
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.AddFn<Mocked.IY_InStr_OutBool_AsyncService, string>("str-argument-2")
			//.AddArg("str-argument-3", "args")
			//.Build();

		//// Act
		//var msg = await binary.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BuildingSingleBinaryNodeTreeWhereTrueRequiresResult()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var binary = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InBoolStr_OutConstInt_AsyncService, string>("blah blah")
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.AddFn<Mocked.IY_InStr_OutBool_AsyncService, string>("str-argument-2")
			//.AddArg("str-argument-3", "args")
			//.Build();

		//// Act
		//var msg = await binary.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BuildingTwoIndepependentBinaryNodeTree()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var binary0 = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InBoolStr_OutConstInt_AsyncService, string>("blah blah")
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.AddFn<Mocked.IY_InStr_OutBool_AsyncService, string>("str-argument-2")
			//.AddArg("str-argument-3", "args")
			//.Build();

		//var root = this._NodeBuilderFactory.Binary(context)
			//.AddTrue(binary0)
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.AddFn<Mocked.IY_InStr_OutBool_AsyncService, string>("str-argument-2")
			//.AddArg("str-argument-3", "args")
			//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BuildingTwoDepependentBinaryNodeTree()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var binary0 = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InBoolStr_OutConstInt_AsyncService, string>("blah blah")
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.AddFn<Mocked.IY_InObjBool_OutBool_AsyncService, string>("str-argument-2")
			//.AddArg(new object(), "args2")
			//.RequireResult()
			//.Build();

		//var root = this._NodeBuilderFactory.Binary(context)
			//.AddTrue(binary0)
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.AddFn<Mocked.IY_InStr_OutBool_AsyncService, string>("str-argument-2")
			//.AddArg("str-argument-3", "args")
			//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task NullCheckBinaryBranchBuilderTest()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var root = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService>()
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.IsNotNull<Mocked.IY_InObjBool_OutStr_AsyncService, bool>(arg: true)
			//.AddArg(new object(), "args2")
			//.RequireResult()
			//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task NullCheckBinaryBranchBuilderWithNoArgMatchingTest()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var root = this._NodeBuilderFactory.Binary(context)
			//.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService>()
			//.AddFalse<Mocked.IY_InStr_AsyncService, string>("str-argument", requiresResult: false)
			//.IsNotNull<Mocked.IY_InObjBool_OutStr_AsyncService>()
			//.AddArg(new object(), "args2")
			//.AddArg(true, "flag2")
			//.RequireResult()
			//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BinaryNodeTreeWithTrueBranchOnly()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var root = this._NodeBuilderFactory.Binary(context)
			//.IsNotNull<Mocked.IY_InObjBool_OutStr_AsyncService>()
			//.AddTrue<Mocked.IY_InStr_OutConstInt_AsyncService>()
			//.AddArg(new object(), "args2")
			//.AddArg(true, "flag2")
			//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}

	//[Fact]
	//public async Task BinaryNodeTreeWithFalseBranchOnly()
	//{
		//// Arrange
		//var cancellationToken = this.CancellationTokenFactory();
		//var context = this._WorkflowContextFactory.Create();

		//var root = this._NodeBuilderFactory.Binary(context)
			//.IsNotNull<Mocked.IY_InObjBool_OutNullStr_AsyncService>()
			//.AddFalse<Mocked.IY_InStr_OutConstInt_AsyncService, string>(args: "blah blah", requiresResult: false)
			//.AddArg(new object(), "args2")
			//.AddArg(true, "flag2")
			//.Build();

		//// Act
		//var msg = await root.Run(cancellationToken);
		//var i = (msg as BaseMsg<int>)!.GetData();

		//// Assert
		//Assert.Equal(1, i);
	//}
//}
