namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class CoreFrameworkTests
{
	private readonly IFnFactory _fnFactory;
	private readonly INodeBuilderFactory _nodeBuilderFactory;
	private readonly IMsgFactory _msgFactory;
	private readonly IWorkflowContextFactory _workflowContextFactory;
	private CancellationToken CancellationTokenFactory() => new CancellationToken();

	public CoreFrameworkTests(
		IFnFactory fnFactory,
		INodeBuilderFactory nodeBuilderFactory,
		IMsgFactory msgFactory,
		IWorkflowContextFactory workflowContextFactory
	)
	{
		this._fnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
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
		// ARRANGE...
		var guid = GuidGenerator.NewGuidAsString();

		// ACT...
		var th = this._nodeBuilderFactory.Create().Configure(c => c.SetId(guid)).Build();

		// ASSERT...
		Assert.Equal(guid, th.NodeConfiguration.Id);
	}

	[Fact]
	public async Task FrameworkTest()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n2, "flag2" of type bool.
		var n1 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<string>("some string", "args")))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.Build();

		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n3, "args3" of type string.
		// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's fn factory.
		var n2 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<object>(new object(), "args2")).AddArg(n1))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.Build();

		var n3 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(n2))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		// ACT...
		await n3.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task UserDefinedStrategyFrameworkTest()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n2, "flag2" of type bool.
		// We will provide a "user defined fn", IY_InStr_OutBool_AsyncService_Fn.
		// Note that factories should be implemented by the consumer to provide their strategies, if they have gone this route.
		var n1 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<string>("some string", "args")))
										.AddFn(new Mocked.IY_InStr_OutBool_AsyncService_Fn(new Mocked.Y_InStr_OutBool_AsyncService(), this._msgFactory).SetNextParamName("flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.Build();

		// The output of this Node will be used as a param for the Task produced by the fn contained in Node n3, "args3" of type string.
		// An IMsg will be added which contains the value for "args2" param that is required by IY_InObjBool_OutStr_AsyncService, housed in th's fn factory.
		var n2 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<object>(new object(), "args2")).AddArg(n1))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.Build();

		var n3 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(n2))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		// ACT...
		await n3.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task ServiceThatAcceptsNoArgumentsFrameworkTest()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		var n3 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create(string.Empty, "args3")))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_AsyncService), nameof(Mocked.IY_InStr_AsyncService.ProcessStrAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		// This is the focus of this test. A Node that wraps an async service that takes no arguments.
		var n4 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(n3))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_AsyncService), nameof(Mocked.IY_AsyncService.ProcessAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		// ACT...
		await n4.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n3);
		Assert.NotNull(n4);
	}

	[Fact]
	public async Task SingletonServiceUsedByTwoParamHandlesTest()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		// The point of this test will be to use a service that is registered as a singleton (IY_InObj_OutObj_SingletonAsyncService) in two task nodes.
		// These task nodes will be used as params for a third task node.
		// A random number generator will provide a process emulation time for "SomeOperationAsync" to make sure the service resource is held onto for some time by each task node.
		var n1 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<object>(new object(), "arg1")))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nameof(Mocked.IY_InObj_OutObj_SingletonAsyncService.GetObjAsync), "arg1").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		var n2 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<object>(new object(), "arg1")))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nameof(Mocked.IY_InObj_OutObj_SingletonAsyncService.GetObjAsync), "arg2").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		var n3 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(n1).AddArg(n2))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjObj_OutObj_AsyncService), nameof(Mocked.IY_InObjObj_OutObj_AsyncService.GetObjAsync)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		// ACT...
		await n3.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task ResultChainAccessibleThroughSharedContext()
	{
		// ARRANGE...
		// Behavior: Ability to access previous tasks' results further down the workflow chain
		// i.e the result of n1 should be accessible by n3... 
		var cancellationToken = this.CancellationTokenFactory();
		var workflowContext = this._workflowContextFactory.Create();
		var n1 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<string>(string.Empty, "args")).AddContext(workflowContext))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		var n2 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(this._msgFactory.Create<object>(new object(), "args2")).AddArg(n1).AddContext(workflowContext))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), "args3").AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.Build();

		var n3 = this._nodeBuilderFactory.Create()
										.Configure(c => c.AddArg(n2).AddArg(c => c.GetMsg(n1.NodeConfiguration.Id).SetParam("flag3")).AddContext(workflowContext))
										.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStrBool_AsyncService), nameof(Mocked.IY_InStrBool_AsyncService.ProcessStrBool)).AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.Build();

		// ACT...
		await n3.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task WorkflowUsingOnlyTheSharedContextForParams()
	{
		// ARRANGE...
		// Behavior: Define a workflow using only the shared workflowContext to pass params between strategies.
		// i.e no next param specified when creating a fn factory
		var cancellationToken = this.CancellationTokenFactory();
		var workflowContext = this._workflowContextFactory.Create();
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(this._msgFactory.Create<string>(string.Empty, "args")).AddContext(workflowContext).IgnorePromisedResults())
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync)).AsAsync())
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
		.Configure(c =>
			c
				.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
				.AddArg(c => c.GetMsg(n1.NodeConfiguration.Id).SetParam("flag2"))
				.AddArg(n1)
				.AddContext(workflowContext)
				.IgnorePromisedResults()
		)
		.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync)).AsAsync())
		.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
		.Build();

		var n3 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddContext(workflowContext)
					.AddArg(c => c.GetMsg(n1.NodeConfiguration.Id).SetParam("flag3"))
					.AddArg(c => c.GetMsg(n2.NodeConfiguration.Id).SetParam("args3"))
					.AddArg(n2)
					.IgnorePromisedResults()
			)
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStrBool_AsyncService), nameof(Mocked.IY_InStrBool_AsyncService.ProcessStrBool)).AsAsync())
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		// ACT...
		await n3.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
	}

	[Fact]
	public async Task WorkflowWithSyncStrategyResultFeedingIntoAsyncFnWithoutWorkflowContext()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(this._msgFactory.Create<int>(300, "sleep")))
			.AddFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory).SetNextParamName("flag2").AsSync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(this._msgFactory.Create<object>(new object(), "args2")).AddArg(n1))
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync)).AsAsync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		await n2.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithSyncStrategyResultFeedingIntoAsyncFnUsingWorkflowContext()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		var workflowContext = this._workflowContextFactory.Create();
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddContext(workflowContext)
					.AddArg(this._msgFactory.Create<int>(300, "sleep"))
			)
			.AddFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddContext(workflowContext)
					.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
					.AddArg(c => c.GetMsg(n1.NodeConfiguration.Id).SetParam("flag2"))
					.AddArg(n1)
					.IgnorePromisedResults()
			)
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync)).AsAsync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		await n2.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithAsyncStrategyResultFeedingIntoSyncFnWithoutWorkflowContext()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(this._msgFactory.Create<string>("some string parameter", "args")))
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutInt_AsyncService), nameof(Mocked.IY_InStr_OutInt_AsyncService.GetIntAsync), "sleep").AsAsync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(n1))
			.AddFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		await n2.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithAsyncStrategyResultFeedingIntoSyncFnUsingWorkflowContext()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		var workflowContext = this._workflowContextFactory.Create();

		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
					.AddContext(workflowContext)
			)
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InStr_OutInt_AsyncService), nameof(Mocked.IY_InStr_OutInt_AsyncService.GetIntAsync)).AsAsync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddArg(n1)
					.AddArg(c => c.GetMsg(n1.NodeConfiguration.Id).SetParam("sleep"))
					.AddContext(workflowContext)
			)
			.AddFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		await n2.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithBaseAsyncStrategyResultFeedingIntoSyncBaseFn()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();

		var n1 = this._nodeBuilderFactory.Create()
			.Configure(
					c => c.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
			)
			.AddFn(new Mocked.TestStrategy2(new Mocked.Y_InStr_OutInt_AsyncService(), this._msgFactory).SetNextParamName("sleep").AsAsync())
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(n1))
			.AddFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		await n2.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task WorkflowWithBaseAsyncStrategyResultFeedingIntoSyncBaseFnUsingWorkflowContext()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();
		var workflowContext = this._workflowContextFactory.Create();
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddArg(this._msgFactory.Create<string>("some string parameter", "args"))
					.AddContext(workflowContext)
			)
			.AddFn(new Mocked.TestStrategy2(new Mocked.Y_InStr_OutInt_AsyncService(), this._msgFactory))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddContext(workflowContext)
					.AddArg(n1)
					.AddArg(c => c.GetMsg(n1.NodeConfiguration.Id).SetParam("sleep"))
			)
			.AddFn(new Mocked.TestSyncFn(new Mocked.Y_InInt_OutBool_SyncService(), this._msgFactory))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		await n2.Run(cancellationToken);

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	[Fact]
	public async Task NodesProvidedRawDataConstructMsgsAndUseThem()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();

		// Service types are irrelevant in this scenario... so let's just use the singleton...
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c.AddArg<object>(new object(), "arg1")
			)
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nextParamName: "arg1").AsAsync())
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c.AddArg<object>(new object(), "arg1")
			)
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObj_OutObj_SingletonAsyncService), nextParamName: "arg2").AsAsync())
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		var n3 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddArg(n1)
					.AddArg(n2)
			)
			.AddFn(this._fnFactory.Build(typeof(Mocked.IY_InObjObj_OutObj_AsyncService)).AsAsync())
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		// ACT...
		var msgs = await n3.Run(cancellationToken);
		var msg = msgs.First();

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
		Assert.IsType<object>((msg as BaseMsg<object>)!.GetData());
	}

	[Fact]
	public async Task NodesProvidedTypesForConstructingFn()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();

		// Service types are irrelevant in this scenario... so let's just use the singleton...
		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c.AddArg<object>(new object(), "arg1")
			)
			.AddFn<Mocked.IY_InObj_OutObj_SingletonAsyncService>(nextParamName: "arg1")
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c.AddArg<object>(new object(), "arg1")
			)
			.AddFn<Mocked.IY_InObj_OutObj_SingletonAsyncService>(nextParamName: "arg2")
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		var n3 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c.AddArg(n1, n2)
			)
			.AddFn<Mocked.IY_InObjObj_OutObj_AsyncService>()
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		// ACT...
		var msgs = await n3.Run(cancellationToken);
		var msg = msgs.First();

		// ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		Assert.NotNull(n3);
		Assert.IsType<object>((msg as BaseMsg<object>)!.GetData());
	}

	[Fact]
	public async Task WorkflowComposedOfAsyncAndSyncNodesThatAreBuiltFromFunctionPointers()
	{
		// ARRANGE...
		var cancellationToken = this.CancellationTokenFactory();

		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c => c.AddArg(this._msgFactory.Create<int>(300, "sleep")))
			.AddFn((p) =>
			{
				var msg = p["sleep"] as BaseMsg<int>;
				var data = msg!.GetData();
				Assert.Equal(300, data);
				return this._msgFactory.Create<int>(data).SetParam("next_param_name");
			})
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c =>
				c
					.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
					.AddArg(n1)
			)
			.AddFn(async p =>
			{
				await Task.Delay(150);
				var msg = p["next_param_name"] as BaseMsg<int>;
				var msg2 = p["args2"] as BaseMsg<object>;
				Assert.NotNull(msg2!.GetData());
				var data = msg!.GetData();
				return this._msgFactory.Create<int>(data);
			})
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT...
		var msgs = await n2.Run(cancellationToken);
		var msg = msgs.First();
		var data = (msg as BaseMsg<int>)!.GetData();

		// ASSERT...
		Assert.Equal(300, data);
		Assert.NotNull(n1);
		Assert.NotNull(n2);
	}

	//[Fact]
	//public async Task GIVEN_AMultiNodeWorkflow_WHEN_SyncFnAdapterUsingWorkflowContext_THEN_BuildsOffContext()
	//{
		//// ARRANGE...
		//var cancellationToken = this.CancellationTokenFactory();
		//var workflowContext = new WorkflowContext();

		//var n1 =
			//this._nodeBuilderFactory.Create(workflowContext)
				//.Configure(c => c.AddArg("arg1").AddContext(workflowContext))
				//.AddFn<Mocked.IY_InBoolStr_OutConstInt_AsyncService, bool>(true)
				//.Build();

		//var root =
			//this._nodeBuilderFactory.Create(workflowContext)
				//.Configure(c => c.AddArg(n1).AddContext(workflowContext))
				//.AddFn(c =>
				//{
					//var d = c.GetMsgData<int>(n1.NodeConfiguration.Id);
					//return this._msgFactory.Create<int>(d == 1 ? 1 : 0);
				//})
				//.Build();

		//// ACT...
		//var msgs = await root.Run(cancellationToken);
		//var msg = msgs.First();
		//var data = (msg as BaseMsg<int>)!.GetData();

		//// ASSERT...
		//Assert.Equal(1, data);
	//}
}
