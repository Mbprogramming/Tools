### [FtpCmdline](FtpCmdline.md 'FtpCmdline')

## Program Class

Main program implementation class and entry point

```csharp
internal class Program
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Program

### Remarks
complete implementation

| Fields | |
| :--- | :--- |
| [exclude](Program.exclude.md 'FtpCmdline.Program.exclude') | exclude items in this list |
| [host](Program.host.md 'FtpCmdline.Program.host') | ftp host option |
| [localPath](Program.localPath.md 'FtpCmdline.Program.localPath') | local path option |
| [log](Program.log.md 'FtpCmdline.Program.log') | extended logging option |
| [newPath](Program.newPath.md 'FtpCmdline.Program.newPath') | ftp new path option |
| [path](Program.path.md 'FtpCmdline.Program.path') | ftp path option |
| [pwd](Program.pwd.md 'FtpCmdline.Program.pwd') | ftp password  option |
| [recursive](Program.recursive.md 'FtpCmdline.Program.recursive') | Go down the directory tree recursivly |
| [skip](Program.skip.md 'FtpCmdline.Program.skip') | skip or overwrite existing files |
| [user](Program.user.md 'FtpCmdline.Program.user') | ftp user option |

| Methods | |
| :--- | :--- |
| [Clear(InvocationContext)](Program.Clear(InvocationContext).md 'FtpCmdline.Program.Clear(System.CommandLine.Invocation.InvocationContext)') | clear items in folder |
| [Delete(InvocationContext)](Program.Delete(InvocationContext).md 'FtpCmdline.Program.Delete(System.CommandLine.Invocation.InvocationContext)') | delete file or directory on server |
| [Download(InvocationContext)](Program.Download(InvocationContext).md 'FtpCmdline.Program.Download(System.CommandLine.Invocation.InvocationContext)') | download folder or file |
| [GetClient(InvocationContext, StatusContext)](Program.GetClient(InvocationContext,StatusContext).md 'FtpCmdline.Program.GetClient(System.CommandLine.Invocation.InvocationContext, Spectre.Console.StatusContext)') | create and connect ftp client |
| [Info(InvocationContext)](Program.Info(InvocationContext).md 'FtpCmdline.Program.Info(System.CommandLine.Invocation.InvocationContext)') | get ftp server infos |
| [List(InvocationContext)](Program.List(InvocationContext).md 'FtpCmdline.Program.List(System.CommandLine.Invocation.InvocationContext)') | list entries from path on server |
| [Main(string[])](Program.Main(string[]).md 'FtpCmdline.Program.Main(string[])') | main entry point |
| [Rename(InvocationContext)](Program.Rename(InvocationContext).md 'FtpCmdline.Program.Rename(System.CommandLine.Invocation.InvocationContext)') | rename file or directory on server |
| [Upload(InvocationContext)](Program.Upload(InvocationContext).md 'FtpCmdline.Program.Upload(System.CommandLine.Invocation.InvocationContext)') | upload folder or file |
