### [FtpCmdline](FtpCmdline.md 'FtpCmdline')

## Program Class

Main program implementation class and entry point

```csharp
internal class Program
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Program

### Remarks
complete implementation <br/>  
See the following package documentations <br/>[Spectre.Console](https://spectreconsole.net 'https://spectreconsole.net')<br/>[System.CommandLine](https://learn.microsoft.com/en-us/dotnet/standard/commandline/ 'https://learn.microsoft.com/en-us/dotnet/standard/commandline/')

| Fields | |
| :--- | :--- |
| [exclude](Program.exclude.md 'FtpCmdline.Program.exclude') | exclude items in this list |
| [ftpClientLevel](Program.ftpClientLevel.md 'FtpCmdline.Program.ftpClientLevel') | ftp client log level |
| [host](Program.host.md 'FtpCmdline.Program.host') | ftp host option |
| [localPath](Program.localPath.md 'FtpCmdline.Program.localPath') | local path option |
| [newPath](Program.newPath.md 'FtpCmdline.Program.newPath') | ftp new path option |
| [output](Program.output.md 'FtpCmdline.Program.output') | result output file |
| [outputLevel](Program.outputLevel.md 'FtpCmdline.Program.outputLevel') | output file log level |
| [parallelTasks](Program.parallelTasks.md 'FtpCmdline.Program.parallelTasks') | count of parallel upload/download streams |
| [path](Program.path.md 'FtpCmdline.Program.path') | ftp path option |
| [pwd](Program.pwd.md 'FtpCmdline.Program.pwd') | ftp password  option |
| [recursive](Program.recursive.md 'FtpCmdline.Program.recursive') | Go down the directory tree recursivly |
| [screenLevel](Program.screenLevel.md 'FtpCmdline.Program.screenLevel') | screen log level |
| [skip](Program.skip.md 'FtpCmdline.Program.skip') | skip or overwrite existing files |
| [tree](Program.tree.md 'FtpCmdline.Program.tree') | list only directory tree |
| [user](Program.user.md 'FtpCmdline.Program.user') | ftp user option |

| Methods | |
| :--- | :--- |
| [Clear(InvocationContext)](Program.Clear(InvocationContext).md 'FtpCmdline.Program.Clear(System.CommandLine.Invocation.InvocationContext)') | clear items in folder |
| [Delete(InvocationContext)](Program.Delete(InvocationContext).md 'FtpCmdline.Program.Delete(System.CommandLine.Invocation.InvocationContext)') | delete file or directory on server |
| [Download(InvocationContext)](Program.Download(InvocationContext).md 'FtpCmdline.Program.Download(System.CommandLine.Invocation.InvocationContext)') | download folder or file |
| [DownloadParallel(InvocationContext)](Program.DownloadParallel(InvocationContext).md 'FtpCmdline.Program.DownloadParallel(System.CommandLine.Invocation.InvocationContext)') | download folder or file with parallel streams |
| [GetClient(InvocationContext, StatusContext, Logger, bool)](Program.GetClient(InvocationContext,StatusContext,Logger,bool).md 'FtpCmdline.Program.GetClient(System.CommandLine.Invocation.InvocationContext, Spectre.Console.StatusContext, FtpCmdline.Logger, bool)') | create and connect ftp client |
| [GetClientProgress(InvocationContext, ProgressTask, Logger, bool)](Program.GetClientProgress(InvocationContext,ProgressTask,Logger,bool).md 'FtpCmdline.Program.GetClientProgress(System.CommandLine.Invocation.InvocationContext, Spectre.Console.ProgressTask, FtpCmdline.Logger, bool)') | create and connect ftp client with progress context |
| [Info(InvocationContext)](Program.Info(InvocationContext).md 'FtpCmdline.Program.Info(System.CommandLine.Invocation.InvocationContext)') | get ftp server infos |
| [List(InvocationContext)](Program.List(InvocationContext).md 'FtpCmdline.Program.List(System.CommandLine.Invocation.InvocationContext)') | list entries from path on server |
| [Main(string[])](Program.Main(string[]).md 'FtpCmdline.Program.Main(string[])') | main entry point |
| [Rename(InvocationContext)](Program.Rename(InvocationContext).md 'FtpCmdline.Program.Rename(System.CommandLine.Invocation.InvocationContext)') | rename file or directory on server |
| [Upload(InvocationContext)](Program.Upload(InvocationContext).md 'FtpCmdline.Program.Upload(System.CommandLine.Invocation.InvocationContext)') | upload folder or file |
| [UploadParallel(InvocationContext)](Program.UploadParallel(InvocationContext).md 'FtpCmdline.Program.UploadParallel(System.CommandLine.Invocation.InvocationContext)') | upload path with multiple stream |
