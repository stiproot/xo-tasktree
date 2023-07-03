namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class FnFactoryTests
{
	private readonly IY_InStr_OutBool_AsyncService _testService1;
	private readonly IY_InStrBool_AsyncService _testService3WithTwoArgs;
	private readonly IY_OutConstBool_SyncService _serviceThatReturnsBool;
	private readonly IMsgFactory _msgFactory;

	public FnFactoryTests(
		IY_InStr_OutBool_AsyncService testService1,
		IY_InStrBool_AsyncService testService3WithTwoArgs,
		IY_OutConstBool_SyncService serviceThatReturnsBool,
		IMsgFactory msgFactory
	)
	{
		this._testService1 = testService1 ?? throw new System.ArgumentNullException(nameof(testService1));
		this._testService3WithTwoArgs = testService3WithTwoArgs ?? throw new System.ArgumentNullException(nameof(testService3WithTwoArgs));
		this._serviceThatReturnsBool = serviceThatReturnsBool ?? throw new System.ArgumentNullException(nameof(serviceThatReturnsBool));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	[Fact]
	public void FnBuilder_Constructor_ProvidedNullServiceProvider_ThrowsArgumentNullException()
	{
		// Act / Assert
		Assert.Throws<ArgumentNullException>(() => new FnFactory(null!));
	}

	[Fact]
	public async Task FnBuilder_ProvidedTypeAndSyncMethodName_ReturnsFactory()
	{
		// Arrange
		IArgs @params = new Args(new List<IMsg>()); 
		var type = this._serviceThatReturnsBool.GetType();
		var methodName = nameof(this._serviceThatReturnsBool.GetBool);
		var serviceProvider = Substitute.For<IServiceProvider>();
		serviceProvider.GetService(type).Returns(x => new Y_OutConstBool_SyncService());
		var builder = new FnFactory(serviceProvider);

		// Act
		var functory = builder.Build(type, methodName).AsAsync();
		var result = await functory.Invoke(@params);
		var data = (result as Msg<bool>)!.GetData();

		// Assert
		Assert.IsType<bool>(data);
		Assert.True(data);
	}

	[Fact]
	public async Task FnBuilder_ProvidedTypeAndAsyncMethodName_ReturnsFactory()
	{
		// Arrange
		// var @params = new Dictionary<string, IMsg> { { "args", this._msgFactory.Create<string>("some-string", "args") } };
		IArgs @params = new Args(new List<IMsg> { new Msg<string>("some-string", "args") });
		var type = this._testService1.GetType();
		var methodName = nameof(this._testService1.GetBoolAsync);
		var serviceProvider = Substitute.For<IServiceProvider>();
		serviceProvider.GetService(type).Returns(x => new Y_InStr_OutBool_AsyncService());
		var builder = new FnFactory(serviceProvider);

		// Act
		var functory = builder.Build(type, methodName, nextParamName: null).AsAsync();
		var result = await functory.Invoke(@params);
		var data = (result as Msg<bool>)!.GetData();

		// Assert
		Assert.IsType<bool>(data);
		Assert.True(data);
	}

	[Fact]
	public async Task FnBuilder_ProvidedIncorrectNumberOfParams_ThrowsArgumentException()
	{
		// Arrange
		// we will leave out "flag3" param of type bool that ITestServie3WithTwoArgs expects.
		// var @params = new Dictionary<string, IMsg> { { "args3", this._msgFactory.Create<string>("some-string", "args3") } };
		IArgs @params = new Args(new List<IMsg> { new Msg<string>("some-string", "args3") });
		var type = this._testService3WithTwoArgs.GetType();
		var methodName = nameof(this._testService3WithTwoArgs.ProcessStrBool);
		var serviceProvider = Substitute.For<IServiceProvider>();
		serviceProvider.GetService(type).Returns(x => new Y_InStrBool_AsyncService());
		var builder = new FnFactory(serviceProvider);

		// Act
		var functory = builder.Build(type, methodName, nextParamName: null).AsAsync();

		// Assert
		var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await functory.Invoke(@params));
		Assert.Equal($"Invalid parameters for method {nameof(this._testService3WithTwoArgs.ProcessStrBool)}. Arguments provided: args3, Parameters expected: args3,flag3", exception.Message);
	}

	[Fact]
	public async Task FnBuilder_ProvidedTypeOnly_BuildsFnUsingFirstMethodName()
	{
		// Arrange
		// var @params = new Dictionary<string, IMsg> { { "args", this._msgFactory.Create<string>("some-string", "args") } };
		IArgs @params = new Args(new List<IMsg> { new Msg<string>("some-string", "args") });
		var type = this._testService1.GetType();
		var serviceProvider = Substitute.For<IServiceProvider>();
		serviceProvider.GetService(type).Returns(x => new Y_InStr_OutBool_AsyncService());
		var builder = new FnFactory(serviceProvider);

		// Act
		var functory = builder.Build(type).AsAsync();
		var result = await functory.Invoke(@params);
		var data = (result as Msg<bool>)!.GetData();

		// Assert
		Assert.IsType<bool>(data);
		Assert.True(data);
	}

	[Fact]
	public async Task FnBuilder_ProvidedGenericType_BuildsFn()
	{
		// Arrange
		// var @params = new Dictionary<string, IMsg> { { "args", this._msgFactory.Create<string>("some-string", "args") } };
		IArgs @params = new Args(new List<IMsg> { new Msg<string>("some-string", "args") });
		var serviceProvider = Substitute.For<IServiceProvider>();
		serviceProvider.GetService(typeof(IY_InStr_OutBool_AsyncService)).Returns(x => new Y_InStr_OutBool_AsyncService());
		var builder = new FnFactory(serviceProvider);

		// Act
		var functory = builder.Build<IY_InStr_OutBool_AsyncService>().AsAsync();
		var result = await functory.Invoke(@params);
		var data = (result as Msg<bool>)!.GetData();

		// Assert
		Assert.IsType<bool>(data);
		Assert.True(data);
	}
}