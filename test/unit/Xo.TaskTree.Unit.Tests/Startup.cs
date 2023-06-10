using Xo.TaskTree.DependencyInjection.Extensions;

namespace Xo.TaskTree.Unit.Tests;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddTransient<IY_InStr_OutBool_AsyncService, Y_InStr_OutBool_AsyncService>();
		services.AddTransient<IY_InObjBool_OutStr_AsyncService, Y_InObjBool_OutStr_AsyncService>();
		services.AddTransient<IY_InObjBool_OutNullStr_AsyncService, Y_InObjBool_OutNullStr_AsyncService>();
		services.AddTransient<IY_InObjBool_OutBool_AsyncService, Y_InObjBool_OutBool_AsyncService>();
		services.AddTransient<IY_InStr_AsyncService, Y_InStr_AsyncService>();
		services.AddTransient<IY_InStrBool_AsyncService, Y_InStrBool_AsyncService>();
		services.AddTransient<IY_InStrBool_OutStr_AsyncService, Y_InStrBool_OutStr_AsyncService>();
		services.AddTransient<IY_AsyncService, Y_AsyncService>();
		services.AddSingleton<IY_InObj_OutObj_SingletonAsyncService, Y_InObj_OutObj_SingletonAsyncService>();
		services.AddTransient<IY_InObjObj_OutObj_AsyncService, Y_InObjObj_OutObj_AsyncService>();
		services.AddTransient<IY_SyncService, Y_SyncService>();
		services.AddTransient<IY_OutConstBool_SyncService, Y_OutConstBool_SyncService>();
		services.AddTransient<IY_InInt_OutBool_SyncService, Y_InInt_OutBool_SyncService>();
		services.AddTransient<IY_InStr_OutInt_AsyncService, Y_InStr_OutInt_AsyncService>();
		services.AddTransient<IY_InStr_OutConstInt_AsyncService, Y_InStr_OutConstInt_AsyncService>();
		services.AddTransient<IY_InBoolStr_OutConstInt_AsyncService, Y_InBoolStr_OutConstInt_AsyncService>();

		services.AddTaskFlowServices();
	}
}
