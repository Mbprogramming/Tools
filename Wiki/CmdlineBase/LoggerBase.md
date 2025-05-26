### [CmdlineBase](CmdlineBase.md 'CmdlineBase')

## LoggerBase Class

general logger

```csharp
public class LoggerBase :
System.IDisposable
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; LoggerBase

Implements [System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/System.IDisposable 'System.IDisposable')

| Constructors | |
| :--- | :--- |
| [LoggerBase(InvocationContext, Option&lt;string&gt;, Option&lt;LogLevel&gt;, Option&lt;LogLevel&gt;)](LoggerBase.LoggerBase(InvocationContext,Option_string_,Option_LogLevel_,Option_LogLevel_).md 'CmdlineBase.LoggerBase.LoggerBase(System.CommandLine.Invocation.InvocationContext, System.CommandLine.Option<string>, System.CommandLine.Option<CmdlineBase.LogLevel>, System.CommandLine.Option<CmdlineBase.LogLevel>)') | constructor |

| Fields | |
| :--- | :--- |
| [_level](LoggerBase._level.md 'CmdlineBase.LoggerBase._level') | log level for output file |
| [_oldScreenLevel](LoggerBase._oldScreenLevel.md 'CmdlineBase.LoggerBase._oldScreenLevel') | old screen level to disable output during progress |
| [_outputFile](LoggerBase._outputFile.md 'CmdlineBase.LoggerBase._outputFile') | output file |
| [_outputLock](LoggerBase._outputLock.md 'CmdlineBase.LoggerBase._outputLock') | lock to write file from different threads |
| [_outputPath](LoggerBase._outputPath.md 'CmdlineBase.LoggerBase._outputPath') | output file path |
| [_screenLevel](LoggerBase._screenLevel.md 'CmdlineBase.LoggerBase._screenLevel') | log level for screeb |

| Methods | |
| :--- | :--- |
| [Dispose()](LoggerBase.Dispose().md 'CmdlineBase.LoggerBase.Dispose()') | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. |
| [DoInProgress()](LoggerBase.DoInProgress().md 'CmdlineBase.LoggerBase.DoInProgress()') | set in progress |
| [LogError(object)](LoggerBase.LogError(object).md 'CmdlineBase.LoggerBase.LogError(object)') | log error object |
| [LogError(string)](LoggerBase.LogError(string).md 'CmdlineBase.LoggerBase.LogError(string)') | Log Error |
| [LogError(Exception)](LoggerBase.LogError(Exception).md 'CmdlineBase.LoggerBase.LogError(System.Exception)') | log exception |
| [LogInfo(object)](LoggerBase.LogInfo(object).md 'CmdlineBase.LoggerBase.LogInfo(object)') | log info object |
| [LogInfo(string)](LoggerBase.LogInfo(string).md 'CmdlineBase.LoggerBase.LogInfo(string)') | Log Info |
| [LogVerbose(object)](LoggerBase.LogVerbose(object).md 'CmdlineBase.LoggerBase.LogVerbose(object)') | log verbose object |
| [LogVerbose(string)](LoggerBase.LogVerbose(string).md 'CmdlineBase.LoggerBase.LogVerbose(string)') | Log Verbose |
| [LogWarn(object)](LoggerBase.LogWarn(object).md 'CmdlineBase.LoggerBase.LogWarn(object)') | log warning object |
| [LogWarn(string)](LoggerBase.LogWarn(string).md 'CmdlineBase.LoggerBase.LogWarn(string)') | Log Warning |
| [StopInProgress()](LoggerBase.StopInProgress().md 'CmdlineBase.LoggerBase.StopInProgress()') | set no in progress |
