### [FtpCmdline](FtpCmdline.md 'FtpCmdline').[Program](Program.md 'FtpCmdline.Program')

## Program.Upload(string, string, string, string, string, bool) Method

upload file or directory to server

```csharp
private static System.Threading.Tasks.Task Upload(string host, string user, string pwd, string path, string localPath, bool log);
```
#### Parameters

<a name='FtpCmdline.Program.Upload(string,string,string,string,string,bool).host'></a>

`host` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ftp host

<a name='FtpCmdline.Program.Upload(string,string,string,string,string,bool).user'></a>

`user` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ftp user

<a name='FtpCmdline.Program.Upload(string,string,string,string,string,bool).pwd'></a>

`pwd` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ftp user password

<a name='FtpCmdline.Program.Upload(string,string,string,string,string,bool).path'></a>

`path` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

server path

<a name='FtpCmdline.Program.Upload(string,string,string,string,string,bool).localPath'></a>

`localPath` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

local path

<a name='FtpCmdline.Program.Upload(string,string,string,string,string,bool).log'></a>

`log` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

enable extended logging

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')