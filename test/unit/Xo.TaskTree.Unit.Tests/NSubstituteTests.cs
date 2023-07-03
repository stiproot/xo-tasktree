namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class NSubsistuteTests
{
	private readonly IFnFactory _fnFactory;
	private readonly IWorkflowContextFactory _workflowContextFactory;
	private readonly INodeFactory _nodeFactory;
	private readonly IMsgFactory _msgFactory;
	private CancellationToken CancellationTokenFactory() => new CancellationToken();

	public NSubsistuteTests(
		IFnFactory fnFactory,
		IWorkflowContextFactory workflowContextFactory,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory
	)
	{
		this._fnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._workflowContextFactory = workflowContextFactory ?? throw new ArgumentNullException(nameof(workflowContextFactory));
		this._nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	[Fact]
	public async Task WorkflowWithServicesMockedWithNSubstitute()
	{
		// Arrange
		// Behavior: Workflows, with fn factories constructed with the FnFactory, with services mocked using NSubstitute should run.
		var services = new ServiceCollection();
		var testService1 = Substitute.For<IY_InStr_OutBool_AsyncService>();
		testService1.GetBoolAsync(Arg.Any<string>()).Returns(true);
		services.AddTransient<IY_InStr_OutBool_AsyncService>((provider) => testService1);
		var testService2 = Substitute.For<IY_InObjBool_OutStr_AsyncService>();
		testService2.GetStrAsync(Arg.Any<object>(), Arg.Any<bool>()).Returns("");
		services.AddTransient<IY_InObjBool_OutStr_AsyncService>((provider) => testService2);
		var provider = services.BuildServiceProvider();
		var fnFactory = new FnFactory(provider);
		var cancellationToken = this.CancellationTokenFactory();
		//var context = this._workflowContextFactory.Create();
		var context = _workflowContextFactory.Create();

		var n1 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(IY_InStr_OutBool_AsyncService), nameof(Mocked.IY_InStr_OutBool_AsyncService.GetBoolAsync), "flag2").AsAsync())
										.SetExceptionHandler(Substitute.For<Action<Exception>>())
										.AddArg(this._msgFactory.Create<string>(string.Empty, "args"));
		var n2 = this._nodeFactory.Create()
										.SetContext(context)
										.SetFn(this._fnFactory.Build(typeof(IY_InObjBool_OutStr_AsyncService), nameof(Mocked.IY_InObjBool_OutStr_AsyncService.GetStrAsync), null).AsAsync())
										.SetExceptionHandler(Substitute.For<Func<Exception, Task>>())
										.AddArg(this._msgFactory.Create<object>(new object(), "args2"))
										.AddArg(n1);

		// Act / Assert
		Assert.NotNull(n1);
		Assert.NotNull(n2);
		await n2.Run(cancellationToken);
	}
}
