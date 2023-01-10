### [FtpCmdline](FtpCmdline.md 'FtpCmdline')

## FileLogger Class

file logger class for FluentFTP client

```csharp
internal class FileLogger :
FluentFTP.IFtpLogger
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; FileLogger

Implements [FluentFTP.IFtpLogger](https://docs.microsoft.com/en-us/dotnet/api/FluentFTP.IFtpLogger 'FluentFTP.IFtpLogger')

| Constructors | |
| :--- | :--- |
| [FileLogger(StreamWriter, LogLevel)](FileLogger.FileLogger(StreamWriter,LogLevel).md 'FtpCmdline.FileLogger.FileLogger(System.IO.StreamWriter, FtpCmdline.LogLevel)') | constructor |

| Methods | |
| :--- | :--- |
| [Log(FtpLogEntry)](FileLogger.Log(FtpLogEntry).md 'FtpCmdline.FileLogger.Log(FluentFTP.FtpLogEntry)') | write log entry to file |
