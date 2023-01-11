### [FtpCmdline](FtpCmdline.md 'FtpCmdline').[Program](Program.md 'FtpCmdline.Program')

## Program.GetClient(InvocationContext, StatusContext, StreamWriter, bool) Method

create and connect ftp client

```csharp
internal static System.Threading.Tasks.Task<FluentFTP.AsyncFtpClient> GetClient(System.CommandLine.Invocation.InvocationContext context, Spectre.Console.StatusContext context2, System.IO.StreamWriter? outputFile, bool supressStatus=false);
```
#### Parameters

<a name='FtpCmdline.Program.GetClient(System.CommandLine.Invocation.InvocationContext,Spectre.Console.StatusContext,System.IO.StreamWriter,bool).context'></a>

`context` [System.CommandLine.Invocation.InvocationContext](https://docs.microsoft.com/en-us/dotnet/api/System.CommandLine.Invocation.InvocationContext 'System.CommandLine.Invocation.InvocationContext')

command line context

<a name='FtpCmdline.Program.GetClient(System.CommandLine.Invocation.InvocationContext,Spectre.Console.StatusContext,System.IO.StreamWriter,bool).context2'></a>

`context2` [Spectre.Console.StatusContext](https://docs.microsoft.com/en-us/dotnet/api/Spectre.Console.StatusContext 'Spectre.Console.StatusContext')

ansi console status context

<a name='FtpCmdline.Program.GetClient(System.CommandLine.Invocation.InvocationContext,Spectre.Console.StatusContext,System.IO.StreamWriter,bool).outputFile'></a>

`outputFile` [System.IO.StreamWriter](https://docs.microsoft.com/en-us/dotnet/api/System.IO.StreamWriter 'System.IO.StreamWriter')

output file

<a name='FtpCmdline.Program.GetClient(System.CommandLine.Invocation.InvocationContext,Spectre.Console.StatusContext,System.IO.StreamWriter,bool).supressStatus'></a>

`supressStatus` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

#### Returns
[System.Threading.Tasks.Task&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[FluentFTP.AsyncFtpClient](https://docs.microsoft.com/en-us/dotnet/api/FluentFTP.AsyncFtpClient 'FluentFTP.AsyncFtpClient')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')