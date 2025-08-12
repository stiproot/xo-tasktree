namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class TypeInspectorTests
{
	private readonly ISvc_InStr_OutBool_AsyncService _testService1;
	private readonly ISvc_InStr_AsyncService _testService3;
	private readonly ISvc_SyncService _syncTestService;
	private readonly ISvc_OutConstBool_SyncService _returnsBoolService;

	public TypeInspectorTests(
		ISvc_InStr_OutBool_AsyncService testService1,
		ISvc_InStr_AsyncService testService3,
		ISvc_SyncService syncTestService,
		ISvc_OutConstBool_SyncService returnsBoolService
	)
	{
		this._testService1 = testService1 ?? throw new System.ArgumentNullException(nameof(testService1));
		this._testService3 = testService3 ?? throw new System.ArgumentNullException(nameof(testService3));
		this._syncTestService = syncTestService ?? throw new System.ArgumentNullException(nameof(syncTestService));
		this._returnsBoolService = returnsBoolService ?? throw new System.ArgumentNullException(nameof(returnsBoolService));
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedInterfaceMethodThatReturnsTaskOfBool_ReturnTrue()
	{
		// ARRANGE...
		var type = typeof(ISvc_InStr_OutBool_AsyncService);
		var methodName = nameof(ISvc_InStr_OutBool_AsyncService.GetBoolAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedInterfaceMethodThatReturnsTask_ReturnTrue()
	{
		// ARRANGE...
		var type = typeof(ISvc_InStr_AsyncService);
		var methodName = nameof(ISvc_InStr_AsyncService.ProcessStrAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedClassMethodThatReturnsTaskOfBool_ReturnTrue()
	{
		// ARRANGE...
		var type = this._testService1.GetType();
		var methodName = nameof(this._testService1.GetBoolAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedClassMethodThatReturnsTask_ReturnTrue()
	{
		// ARRANGE...
		var type = this._testService3.GetType();
		var methodName = nameof(this._testService3.ProcessStrAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncInterfaceVoidMethod_ReturnFalse()
	{
		// ARRANGE...
		var type = typeof(ISvc_SyncService);
		var methodName = nameof(ISvc_SyncService.Process);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.False(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncInterfaceMethodThatReturnsBool_ReturnFalse()
	{
		// ARRANGE...
		var type = typeof(ISvc_OutConstBool_SyncService);
		var methodName = nameof(ISvc_OutConstBool_SyncService.GetBool);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.False(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncClassVoidMethod_ReturnFalse()
	{
		// ARRANGE...
		var type = this._syncTestService.GetType();
		var methodName = nameof(this._syncTestService.Process);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.False(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncClassMethodThatReturnsBool_ReturnFalse()
	{
		// ARRANGE...
		var type = this._returnsBoolService.GetType();
		var methodName = nameof(this._returnsBoolService.GetBool);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// ACT...
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// ASSERT...
		Assert.False(MethodHasReturnTypeOfTask);
	}
}
