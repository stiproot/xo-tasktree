namespace Xo.TaskTree.Unit.Tests.Mocks;

internal interface IX_OutInt_SyncService
{
	int GetInt();
}

internal class X_OutInt_SyncService : IX_OutInt_SyncService
{
	public int GetInt() => 1;
}

internal interface IX_InInt_OutInt_SyncService
{
	int GetInt(int i);
}

internal class X_InInt_OutInt_SyncService : IX_InInt_OutInt_SyncService
{
	public int GetInt(int i) => i;
}

internal interface IX_OutStr_SyncService
{
	string GetStr();
}

internal class X_OutStr_SyncService : IX_OutStr_SyncService
{
	public string GetStr() => "One";
}

internal interface IX_InStr_OutStr_SyncService
{
	string GetStr(string s);
}

internal class X_InStr_OutStr_SyncService : IX_InStr_OutStr_SyncService
{
	public string GetStr(string s) => s;
}

internal interface IX_InStrInt_OutInt_SyncService
{
	int GetInt(string s, int i);
}

internal class X_OutInt_AsyncService : IX_InStrInt_OutInt_SyncService
{
	public int GetInt(string s, int i) => 1;
}

internal interface IX_InInt_OutInt_AsyncService
{
	Task<int> GetIntAsync(int i);
}

internal class X_InInt_OutInt_AsyncService : IX_InInt_OutInt_AsyncService
{
	public async Task<int> GetIntAsync(int i) => await Task.FromResult(i);
}

internal interface IX_OutStr_AsyncService
{
	Task<string> GetStrAsync();
}

internal class X_OutStr_AsyncService : IX_OutStr_AsyncService
{
	public async Task<string> GetStrAsync() => await Task.FromResult("One");
}

internal interface IX_InStrInt_OutInt_AsyncService
{
	Task<int> GetIntAsync(string s, int i);
}

internal class X_InStrInt_OutInt_AsyncService : IX_InStrInt_OutInt_AsyncService
{
	public async Task<int> GetIntAsync(string s, int i) => await Task.FromResult(i);
}

internal interface IXAsyncService
{
	Task GetAsync();
}

internal class XAsyncService : IXAsyncService
{
	public async Task GetAsync() => await Task.CompletedTask;
}

internal interface IX_InInt_AsyncService
{
	Task InIntAsync(int i);
}

internal class X_InInt_AsyncService : IX_InInt_AsyncService
{
	public async Task InIntAsync(int i) => await Task.CompletedTask;
}