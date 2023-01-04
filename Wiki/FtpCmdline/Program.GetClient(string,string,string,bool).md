### [FtpCmdline](FtpCmdline.md 'FtpCmdline').[Program](Program.md 'FtpCmdline.Program')

## Program.GetClient(string, string, string, bool) Method

create and connect ftp client

```csharp
internal static System.Threading.Tasks.Task<FluentFTP.AsyncFtpClient> GetClient(string host, string user, string pwd, bool log);
```
#### Parameters

<a name='FtpCmdline.Program.GetClient(string,string,string,bool).host'></a>

`host` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ftp host

<a name='FtpCmdline.Program.GetClient(string,string,string,bool).user'></a>

`user` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ftp user

<a name='FtpCmdline.Program.GetClient(string,string,string,bool).pwd'></a>

`pwd` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ftp user password

<a name='FtpCmdline.Program.GetClient(string,string,string,bool).log'></a>

`log` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

enable extended logging

#### Returns
[System.Threading.Tasks.Task&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')[FluentFTP.AsyncFtpClient](https://docs.microsoft.com/en-us/dotnet/api/FluentFTP.AsyncFtpClient 'FluentFTP.AsyncFtpClient')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task-1 'System.Threading.Tasks.Task`1')  
created async ftp client

#### Exceptions

[System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')