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
| [Logger(InvocationContext, Option&lt;string&gt;, Option&lt;LogLevel&gt;, Option&lt;bool&gt;)](Logger.Logger(InvocationContext,Option_string_,Option_LogLevel_,Option_bool_).md 'FtpCmdline.Logger.Logger(System.CommandLine.Invocation.InvocationContext, System.CommandLine.Option<string>, System.CommandLine.Option<FtpCmdline.LogLevel>, System.CommandLine.Option<bool>)') | constructor |

| Properties | |
| :--- | :--- |
| [LogToConsole](Logger.LogToConsole.md 'FtpCmdline.Logger.LogToConsole') | log to console |

| Methods | |
| :--- | :--- |
| [Dispose()](Logger.Dispose().md 'FtpCmdline.Logger.Dispose()') | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| [LogError(string, bool)](Logger.LogError(string,bool).md 'FtpCmdline.Logger.LogError(string, bool)') | Log Error |
| [LogInfo(string, bool)](Logger.LogInfo(string,bool).md 'FtpCmdline.Logger.LogInfo(string, bool)') | Log Info |
| [LogVerbose(string, bool)](Logger.LogVerbose(string,bool).md 'FtpCmdline.Logger.LogVerbose(string, bool)') | Log Verbose |
| [LogWarn(string, bool)](Logger.LogWarn(string,bool).md 'FtpCmdline.Logger.LogWarn(string, bool)') | Log Warning |
