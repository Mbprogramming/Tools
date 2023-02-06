### [FtpCmdline](FtpCmdline.md 'FtpCmdline')

## Logger Class

general logger

```csharp
public class Logger :
FluentFTP.IFtpLogger,
System.IDisposable
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Logger

Implements [FluentFTP.IFtpLogger](https://docs.microsoft.com/en-us/dotnet/api/FluentFTP.IFtpLogger 'FluentFTP.IFtpLogger'), [System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable 'System.IDisposable')

| Constructors | |
| :--- | :--- |
| [Logger(InvocationContext, Option&lt;string&gt;, Option&lt;LogLevel&gt;, Option&lt;LogLevel&gt;, Option&lt;LogLevel&gt;)](Logger.Logger(InvocationContext,Option_string_,Option_LogLevel_,Option_LogLevel_,Option_LogLevel_).md 'FtpCmdline.Logger.Logger(System.CommandLine.Invocation.InvocationContext, System.CommandLine.Option<string>, System.CommandLine.Option<FtpCmdline.LogLevel>, System.CommandLine.Option<FtpCmdline.LogLevel>, System.CommandLine.Option<FtpCmdline.LogLevel>)') | constructor |

| Properties | |
| :--- | :--- |
| [LogToConsole](Logger.LogToConsole.md 'FtpCmdline.Logger.LogToConsole') | log to console |

| Methods | |
| :--- | :--- |
| [Dispose()](Logger.Dispose().md 'FtpCmdline.Logger.Dispose()') | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| [DoInProgress()](Logger.DoInProgress().md 'FtpCmdline.Logger.DoInProgress()') | set in progress |
| [LogError(object)](Logger.LogError(object).md 'FtpCmdline.Logger.LogError(object)') | log error object |
| [LogError(string)](Logger.LogError(string).md 'FtpCmdline.Logger.LogError(string)') | Log Error |
| [LogError(Exception)](Logger.LogError(Exception).md 'FtpCmdline.Logger.LogError(System.Exception)') | log exception |
| [LogInfo(object)](Logger.LogInfo(object).md 'FtpCmdline.Logger.LogInfo(object)') | log info object |
| [LogInfo(string)](Logger.LogInfo(string).md 'FtpCmdline.Logger.LogInfo(string)') | Log Info |
| [LogVerbose(object)](Logger.LogVerbose(object).md 'FtpCmdline.Logger.LogVerbose(object)') | log verbose object |
| [LogVerbose(string)](Logger.LogVerbose(string).md 'FtpCmdline.Logger.LogVerbose(string)') | Log Verbose |
| [LogWarn(object)](Logger.LogWarn(object).md 'FtpCmdline.Logger.LogWarn(object)') | log warning object |
| [LogWarn(string)](Logger.LogWarn(string).md 'FtpCmdline.Logger.LogWarn(string)') | Log Warning |
| [StopInProgress()](Logger.StopInProgress().md 'FtpCmdline.Logger.StopInProgress()') | set no in progress |
