### [FtpCmdline](FtpCmdline.md 'FtpCmdline')

## Logger Class

general logger

```csharp
public class Logger : CmdlineBase.LoggerBase,
FluentFTP.IFtpLogger
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [CmdlineBase.LoggerBase](https://docs.microsoft.com/en-us/dotnet/api/CmdlineBase.LoggerBase 'CmdlineBase.LoggerBase') &#129106; Logger

Implements [FluentFTP.IFtpLogger](https://docs.microsoft.com/en-us/dotnet/api/FluentFTP.IFtpLogger 'FluentFTP.IFtpLogger')

| Constructors | |
| :--- | :--- |
| [Logger(InvocationContext, Option&lt;string&gt;, Option&lt;LogLevel&gt;, Option&lt;LogLevel&gt;, Option&lt;LogLevel&gt;)](Logger.Logger(InvocationContext,Option_string_,Option_LogLevel_,Option_LogLevel_,Option_LogLevel_).md 'FtpCmdline.Logger.Logger(System.CommandLine.Invocation.InvocationContext, System.CommandLine.Option<string>, System.CommandLine.Option<CmdlineBase.LogLevel>, System.CommandLine.Option<CmdlineBase.LogLevel>, System.CommandLine.Option<CmdlineBase.LogLevel>)') | constructor |

| Fields | |
| :--- | :--- |
| [_ftpClientLevel](Logger._ftpClientLevel.md 'FtpCmdline.Logger._ftpClientLevel') | ftp client log level |
| [_oldFtpClientLevel](Logger._oldFtpClientLevel.md 'FtpCmdline.Logger._oldFtpClientLevel') | ftp client old log level for progress functions |

| Properties | |
| :--- | :--- |
| [LogToConsole](Logger.LogToConsole.md 'FtpCmdline.Logger.LogToConsole') | log to console |

| Methods | |
| :--- | :--- |
| [DoInProgress()](Logger.DoInProgress().md 'FtpCmdline.Logger.DoInProgress()') | set in progress |
| [StopInProgress()](Logger.StopInProgress().md 'FtpCmdline.Logger.StopInProgress()') | set no in progress |
