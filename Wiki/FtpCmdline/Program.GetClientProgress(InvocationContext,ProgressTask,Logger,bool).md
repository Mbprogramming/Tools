### [FtpCmdline](FtpCmdline.md 'FtpCmdline').[Program](Program.md 'FtpCmdline.Program')

## Program.GetClientProgress(InvocationContext, ProgressTask, Logger, bool) Method

create and connect ftp client with progress context

```csharp
internal static System.Threading.Tasks.Task<FluentFTP.AsyncFtpClient> GetClientProgress(System.CommandLine.Invocation.InvocationContext context, Spectre.Console.ProgressTask task, FtpCmdline.Logger logger, bool supressStatus=false);
```
#### Parameters

<a name='FtpCmdline.Program.GetClientProgress(System.CommandLine.Invocation.InvocationContext,Spectre.Console.ProgressTask,FtpCmdline.Logger,bool).context'></a>

`context` [System.CommandLine.Invocation.InvocationContext](https://docs.microsoft.com/en-us/dotnet/api/System.CommandLine.Invocation.InvocationContext 'System.CommandLine.Invocation.InvocationContext')

command line context

<a name='FtpCmdline.Program.GetClientProgress(System.CommandLine.Invocation.InvocationContext,Spectre.Console.ProgressTask,FtpCmdline.Logger,bool).task'></a>

`task` [Spectre.Console.ProgressTask](https://docs.microsoft.com/en-us/dotnet/api/Spectre.Console.ProgressTask 'Spectre.Console.ProgressTask')

progress task

<a name='FtpCmdline.Program.GetClientProgress(System.CommandLine.Invocation.InvocationContext,Spectre.Console.ProgressTask,FtpCmdline.Logger,bool).logger'></a>

`logger` [Logger](Logger.md 'FtpCmdline.Logger')

output file

<a name='FtpCmdline.Program.GetClientProgress(System.CommandLine.Invocation.InvocationContext,Spectre.Console.ProgressTask,FtpCmdline.Logger,bool).supressStatus'></a>

`supressStatus` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

supress status messages

#### Returns
[System.Threading.Tasks.Task&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[FluentFTP.AsyncFtpClient](https://docs.microsoft.com/en-us/dotnet/api/FluentFTP.AsyncFtpClient 'FluentFTP.AsyncFtpClient')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')