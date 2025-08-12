using Xo.TaskTree.DependencyInjection.Extensions;

namespace Xo.TaskTree.Unit.Tests;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddTransient<ISvc_InStr_OutBool_AsyncService, Y_InStr_OutBool_AsyncService>();
		services.AddTransient<ISvc_InObjBool_OutStr_AsyncService, Y_InObjBool_OutStr_AsyncService>();
		services.AddTransient<ISvc_InObjBool_OutNullStr_AsyncService, Y_InObjBool_OutNullStr_AsyncService>();
		services.AddTransient<ISvc_InObjBool_OutBool_AsyncService, Y_InObjBool_OutBool_AsyncService>();
		services.AddTransient<ISvc_InStr_AsyncService, Y_InStr_AsyncService>();
		services.AddTransient<ISvc_InStrBool_AsyncService, Y_InStrBool_AsyncService>();
		services.AddTransient<ISvc_InStrBool_OutStr_AsyncService, Y_InStrBool_OutStr_AsyncService>();
		services.AddTransient<ISvc_AsyncService, Y_AsyncService>();
		services.AddSingleton<ISvc_InObj_OutObj_SingletonAsyncService, Y_InObj_OutObj_SingletonAsyncService>();
		services.AddTransient<ISvc_InObjObj_OutObj_AsyncService, Y_InObjObj_OutObj_AsyncService>();
		services.AddTransient<ISvc_SyncService, Y_SyncService>();
		services.AddTransient<ISvc_OutConstBool_SyncService, Y_OutConstBool_SyncService>();
		services.AddTransient<ISvc_InInt_OutBool_SyncService, Y_InInt_OutBool_SyncService>();
		services.AddTransient<ISvc_InStr_OutInt_AsyncService, Y_InStr_OutInt_AsyncService>();
		services.AddTransient<ISvc_InStr_OutConstInt_AsyncService, Y_InStr_OutConstInt_AsyncService>();
		services.AddTransient<ISvc_InInt_OutConstInt_AsyncService, Y_InInt_OutConstInt_AsyncService>();
		services.AddTransient<ISvc_InBoolStr_OutConstInt_AsyncService, Y_InBoolStr_OutConstInt_AsyncService>();
		services.AddTransient<ISvc_OutConstFalseBool_SyncService, Y_OutConstFalseBool_SyncService>();
		services.AddTransient<ISvc_InStr_OutConstStr_AsyncService, Y_InStr_OutConstStr_AsyncService>();
		services.AddTransient<ISvc_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService, Y_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>();
		services.AddTransient<ISvc_InBool_OutConstStr_AsyncService, Y_InBool_OutConstStr_AsyncService>();
		services.AddTransient<ISvc_InStrStrStr_OutConstInt_AsyncService, Y_InStrStrStr_OutConstInt_AsyncService>();
		services.AddTransient<ISvc_OutObj_SyncService, Y_OutObj_SyncService>();
		services.AddTransient<ISvc_InObj_OutConstInt_AsyncService, Y_InObj_OutConstInt_AsyncService>();

		services.AddTaskTreeServices();
	}
}
