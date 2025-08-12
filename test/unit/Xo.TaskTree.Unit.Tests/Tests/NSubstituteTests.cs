namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class NSubsistuteTests
{
	private readonly IFnFactory _fnFactory;
	private readonly IWorkflowContextFactory _workflowContextFactory;
	private readonly INodeBuilderFactory _nodeBuilderFactory;
	private readonly IMsgFactory _msgFactory;
	private CancellationToken CancellationTokenFactory() => new CancellationToken();

	public NSubsistuteTests(
		IFnFactory fnFactory,
		IWorkflowContextFactory workflowContextFactory,
		INodeBuilderFactory nodeBuilderFactory,
		IMsgFactory msgFactory
	)
	{
		this._fnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._workflowContextFactory = workflowContextFactory ?? throw new ArgumentNullException(nameof(workflowContextFactory));
		this._nodeBuilderFactory = nodeBuilderFactory ?? throw new ArgumentNullException(nameof(nodeBuilderFactory));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	[Fact]
	public async Task WorkflowWithServicesMockedWithNSubstitute()
	{
		// ARRANGE...
		// Behavior: Workflows, with fn factories constructed with the FnFactory, with services mocked using NSubstitute should run.
		var services = new ServiceCollection();
		var testService1 = Substitute.For<ISvc_InStr_OutBool_AsyncService>();
		testService1.GetBoolAsync(Arg.Any<string>()).Returns(true);
		services.AddTransient<ISvc_InStr_OutBool_AsyncService>((provider) => testService1);
		var testService2 = Substitute.For<ISvc_InObjBool_OutStr_AsyncService>();
		testService2.GetStrAsync(Arg.Any<object>(), Arg.Any<bool>()).Returns("");
		services.AddTransient<ISvc_InObjBool_OutStr_AsyncService>((provider) => testService2);
		var provider = services.BuildServiceProvider();
		var fnFactory = new FnFactory(provider);
		var cancellationToken = this.CancellationTokenFactory();
		//var workflowContext = this._workflowContextFactory.Create();
		var workflowContext = _workflowContextFactory.Create();

		var n1 = this._nodeBuilderFactory.Create()
			.Configure(c => 
				c
					.AddContext(workflowContext)
					.AddArg(this._msgFactory.Create<string>(string.Empty, "args"))
			)
			.AddFn(this._fnFactory.Build(typeof(ISvc_InStr_OutBool_AsyncService), nameof(Mocked.ISvc_InStr_OutBool_AsyncService.GetBoolAsync), "flag2"))
			.SetExceptionHandler(Substitute.For<Action<Exception>>())
			.Build();

		var n2 = this._nodeBuilderFactory.Create()
			.Configure(c => 
				c
					.AddContext(workflowContext)
					.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
					.AddArg(n1)
			)
			.AddFn(this._fnFactory.Build(typeof(ISvc_InObjBool_OutStr_AsyncService), nameof(Mocked.ISvc_InObjBool_OutStr_AsyncService.GetStrAsync), null))
			.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
			.Build();

		// ACT / ASSERT...
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		await n2.Resolve(cancellationToken);
	}
}
