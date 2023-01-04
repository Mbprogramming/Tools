using System.CommandLine;
using FluentFTP;

namespace FtpCmdline
{
    /// <summary>
    /// Main program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// create and connect ftp client
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private static async Task<AsyncFtpClient> GetClient(string host, string user, string pwd, bool log)
        {
            try
            {
                var client = new AsyncFtpClient(host, user, pwd);
                client.Config.LogToConsole = log;
                await client.AutoConnect();
                return client;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// get ftp server infos
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        static async Task Info(string host, string user, string pwd, bool log)
        {
            try
            {
                using var client = await GetClient(host, user, pwd, log);
                Console.WriteLine(client.ServerType);
                Console.WriteLine(client.ServerOS);
                await client.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// list entries from path on server
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        static async Task List(string host, string user, string pwd, string path, bool log)
        {
            try
            {
                using var client = await GetClient(host, user, pwd, log);
                var files = await client.GetNameListing(path);
                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
                await client.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// delete file or directory on server
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        static async Task Delete(string host, string user, string pwd, string path, bool log)
        {
            try
            {
                using var client = await GetClient(host, user, pwd, log);
                if (await client.DirectoryExists(path))
                {
                    await client.DeleteDirectory(path);
                    Console.WriteLine("Directory deleted");
                }
                else if (await client.FileExists(path))
                {
                    await client.DeleteFile(path);
                    Console.WriteLine("File deleted");
                }
                else
                {
                    Console.WriteLine("File or Directory not exists");
                }
                await client.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// rename file or directory on server
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        static async Task Rename(string host, string user, string pwd, string path, string newName, bool log)
        {
            try
            {
                using var client = await GetClient(host, user, pwd, log);
                if (await client.DirectoryExists(path))
                {
                    await client.MoveDirectory(path, newName);
                    Console.WriteLine("Directory renamed");
                }
                else if (await client.FileExists(path))
                {
                    await client.MoveFile(path, newName);
                    Console.WriteLine("File renamed");
                }
                else
                {
                    Console.WriteLine("File or Directory not exists");
                }
                await client.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// upload file or directory to server
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="path"></param>
        /// <param name="localPath"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        static async Task Upload(string host, string user, string pwd, string path, string localPath, bool log)
        {
            try
            {
                using var client = await GetClient(host, user, pwd, log);
                if (Directory.Exists(localPath))
                {
                    await client.UploadDirectory(localPath, path);
                    Console.WriteLine("Directory uploaded");
                }
                else if (File.Exists(localPath))
                {
                    await client.UploadFile(localPath, path);
                    Console.WriteLine("File uploaded");
                }
                else
                {
                    Console.WriteLine("File or Directory not exists");
                }
                await client.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// main entry point
        /// </summary>
        /// <remarks>
        /// init options and commands
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task<int> Main(string[] args)
        {
            var host = new Option<string>("--host", "The FTP host");
            var user = new Option<string>("--user", "The FTP user");
            var pwd = new Option<string>("--pwd", "The FTP pwd");
            var path = new Option<string>("--path", () => "", "The path to start");
            var newPath = new Option<string>("--newPath", () => "", "The path renamed to");
            var localPath = new Option<string>("--localPath", () => "", "The local path to upload");
            var log = new Option<bool>("--log", () => false, "Show log output");

            var rootCommand = new RootCommand("FTP Helper");

            var listCommand = new Command("list", "List path content on host.")
                                            {
                                                host,
                                                user,
                                                pwd,
                                                path,
                                                log
                                            };

            var infoCommand = new Command("info", "Get Server Infos.")
                                            {
                                                host,
                                                user,
                                                pwd,
                                                log
                                            };

            var deleteCommand = new Command("delete", "Delete file or directory on host.")
                                            {
                                                host,
                                                user,
                                                pwd,
                                                path,
                                                log
                                            };

            var renameCommand = new Command("rename", "Rename file or directory on host.")
                                            {
                                                host,
                                                user,
                                                pwd,
                                                path,
                                                newPath,
                                                log
                                            };

            var uploadCommand = new Command("upload", "Upload file or directory to host.")
                                            {
                                                host,
                                                user,
                                                pwd,
                                                path,
                                                localPath,
                                                log
                                            };

            rootCommand.AddCommand(listCommand);
            rootCommand.AddCommand(infoCommand);
            rootCommand.AddCommand(deleteCommand);
            rootCommand.AddCommand(renameCommand);
            rootCommand.AddCommand(uploadCommand);

            listCommand.SetHandler(async (host, user, pwd, path, log) => await List(host, user, pwd, path, log), host, user, pwd, path, log);
            infoCommand.SetHandler(async (host, user, pwd, log) => await Info(host, user, pwd, log), host, user, pwd, log);
            deleteCommand.SetHandler(async (host, user, pwd, path, log) => await Delete(host, user, pwd, path, log), host, user, pwd, path, log);
            renameCommand.SetHandler(async (host, user, pwd, path, newPath, log) => await Rename(host, user, pwd, path, newPath, log), host, user, pwd, path, newPath, log);
            uploadCommand.SetHandler(async (host, user, pwd, path, localPath, log) => await Upload(host, user, pwd, path, localPath, log), host, user, pwd, path, localPath, log);

            return await rootCommand.InvokeAsync(args);
        }
    }
}