namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class TypeInspectorTests
{
	private readonly IY_InStr_OutBool_AsyncService _testService1;
	private readonly IY_InStr_AsyncService _testService3;
	private readonly IY_SyncService _syncTestService;
	private readonly IY_OutConstBool_SyncService _returnsBoolService;

	public TypeInspectorTests(
		IY_InStr_OutBool_AsyncService testService1,
		IY_InStr_AsyncService testService3,
		IY_SyncService syncTestService,
		IY_OutConstBool_SyncService returnsBoolService
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
		// Arrange
		var type = typeof(IY_InStr_OutBool_AsyncService);
		var methodName = nameof(IY_InStr_OutBool_AsyncService.GetBoolAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedInterfaceMethodThatReturnsTask_ReturnTrue()
	{
		// Arrange
		var type = typeof(IY_InStr_AsyncService);
		var methodName = nameof(IY_InStr_AsyncService.ProcessStrAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedClassMethodThatReturnsTaskOfBool_ReturnTrue()
	{
		// Arrange
		var type = this._testService1.GetType();
		var methodName = nameof(this._testService1.GetBoolAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedClassMethodThatReturnsTask_ReturnTrue()
	{
		// Arrange
		var type = this._testService3.GetType();
		var methodName = nameof(this._testService3.ProcessStrAsync);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.True(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncInterfaceVoidMethod_ReturnFalse()
	{
		// Arrange
		var type = typeof(IY_SyncService);
		var methodName = nameof(IY_SyncService.Process);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.False(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncInterfaceMethodThatReturnsBool_ReturnFalse()
	{
		// Arrange
		var type = typeof(IY_OutConstBool_SyncService);
		var methodName = nameof(IY_OutConstBool_SyncService.GetBool);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.False(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncClassVoidMethod_ReturnFalse()
	{
		// Arrange
		var type = this._syncTestService.GetType();
		var methodName = nameof(this._syncTestService.Process);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.False(MethodHasReturnTypeOfTask);
	}

	[Fact]
	public void TypeInspector_MethodHasReturnTypeOfTask_ProvidedSyncClassMethodThatReturnsBool_ReturnFalse()
	{
		// Arrange
		var type = this._returnsBoolService.GetType();
		var methodName = nameof(this._returnsBoolService.GetBool);
		MethodInfo methodInfo = type.GetMethod(methodName)!;

		// Act
		var MethodHasReturnTypeOfTask = TypeInspector.MethodHasReturnTypeOfTask(methodInfo);

		// Assert
		Assert.False(MethodHasReturnTypeOfTask);
	}
}
