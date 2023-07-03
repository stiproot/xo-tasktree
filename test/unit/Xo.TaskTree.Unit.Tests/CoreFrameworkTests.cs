namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class CoreFrameworkTests
{
	private readonly IFnFactory _fnFactory;
	private readonly INodeFactory _nodeFactory;
	private readonly INodeBuilderFactory _nodeBuilderFactory;
	private readonly IMsgFactory _msgFactory;
	private readonly IWorkflowContextFactory _workflowContextFactory;
	private CancellationToken CancellationTokenFactory() => new CancellationToken();

	public CoreFrameworkTests(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		INodeBuilderFactory nodeBuilderFactory,
		IMsgFactory msgFactory,
		IWorkflowContextFactory workflowContextFactory
	)
	{
		this._fnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._nodeBuilderFactory = nodeBuilderFactory ?? throw new ArgumentNullException(nameof(nodeBuilderFactory));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
		this._workflowContextFactory = workflowContextFactory ?? throw new ArgumentNullException(nameof(workflowContextFactory));
	}

	[Fact]
	public void DependencyInjectionTest()
	{
		Assert.NotNull(this._fnFactory);
	}

	[Fact]
	public void NodeFactoryCanBeSuppliedId()
	{
		// Arrange
		var guid = GuidGenerator.NewGuidAsString();

		// Act
		var th = this._nodeFactory.Create(guid);

		// Assert
		Assert.Equal(guid, th.Id);
	}

	[Fact]
	public async Task FrameworkTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n2, "flag2" of type bool.
		var n1 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<string>("some string", "args"));
		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n3, "args3" of type string.
		// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's fn factory.
		var n2 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.AddArg(n1);
		var n3 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(n2);

		// Act
		await n3.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task UserDefinedStrategyFrameworkTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n2, "flag2" of type bool.
		// We will provide a "user defined fn", IY_InStr_OutBool_AsyncService_Fn.
		// Note that factories should be implemented by the consumer to provide their strategies, if they have gone this route.
		var n1 = this._nodeFactory.Create()
										.SetFn(new Mocked.IY_InStr_OutBool_AsyncService_Fn(new Mocked.Y_InStr_OutBool_AsyncService(), this._msgFactory).SetNextParamName("flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<string>("some string", "args"));
		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n3, "args3" of type string.
		// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's fn factory.
		var n2 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.AddArg(n1);
		var n3 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(n2);

		// Act
		await n3.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task ServiceThatAcceptsNoArgumentsFrameworkTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var n3 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._msgFactory.Create(string.Empty, "args3"));
		// This is the focus of this test. A Node that wraps an async service that takes no arguments.
		var n4 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_AsyncService), nameof(Mocked.IY_AsyncService.ProcessAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(n3);

		// Act
		await n4.Run(cancellationToken);

		// Assert
		Assert.NotNull(n3);
		Assert.NotNull(n4);
	}

	[Fact]
	public async Task SingletonServiceUsedByTwoParamHandlesTest()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		// The point of this test will be to use a service that is registered as a singleton (IY_InObj_OutObj_SingletonAsyncService) in two task nodes.
		// These task nodes will be used as params for a third task node.
		// A random number generator will provide a process emulation time for "SomeOperationAsync" to make sure the service resource is held onto for some time by each task node.
		var n1 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nameof(Mocked.IY_InObj_OutObj_SingletonAsyncService.GetObjAsync), "arg1").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "arg1"));
		var n2 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nameof(Mocked.IY_InObj_OutObj_SingletonAsyncService.GetObjAsync), "arg2").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "arg1"));
		var n3 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjObj_OutObj_AsyncService), nameof(Mocked.IY_InObjObj_OutObj_AsyncService.GetObjAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(n1, n2);

		// Act
		await n3.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task ResultChainAccessibleThroughSharedContext()
	{
		// Arrange
		// Behavior: Ability to access previous tasks' results further down the workflow chain
		// i.e the result of n1 should be accessible by n3... 
		var cancellationToken = this.CancellationTokenFactory();
		var context = this._workflowContextFactory.Create();
		var n1 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._msgFactory.Create<string>(string.Empty, "args"));
		var n2 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.AddArg(n1);
		var n3 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStrBool_AsyncService), nameof(Mocked.IY_InStrBool_AsyncService.ProcessStrBool)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(c => c.GetMsg(n1.Id).SetParam("flag3"))
										.AddArg(n2);

		// Act
		await n3.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task WorkflowUsingOnlyTheSharedContextForParams()
	{
		// Arrange
		// Behavior: Define a workflow using only the shared context to pass params between strategies.
		// i.e no next param specified when creating a fn factory
		var cancellationToken = this.CancellationTokenFactory();
		var context = this._workflowContextFactory.Create();
		var n1 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._msgFactory.Create<string>(string.Empty, "args"))
										.IgnorePromisedResults();
		var n2 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.AddArg(c => c.GetMsg(n1.Id).SetParam("flag2"))
										.AddArg(n1)
										.IgnorePromisedResults();
		var n3 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStrBool_AsyncService), nameof(Mocked.IY_InStrBool_AsyncService.ProcessStrBool)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(
												c => c.GetMsg(n1.Id).SetParam("flag3"),
												c => c.GetMsg(n2.Id).SetParam("args3")
										)
										.AddArg(n2)
										.IgnorePromisedResults();

		// Act
		await n3.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task WorkflowWithSyncStrategyResultFeedingIntoAsyncFnWithoutWorkflowContext()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var n1 = this._nodeFactory.Create()
										.SetFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory).SetNextParamName("flag2").AsSync())
										.AddArg(this._msgFactory.Create<int>(300, "sleep"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		var n2 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync)).AsAsync())
										.AddArg(n1)
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		// Act
		await n2.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithSyncStrategyResultFeedingIntoAsyncFnUsingWorkflowContext()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var context = this._workflowContextFactory.Create();
		var n1 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
										.AddArg(this._msgFactory.Create<int>(300, "sleep"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());
		var n2 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync)).AsAsync())
										.AddArg(n1)
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.AddArg(c => c.GetMsg(n1.Id).SetParam("flag2"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.IgnorePromisedResults();

		// Act
		await n2.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithAsyncStrategyResultFeedingIntoSyncFnWithoutWorkflowContext()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var n1 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutInt_AsyncService), nameof(Mocked.IY_InStr_OutInt_AsyncService.GetIntAsync), "sleep").AsAsync())
										.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());
		var n2 = this._nodeFactory.Create()
										.AddArg(n1)
										.SetFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		// Act
		await n2.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithAsyncStrategyResultFeedingIntoSyncFnUsingWorkflowContext()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var context = this._workflowContextFactory.Create();
		var n1 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutInt_AsyncService), nameof(Mocked.IY_InStr_OutInt_AsyncService.GetIntAsync)).AsAsync())
										.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());
		var n2 = this._nodeFactory.Create()
										.SetContext(context)
										.AddArg(n1)
										.SetFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
										.AddArg(c => c.GetMsg(n1.Id).SetParam("sleep"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		// Act
		await n2.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithBaseAsyncStrategyResultFeedingIntoSyncBaseFn()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var n1 = this._nodeFactory.Create()
										.SetFn(new Mocked.TestStrategy2(new Mocked.Y_InStr_OutInt_AsyncService(), this._msgFactory).SetNextParamName("sleep").AsAsync())
										.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());
		var n2 = this._nodeFactory.Create()
										.AddArg(n1)
										.SetFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		// Act
		await n2.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithBaseAsyncStrategyResultFeedingIntoSyncBaseFnUsingWorkflowContext()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var context = this._workflowContextFactory.Create();
		var n1 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(new Mocked.TestStrategy2(new Mocked.Y_InStr_OutInt_AsyncService(), this._msgFactory))
										.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());
		var n2 = this._nodeFactory.Create()
										.SetContext(context)
										.AddArg(n1)
										.SetFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
										.AddArg(c => c.GetMsg(n1.Id).SetParam("sleep"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		// Act
		await n2.Run(cancellationToken);

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task NodesProvidedRawDataConstructMsgsAndUseThem()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();

		// Service types are irrelevant in this scenario... so let's just use the singleton...
		var n1 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nextParamName: "arg1").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg<object>(new object(), "arg1");
		var n2 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nextParamName: "arg2").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg<object>(new object(), "arg1");
		var n3 = this._nodeFactory.Create()
										.SetFn(this._fnFactory.Build(typeof(Mocked.IY_InObjObj_OutObj_AsyncService)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(n1, n2);

		// Act
		var msgs = await n3.Run(cancellationToken);
		var msg = msgs.First();

		// Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
		Assert.IsType<object>((msg as BaseMsg<object>)!.GetData());
	}

	 [Fact]
	 public async Task NodesProvidedTypesForConstructingFn()
	 {
		 // Arrange
		 var cancellationToken = this.CancellationTokenFactory();

		 // Service types are irrelevant in this scenario... so let's just use the singleton...
		 var n1 = this._nodeBuilderFactory.Create()
										 .AddFn<Mocked.IY_InObj_OutObj_SingletonAsyncService>(nextParamName: "arg1")
										 .SetExceptionHandler(Substitute.For<Action<Exception>>())
										 .AddArg<object>(new object(), "arg1")
										 .Build();
		 var n2 = this._nodeBuilderFactory.Create()
										 .AddFn<Mocked.IY_InObj_OutObj_SingletonAsyncService>(nextParamName: "arg2")
										 .SetExceptionHandler(Substitute.For<Action<Exception>>())
										 .AddArg<object>(new object(), "arg1")
										 .Build();
		 var n3 = this._nodeBuilderFactory.Create()
										 .AddFn<Mocked.IY_InObjObj_OutObj_AsyncService>()
										 .SetExceptionHandler(Substitute.For<Action<Exception>>())
										 .AddArg(n1, n2)
										 .Build();

		 // Act
		 var msgs = await n3.Run(cancellationToken);
		 var msg = msgs.First();

		 // Assert
		 Assert.NotNull(n1);
		 Assert.NotNull(n2);
		 Assert.NotNull(n3);
		 Assert.IsType<object>((msg as BaseMsg<object>)!.GetData());
	 }

	[Fact]
	public async Task WorkflowComposedOfAsyncAndSyncNodesThatAreBuiltFromFunctionPointers()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();

		var n1 = this._nodeFactory.Create()
										.SetFn((p) =>
										{
											var msg = p["sleep"] as BaseMsg<int>;
											var data = msg!.GetData();
											Assert.Equal(300, data);
											return this._msgFactory.Create<int>(data).SetParam("next_param_name");
										})
										.AddArg(this._msgFactory.Create<int>(300, "sleep"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		var n2 = this._nodeFactory.Create()
										.SetFn(async p =>
										{
											await Task.Delay(150);
											var msg = p["next_param_name"] as BaseMsg<int>;
											var msg2 = p["args2"] as BaseMsg<object>;
											Assert.NotNull(msg2!.GetData());
											var data = msg!.GetData();
											return this._msgFactory.Create<int>(data);
										})
										.AddArg(n1)
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>());

		// Act
		var msgs = await n2.Run(cancellationToken);
		var msg = msgs.First(); 
		var data = (msg as BaseMsg<int>)!.GetData();

		// Assert
		Assert.Equal(300, data);
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task GIVEN_AMultiNodeWorkflow_WHEN_SyncFnAdapterUsingWorkflowContext_THEN_BuildsOffContext()
	{
		// Arrange
		var cancellationToken = this.CancellationTokenFactory();
		var context = new WorkflowContext();

		var n1 =
			this._nodeBuilderFactory.Create(context)
				.AddFn<Mocked.IY_InBoolStr_OutConstInt_AsyncService, bool>(true)
				.AddArg("arg1")
				.Build();

		var root =
			this._nodeBuilderFactory.Create(context)
				.AddArg(n1)
				.AddFn(c =>
				{
					var d = c.GetMsgData<int>(n1.Id);
					return this._msgFactory.Create<int>(d == 1 ? 1 : 0);
				})
				.Build();

		// Act
		var msgs = await root.Run(cancellationToken);
		var msg = msgs.First(); 
		var data = (msg as BaseMsg<int>)!.GetData();

		// Assert
		Assert.Equal(1, data);
	}
}
