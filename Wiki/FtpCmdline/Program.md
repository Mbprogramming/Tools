### [FtpCmdline](FtpCmdline.md 'FtpCmdline')

## Program Class

Main program class

```csharp
internal class Program
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; Program

### Remarks
complete implementation

| Fields | |
| :--- | :--- |
| [returnCode](Program.returnCode.md 'FtpCmdline.Program.returnCode') | return code 0...success 1...error 2...file not found |

| Methods | |
| :--- | :--- |
| [Delete(string, string, string, string, bool)](Program.Delete(string,string,string,string,bool).md 'FtpCmdline.Program.Delete(string, string, string, string, bool)') | delete file or directory on server |
| [GetClient(string, string, string, bool)](Program.GetClient(string,string,string,bool).md 'FtpCmdline.Program.GetClient(string, string, string, bool)') | create and connect ftp client |
| [Info(string, string, string, bool)](Program.Info(string,string,string,bool).md 'FtpCmdline.Program.Info(string, string, string, bool)') | get ftp server infos |
| [List(string, string, string, string, bool)](Program.List(string,string,string,string,bool).md 'FtpCmdline.Program.List(string, string, string, string, bool)') | list entries from path on server |
| [Main(string[])](Program.Main(string[]).md 'FtpCmdline.Program.Main(string[])') | main entry point |
| [Rename(string, string, string, string, string, bool)](Program.Rename(string,string,string,string,string,bool).md 'FtpCmdline.Program.Rename(string, string, string, string, string, bool)') | rename file or directory on server |
| [Upload(string, string, string, string, string, bool)](Program.Upload(string,string,string,string,string,bool).md 'FtpCmdline.Program.Upload(string, string, string, string, string, bool)') | upload file or directory to server |
