 using System.CommandLine;
using System.CommandLine.Invocation;
using FluentFTP;
using Spectre.Console;

namespace FtpCmdline
{
    /// <summary>
    /// Main program implementation class and entry point
    /// </summary>
    /// <remarks>
    /// complete implementation
    /// </remarks>
    internal class Program
    {
        /// <summary>
        /// ftp host option
        /// </summary>
        internal static Option<string>? host;
        /// <summary>
        /// ftp user option
        /// </summary>
        internal static Option<string>? user;
        /// <summary>
        /// ftp password  option
        /// </summary>
        internal static Option<string>? pwd;
        /// <summary>
        /// ftp path option
        /// </summary>
        internal static Option<string>? path;
        /// <summary>
        /// ftp new path option
        /// </summary>
        internal static Option<string>? newPath;
        /// <summary>
        /// local path option
        /// </summary>
        internal static Option<string>? localPath;
        /// <summary>
        /// extended logging option
        /// </summary>
        internal static Option<bool>? log;

        /// <summary>
        /// create and connect ftp client
        /// </summary>
        /// <param name="context">command line context</param>
        /// <param name="context2">ansi console status context</param>
        /// <returns></returns>
        internal static async Task<AsyncFtpClient> GetClient(InvocationContext context, StatusContext context2)
        {
            try
            {
                context2.Status = "Connecting...";

                var hostValue = host != null ? context.ParseResult.GetValueForOption(host) : string.Empty;
                var userValue = user != null ? context.ParseResult.GetValueForOption(user) : string.Empty;
                var pwdValue = pwd != null ? context.ParseResult.GetValueForOption(pwd) : string.Empty;
                var logValue = log != null && context.ParseResult.GetValueForOption(log);

                var client = new AsyncFtpClient(hostValue, userValue, pwdValue);
                client.Config.LogToConsole = logValue;
                await client.AutoConnect(context.GetCancellationToken());
                
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
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task Info(InvocationContext context)
        {
            await AnsiConsole.Status()
                     .Spinner(Spinner.Known.Dots12)
                     .StartAsync("Info...", async ctx =>
                     {
                         try
                         {
                             using var client = await GetClient(context, ctx);
                             ctx.Status = "Info...";

                             AnsiConsole.WriteLine(client.ServerType.ToString());
                             AnsiConsole.WriteLine(client.ServerOS.ToString());
                             await client.Disconnect();
                         }
                         catch (Exception ex)
                         {
                             AnsiConsole.WriteLine(ex.Message);
                             if (ex.InnerException != null)
                             {
                                 AnsiConsole.WriteLine(ex.InnerException.Message);
                             }
                             context.ExitCode = 1;
                         }
                     });
        }

        /// <summary>
        /// list entries from path on server
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task List(InvocationContext context)
        {
            await AnsiConsole.Status()
                      .Spinner(Spinner.Known.Dots12)
                      .StartAsync("List...", async ctx =>
                      {
                          try
                          {
                              var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                              using var client = await GetClient(context, ctx);
                              ctx.Status = "List...";
                              var files = await client.GetNameListing(pathValue, context.GetCancellationToken());
                              foreach (var file in files)
                              {
                                  AnsiConsole.WriteLine(file);
                              }
                              await client.Disconnect();
                          }
                          catch (Exception ex)
                          {
                              AnsiConsole.WriteLine(ex.Message);
                              if (ex.InnerException != null)
                              {
                                  AnsiConsole.WriteLine(ex.InnerException.Message);
                              }
                              context.ExitCode = 1;
                          }
                      });
        }

        /// <summary>
        /// delete file or directory on server
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task Delete(InvocationContext context)
        {
            await AnsiConsole.Status()
                       .Spinner(Spinner.Known.Dots12)
                       .StartAsync("Delete...", async ctx =>
                       {
                           try
                           {
                               var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                               using var client = await GetClient(context, ctx);
                               ctx.Status = "Delete...";
                               if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                               {
                                   await client.DeleteDirectory(pathValue, context.GetCancellationToken());
                                   AnsiConsole.WriteLine("Directory deleted");
                               }
                               else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                               {
                                   await client.DeleteFile(pathValue, context.GetCancellationToken());
                                   AnsiConsole.WriteLine("File deleted");
                               }
                               else
                               {
                                   AnsiConsole.WriteLine("File or Directory not exists");
                                   context.ExitCode = 2;
                               }
                               await client.Disconnect();
                           }
                           catch (Exception ex)
                           {
                               AnsiConsole.WriteLine(ex.Message);
                               if (ex.InnerException != null)
                               {
                                   AnsiConsole.WriteLine(ex.InnerException.Message);
                               }
                               context.ExitCode = 1;
                           }
                       });
        }

        /// <summary>
        /// rename file or directory on server
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task Rename(InvocationContext context)
        {
            await AnsiConsole.Status()
                       .Spinner(Spinner.Known.Dots12)
                       .StartAsync("Rename...", async ctx =>
                       {
                           try
                           {
                               var newPathValue = newPath != null ? context.ParseResult.GetValueForOption(newPath) : string.Empty;
                               var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                               using var client = await GetClient(context, ctx);
                               ctx.Status = "Rename...";

                               if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                               {
                                   await client.MoveDirectory(pathValue, newPathValue, FtpRemoteExists.Overwrite, context.GetCancellationToken());
                                   AnsiConsole.WriteLine("Directory renamed");
                               }
                               else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                               {
                                   await client.MoveFile(pathValue, newPathValue, FtpRemoteExists.Overwrite, context.GetCancellationToken());
                                   AnsiConsole.WriteLine("File renamed");
                               }
                               else
                               {
                                   AnsiConsole.WriteLine("File or Directory not exists");
                                   context.ExitCode = 2;
                               }
                               await client.Disconnect();
                           }
                           catch (Exception ex)
                           {
                               AnsiConsole.WriteLine(ex.Message);
                               if (ex.InnerException != null)
                               {
                                   AnsiConsole.WriteLine(ex.InnerException.Message);
                               }
                               context.ExitCode = 1;
                           }
                       });
        }

        /// <summary>
        /// upload folder or file
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task Upload(InvocationContext context)
        {
            await AnsiConsole.Status()
                       .Spinner(Spinner.Known.Dots12)
                       .StartAsync("Prepare Upload...", async ctx =>
                       {
                           try
                           {
                               var localPathValue = localPath != null ? context.ParseResult.GetValueForOption(localPath) : string.Empty;
                               var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                               using var client = await GetClient(context, ctx);
                               ctx.Status = "Prepare Upload...";

                               Progress<FtpProgress> progress = new(p => {
                                   ctx.Status("Upload " + (p.FileIndex + 1) + " of " + p.FileCount + " (" + p.TransferSpeedToString() + ") " + p.RemotePath + " " + (int)p.Progress + "%");
                               });

                               if (Directory.Exists(localPathValue))
                               {

                                   await client.UploadDirectory(localPathValue, pathValue, 
                                       FtpFolderSyncMode.Update, 
                                       FtpRemoteExists.Overwrite, 
                                       FtpVerify.None, null, progress, 
                                       context.GetCancellationToken());

                                   AnsiConsole.WriteLine("Directory uploaded");
                                   context.ExitCode = 0;
                               }
                               else if (File.Exists(localPathValue))
                               {
                                   await client.UploadFile(localPathValue, pathValue, 
                                       FtpRemoteExists.Overwrite, 
                                       true, FtpVerify.None, 
                                       progress, context.GetCancellationToken());
                                   AnsiConsole.WriteLine("File uploaded");
                               }
                               else
                               {
                                   AnsiConsole.WriteLine("File or Directory not exists");
                                   context.ExitCode = 2;
                               }
                               await client.Disconnect();
                           }
                           catch (Exception ex)
                           {
                               AnsiConsole.WriteLine(ex.Message);
                               if (ex.InnerException != null)
                               {
                                   AnsiConsole.WriteLine(ex.InnerException.Message);
                               }
                               context.ExitCode = 1;
                           }
                       });
        }

        /// <summary>
        /// download folder or file
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task Download(InvocationContext context)
        {
            await AnsiConsole.Status()
                       .Spinner(Spinner.Known.Dots12)
                       .StartAsync("Prepare Download...", async ctx =>
                       {
                           try
                           {
                               var localPathValue = localPath != null ? context.ParseResult.GetValueForOption(localPath) : string.Empty;
                               var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                               using var client = await GetClient(context, ctx);
                               ctx.Status = "Prepare Download...";

                               Progress<FtpProgress> progress = new(p => {
                                   ctx.Status("Download " + (p.FileIndex + 1) + " of " + p.FileCount + " (" + p.TransferSpeedToString() + ") " + p.LocalPath + " " + (int)p.Progress + "%");
                               });

                               if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                               {

                                   await client.DownloadDirectory(localPathValue, pathValue,
                                       FtpFolderSyncMode.Update,
                                       FtpLocalExists.Overwrite,
                                       FtpVerify.None, null, progress,
                                       context.GetCancellationToken());

                                   AnsiConsole.WriteLine("Directory downloaded");
                                   context.ExitCode = 0;
                               }
                               else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                               {
                                   await client.DownloadFile(localPathValue, pathValue,
                                       FtpLocalExists.Overwrite,
                                       FtpVerify.None,
                                       progress, context.GetCancellationToken());
                                   AnsiConsole.WriteLine("File downloaded");
                               }
                               else
                               {
                                   AnsiConsole.WriteLine("File or Directory not exists");
                                   context.ExitCode = 2;
                               }
                               await client.Disconnect();
                           }
                           catch (Exception ex)
                           {
                               AnsiConsole.WriteLine(ex.Message);
                               if (ex.InnerException != null)
                               {
                                   AnsiConsole.WriteLine(ex.InnerException.Message);
                               }
                               context.ExitCode = 1;
                           }
                       });
        }

        /// <summary>
        /// main entry point
        /// </summary>
        /// <remarks>
        /// init options and commands
        /// </remarks>
        /// <param name="args">command line parameter</param>
        /// <returns></returns>
        static async Task<int> Main(string[] args)
        {
            host = new Option<string>("--host", "The FTP host")
            {
                IsRequired = true
            };
            host.AddAlias("-h");
            user = new Option<string>("--user", "The FTP user");
            user.AddAlias("-u");
            pwd = new Option<string>("--pwd", "The FTP pwd");
            pwd.AddAlias("-p");
            path = new Option<string>("--path", () => "", "The path to start")
            {
                IsRequired = true
            };
            newPath = new Option<string>("--newPath", () => "", "The path renamed to")
            {
                IsRequired = true
            };
            newPath.AddAlias("-n");
            localPath = new Option<string>("--localPath", () => "", "The local path to upload");
            localPath.AddAlias("-l");
            log = new Option<bool>("--log", () => false, "Show log output");

            var rootCommand = new RootCommand("FTP Helper");

            rootCommand.AddGlobalOption(host);
            rootCommand.AddGlobalOption(user);
            rootCommand.AddGlobalOption(pwd);
            rootCommand.AddGlobalOption(log);

            var listCommand = new Command("list", "List path content on host.")
                                            {
                                                path
                                            };

            var infoCommand = new Command("info", "Get Server Infos.");

            var deleteCommand = new Command("delete", "Delete file or directory on host.")
                                            {
                                                path
                                            };

            var renameCommand = new Command("rename", "Rename file or directory on host.")
                                            {
                                                path,
                                                newPath
                                            };

            var uploadCommand = new Command("upload", "Upload file or directory to host.")
                                            {
                                                path,
                                                localPath
                                            };

            var downloadCommand = new Command("download", "Download file or directory from host.")
                                            {
                                                path,
                                                localPath
                                            };

            rootCommand.AddCommand(listCommand);
            rootCommand.AddCommand(infoCommand);
            rootCommand.AddCommand(deleteCommand);
            rootCommand.AddCommand(renameCommand);
            rootCommand.AddCommand(uploadCommand);
            rootCommand.AddCommand(downloadCommand);

            infoCommand.SetHandler(Info);
            listCommand.SetHandler(List);
            deleteCommand.SetHandler(Delete);
            renameCommand.SetHandler(Rename);
            uploadCommand.SetHandler(Upload);
            downloadCommand.SetHandler(Download);

            return await rootCommand.InvokeAsync(args);
        }
    }
}