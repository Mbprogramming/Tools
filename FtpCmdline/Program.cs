using FluentFTP;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks.Dataflow;

namespace FtpCmdline
{
    /// <summary>
    /// Main program implementation class and entry point
    /// </summary>
    /// <remarks>
    /// complete implementation <br/>
    /// See the following package documentations <br/>
    /// <see href="https://spectreconsole.net">Spectre.Console</see> <br/>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/commandline/">System.CommandLine</see>
    /// </remarks>
    internal class Program
    {
        #region options
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
        /// Go down the directory tree recursivly
        /// </summary>
        internal static Option<bool>? recursive;
        /// <summary>
        /// exclude items in this list 
        /// </summary>
        internal static Option<string[]>? exclude;
        /// <summary>
        /// skip or overwrite existing files
        /// </summary>
        internal static Option<bool>? skip;
        /// <summary>
        /// list only directory tree
        /// </summary>
        internal static Option<bool>? tree;
        /// <summary>
        /// result output file
        /// </summary>
        internal static Option<string>? output;
        /// <summary>
        /// output file log level
        /// </summary>
        internal static Option<LogLevel>? outputLevel;
        /// <summary>
        /// count of parallel upload/download streams
        /// </summary>
        internal static Option<int>? parallelTasks;
        #endregion

        #region internal functions and variables
        private static async Task<IList<FtpListItem>> GetItems(AsyncFtpClient client, string path, IList<FtpListItem> items, bool recursive, string[]? exclude, CancellationToken token)
        {
            try
            {
                var result = await client.GetListing(path, token);
                if (!token.IsCancellationRequested)
                {
                    foreach (FtpListItem item in result)
                    {
                        if (exclude != null)
                        {
                            var skip = false;
                            foreach (var ex in exclude)
                            {
                                if (item.FullName.Contains(ex))
                                {
                                    skip = true;
                                    break;
                                }

                            }
                            if (skip)
                            {
                                continue;
                            }
                        }

                        items.Add(item);

                        if (item.Type == FtpObjectType.Directory && recursive)
                        {
                            await GetItems(client, path + "/" + item.Name, items, recursive, exclude, token);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return items;
        }

        private static async Task OutputToDo(InvocationContext context, StatusContext ctx, Func<InvocationContext, StatusContext, Logger, Task> todo)
        {
            using var logger = new Logger(context, output, outputLevel, log);
            await todo(context, ctx, logger);
        }

        private static async Task OutputToDoProgress(InvocationContext context, ProgressContext ctx, Func<InvocationContext, ProgressContext, Logger, Task> todo)
        {
            using var logger = new Logger(context, output, outputLevel, log);
            await todo(context, ctx, logger);
        }

        /// <summary>
        /// create and connect ftp client
        /// </summary>
        /// <param name="context">command line context</param>
        /// <param name="context2">Ansi console status context</param>
        /// <param name="logger">output file</param>
        /// <param name="supressStatus">supress status messages</param>
        /// <returns></returns>
        internal static async Task<AsyncFtpClient> GetClient(InvocationContext context, StatusContext context2, Logger logger, bool supressStatus = false)
        {
            try
            {
                var hostValue = host != null ? context.ParseResult.GetValueForOption(host) : string.Empty;
                var userValue = user != null ? context.ParseResult.GetValueForOption(user) : string.Empty;
                var pwdValue = pwd != null ? context.ParseResult.GetValueForOption(pwd) : string.Empty;

                if (!supressStatus)
                {
                    context2.Status = "Connecting...";
                }

                var client = new AsyncFtpClient(hostValue, userValue, pwdValue, 0, null, logger);
                client.Config.LogToConsole = logger.LogToConsole;
                await client.AutoConnect(context.GetCancellationToken());

                return client;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// create and connect ftp client with progress context
        /// </summary>
        /// <param name="context">command line context</param>
        /// <param name="task">progress task</param>
        /// <param name="logger">output file</param>
        /// <param name="supressStatus">supress status messages</param>
        /// <returns></returns>
        internal static async Task<AsyncFtpClient> GetClientProgress(InvocationContext context, ProgressTask task, Logger logger, bool supressStatus = false)
        {
            try
            {
                var hostValue = host != null ? context.ParseResult.GetValueForOption(host) : string.Empty;
                var userValue = user != null ? context.ParseResult.GetValueForOption(user) : string.Empty;
                var pwdValue = pwd != null ? context.ParseResult.GetValueForOption(pwd) : string.Empty;

                if (!supressStatus)
                {
                    task.Description = "Connecting...";
                    task.Value = 50.0;
                }

                var client = new AsyncFtpClient(hostValue, userValue, pwdValue, 0, null, logger);
                client.Config.LogToConsole = logger.LogToConsole;
                await client.AutoConnect(context.GetCancellationToken());

                return client;
            }
            catch
            {
                throw;
            }
        }

        private static Tuple<IList<string>, IList<string>> IterateLocalDirectory(string localPath, IList<string>? directories, IList<string>? files)
        {
            var d = directories ?? new List<string>();
            var f = files ?? new List<string>();

            if (Directory.Exists(localPath))
            {
                d.Add(localPath);
                foreach (var file in Directory.GetFiles(localPath))
                {
                    f.Add(file);
                }
                foreach (var dir in Directory.GetDirectories(localPath))
                {
                    IterateLocalDirectory(dir, d, f);
                }
            }

            return new Tuple<IList<string>, IList<string>>(d, f);
        }

        private static string TrimPad(string inStr, int length)
        {
            var result = inStr;
            if (result.Length > length)
            {
                result = result.Substring(result.Length - length);
            }
            else
            {
                result = result.PadLeft(length);
            }
            return "=> " + result;
        }

        #endregion

        #region commands
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
                         await OutputToDo(context, ctx, async (context, ctx, logger) =>
                         {
                             using var timestampHelper = new TimestampHelper(logger);
                             try
                             {
                                 using var client = await GetClient(context, ctx, logger);
                                 ctx.Status = "Info...";

                                 logger.LogInfo(client.ServerType.ToString());
                                 logger.LogInfo(client.ServerOS.ToString());

                                 await client.Disconnect();
                             }
                             catch (Exception ex)
                             {
                                 logger.LogError(ex.Message);
                                 if (ex.InnerException != null)
                                 {
                                     logger.LogError(ex.InnerException.Message);
                                 }
                                 context.ExitCode = 1;
                             }
                         });
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
                          await OutputToDo(context, ctx, async (context, ctx, logger) =>
                          {
                              using var timestampHelper = new TimestampHelper(logger);
                              try
                              {
                                  var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;
                                  var recursiveValue = recursive != null ? context.ParseResult.GetValueForOption(recursive) : false;
                                  var excludeValue = exclude != null ? context.ParseResult.GetValueForOption(exclude) : null;
                                  var treeValue = tree != null && context.ParseResult.GetValueForOption(tree);

                                  using var client = await GetClient(context, ctx, logger);
                                  ctx.Status = "List...";
                                  var items = await GetItems(client, pathValue ?? string.Empty, new List<FtpListItem>(), recursiveValue, excludeValue, context.GetCancellationToken());
                                  Tree? root = null;
                                  IList<TreeNode> parents = new List<TreeNode>();
                                  int level = 0;
                                  foreach (var item in items)
                                  {
                                      if (treeValue)
                                      {
                                          if (item.Type == FtpObjectType.Directory)
                                          {
                                              logger.LogInfo(item.FullName);
                                              var indeedTemp = item.FullName;
                                              if (pathValue != null && pathValue.Length > 0)
                                              {
                                                  indeedTemp = indeedTemp.Replace(pathValue, "");
                                              }

                                              var indeed = indeedTemp.Where(w => w == '/').Count() - 1;

                                              if (indeed == 0)
                                              {
                                                  if (root != null)
                                                  {
                                                      logger.LogInfo(root.ToString());
                                                  }
                                                  root = new Tree(item.Name);
                                                  parents = new List<TreeNode>();
                                                  level = 0;
                                              }
                                              else
                                              {
                                                  if (indeed == 1 && root != null)
                                                  {
                                                      parents.Add(root.AddNode(item.Name));
                                                      level = 1;
                                                  }
                                                  else
                                                  {
                                                      if (indeed > level)
                                                      {
                                                          parents.Add(parents.Last().AddNode(item.Name));
                                                          level++;
                                                      }
                                                      else if (indeed == level)
                                                      {
                                                          parents.Remove(parents.Last());
                                                          parents.Add(parents.Last().AddNode(item.Name));
                                                      }
                                                      else if (indeed < level)
                                                      {
                                                          while (level > indeed)
                                                          {
                                                              level--;
                                                              parents.Remove(parents.Last());
                                                          }
                                                          parents.Remove(parents.Last());
                                                          parents.Add(parents.Last().AddNode(item.Name));
                                                      }
                                                  }
                                              }
                                          }
                                      }
                                      else
                                      {
                                          logger.LogInfo(item.FullName);
                                      }
                                  }
                                  if (treeValue && root != null)
                                  {
                                      logger.LogInfo(root.ToString());
                                      logger.LogInfo("Found " + items.Where(w => w.Type == FtpObjectType.Directory).Count() + " items");                                  }
                                  else
                                  {
                                      logger.LogInfo("Found " + items.Count + " items");
                                  }
                                  await client.Disconnect();
                              }
                              catch (Exception ex)
                              {
                                  logger.LogError(ex.Message);
                                  if (ex.InnerException != null)
                                  {
                                      logger.LogError(ex.InnerException.Message);
                                  }
                                  context.ExitCode = 1;
                              }
                          });
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
                           await OutputToDo(context, ctx, async (context, ctx, logger) =>
                           {
                               using var timestampHelper = new TimestampHelper(logger);
                               try
                               {
                                   var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                                   using var client = await GetClient(context, ctx, logger);
                                   ctx.Status = "Delete...";
                                   if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                                   {
                                       logger.LogVerbose("Try to delete folder " + pathValue);
                                       await client.DeleteDirectory(pathValue, context.GetCancellationToken());
                                       logger.LogInfo("Folder deleted " + pathValue);
                                   }
                                   else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                                   {
                                       logger.LogVerbose("Try to delete file " + pathValue);
                                       await client.DeleteFile(pathValue, context.GetCancellationToken());
                                       logger.LogInfo("File deleted " + pathValue);
                                   }
                                   else
                                   {
                                       logger.LogWarn("File or Directory not exists");
                                       context.ExitCode = 2;
                                   }
                                   await client.Disconnect();
                               }
                               catch (Exception ex)
                               {
                                   logger.LogError(ex.Message);
                                   if (ex.InnerException != null)
                                   {
                                       logger.LogError(ex.InnerException.Message);
                                   }
                                   context.ExitCode = 1;
                               }
                           });
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
                           await OutputToDo(context, ctx, async (context, ctx, logger) =>
                           {
                               using var timestampHelper = new TimestampHelper(logger);
                               try
                               {
                                   var newPathValue = newPath != null ? context.ParseResult.GetValueForOption(newPath) : string.Empty;
                                   var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;

                                   using var client = await GetClient(context, ctx, logger);
                                   ctx.Status = "Rename...";

                                   if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                                   {
                                       logger.LogVerbose("Try to rename folder " + pathValue + " to " + newPathValue);
                                       await client.MoveDirectory(pathValue, newPathValue, FtpRemoteExists.Overwrite, context.GetCancellationToken());
                                       logger.LogInfo("Folder renamed from " + pathValue + " to " + newPathValue);
                                   }
                                   else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                                   {
                                       logger.LogVerbose("Try to rename file " + pathValue + " to " + newPathValue);
                                       await client.MoveFile(pathValue, newPathValue, FtpRemoteExists.Overwrite, context.GetCancellationToken());
                                       logger.LogInfo("File renamed from " + pathValue + " to " + newPathValue);
                                   }
                                   else
                                   {
                                       logger.LogWarn("File or directory not exists");
                                       context.ExitCode = 2;
                                   }
                                   await client.Disconnect();
                               }
                               catch (Exception ex)
                               {
                                   logger.LogError(ex.Message);
                                   if (ex.InnerException != null)
                                   {
                                       logger.LogError(ex.InnerException.Message);
                                   }
                                   context.ExitCode = 1;
                               }
                           });
                       });
        }

        /// <summary>
        /// upload path with multiple stream
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static async Task UploadParallel(InvocationContext context)
        {
            await AnsiConsole.Progress()
                       .AutoClear(true)
                       .StartAsync(async ctx =>
                       {
                           var mainTask = ctx.AddTask("Starting...");
                           mainTask.StartTask();
                           var currentFile = 1;
                           var allFiles = 1;
                           await OutputToDoProgress(context, ctx, async (context, ctx, logger) =>
                           {
                               using var timestampHelper = new TimestampHelper(logger);
                               try
                               {
                                   var localPathValue = localPath != null ? context.ParseResult.GetValueForOption(localPath) : string.Empty;
                                   var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;
                                   var skipValue = skip != null ? context.ParseResult.GetValueForOption(skip) : true;
                                   var parallelTaskValue = parallelTasks != null ? context.ParseResult.GetValueForOption(parallelTasks) : 1;

                                   mainTask.Description = "Prepare Upload...";

                                   Progress<FtpProgress> progress = new(p =>
                                   {
                                       mainTask.Description = "Upload " + p.RemotePath;
                                       mainTask.Value = p.Progress;
                                       try
                                       {
                                           if ((int)p.Progress == 100)
                                           {
                                               logger.LogInfo("Upload " + p.RemotePath);
                                           }
                                       }
                                       catch (Exception ex)
                                       {
                                           logger.LogError(ex.Message);
                                           if(ex.InnerException != null)
                                           {
                                               logger.LogError(ex.InnerException.Message);
                                           }
                                       }
                                   });

                                   if (Directory.Exists(localPathValue))
                                   {
                                       var directoryCreated = 0;
                                       var fileUpload = 0;
                                       var local = IterateLocalDirectory(localPathValue, null, null);

                                       // Create folder
                                       var progressValue = 100 / local.Item1.Count;
                                       mainTask.Value = 0.0;
                                       var client = await GetClientProgress(context, mainTask, logger);
                                       foreach (var d in local.Item1.OrderBy(o => o.Length).ToList())
                                       {
                                           if (d.Replace(localPathValue, "").Length <= 0)
                                           {
                                               continue;
                                           }
                                           var toCreate = pathValue ?? "";
                                           if (toCreate.Last() != '/')
                                           {
                                               toCreate += "/";
                                           }

                                           toCreate += d.Replace(localPathValue, "")[1..].Replace("\\", "/");
                                           if (toCreate.Length > 0)
                                           {
                                               try
                                               {
                                                   if (!client.IsConnected)
                                                   {
                                                       // TODO: disable screen output
                                                       logger.LogWarn("Reconnect client");
                                                       mainTask.Description = "Reconnecting";
                                                       client.Dispose();
                                                       client = await GetClientProgress(context, mainTask, logger);
                                                   }
                                                   if (!await client.DirectoryExists(toCreate))
                                                   {
                                                       logger.LogVerbose("Try to create " + toCreate);
                                                       mainTask.Description = TrimPad("Create Folder " + toCreate, 60);
                                                       await client.CreateDirectory(toCreate, context.GetCancellationToken());
                                                       mainTask.Increment(progressValue);
                                                       logger.LogInfo("Create folder " + toCreate);
                                                   }
                                                   else
                                                   {
                                                       mainTask.Description = TrimPad("Folder exists " + toCreate, 60);
                                                       mainTask.Increment(progressValue);
                                                       logger.LogInfo("Folder exists " + toCreate);
                                                   }
                                               }
                                               catch (Exception ex)
                                               {
                                                   logger.LogError(ex.Message);
                                                   if (ex.InnerException != null)
                                                   {
                                                       logger.LogError(ex.InnerException.Message);
                                                       AnsiConsole.WriteException(ex.InnerException);
                                                   }
                                                   if (!client.IsConnected)
                                                   {
                                                       logger.LogWarn("Reconnect client");
                                                       mainTask.Description = "Reconnecting";
                                                       client.Dispose();
                                                       client = await GetClientProgress(context, mainTask, logger);
                                                   }
                                                   if (!await client.DirectoryExists(toCreate))
                                                   {
                                                       logger.LogVerbose("Try to create " + toCreate);
                                                       mainTask.Description = "Create Folder " + toCreate;
                                                       await client.CreateDirectory(toCreate, context.GetCancellationToken());
                                                       logger.LogInfo("Create folder " + toCreate);
                                                   }
                                               }
                                           }
                                           directoryCreated++;
                                       }
                                       await client.Disconnect();
                                       client.Dispose();

                                       allFiles = local.Item2.Count;
                                       currentFile = 1;
                                       if (parallelTaskValue > 1 && allFiles > 20)
                                       {
                                           var countPerTask = allFiles / parallelTaskValue;
                                           var listOfList = new List<List<string>>();

                                           for (var i = 0; i < parallelTaskValue; i++)
                                           {
                                               if (i == parallelTaskValue - 1)
                                               {
                                                   var newList = local.Item2.Skip(i * countPerTask).ToList();
                                                   listOfList.Add(newList);
                                                   logger.LogVerbose("Create new parallel list with " + newList.Count + " items");
                                               }
                                               else
                                               {
                                                   var newList = local.Item2.Skip(i * countPerTask).Take(countPerTask).ToList();
                                                   listOfList.Add(newList);
                                                   logger.LogVerbose("Create new parallel list with " + newList.Count + " items");
                                               }
                                           }
                                           var taskList = new List<Task>();
                                           var progressList = new List<double>();
                                           object lockObj = new();

                                           var overallTask = ctx.AddTask(TrimPad("Overall", 60));
                                           var refreshOverall = () =>
                                           {
                                               try
                                               {
                                                   var overallProgress = double.MinValue;
                                                   lock (lockObj)
                                                   {
                                                       overallProgress = progressList.Aggregate((a, b) => (a + b) / 2);
                                                   }
                                                   if (overallProgress > double.MinValue)
                                                   {
                                                       overallTask.Value = overallProgress;
                                                   }
                                               }
                                               catch (Exception ex)
                                               {
                                                   logger.LogError(ex.Message);
                                               }
                                           };
                                           overallTask.StartTask();
                                           for (var i = 0; i < listOfList.Count; i++)
                                           {
                                               var temp = i;
                                               progressList.Add(0.0);
                                               taskList.Add(Task.Run(async () =>
                                               {
                                                   var localIndex = temp;
                                                   var localList = new List<string>(listOfList[localIndex]);
                                                   var increment = 100.0 / localList.Count;
                                                   var task = ctx.AddTask("Uploading ");
                                                   task.StartTask();
                                                   foreach (var item in localList)
                                                   {
                                                       var taskDescription = item.Substring(item.LastIndexOf("\\"));
                                                       task.Description = TrimPad(taskDescription + "(0%)", 60);
                                                       Progress<FtpProgress> progress = new(p =>
                                                       {
                                                           task.Description = TrimPad(taskDescription + "(" + (int)p.Progress + "%)", 60);
                                                       });
                                                       var toCopy = pathValue ?? "";
                                                       if (toCopy.Last() != '/')
                                                       {
                                                           toCopy += "/";
                                                       }

                                                       toCopy += item.Replace(localPathValue, "")[1..].Replace("\\", "/");
                                                       var clientIntern = await GetClientProgress(context, mainTask, logger, true);
                                                       try
                                                       {
                                                           if (!clientIntern.IsConnected)
                                                           {
                                                               logger.LogWarn("Reconnect client (" + localIndex + ")");
                                                               mainTask.Description = "Reconnecting";
                                                               clientIntern.Dispose();
                                                               clientIntern = await GetClientProgress(context, mainTask, logger);
                                                           }
                                                           logger.LogVerbose("Try to upload (" + localIndex + ") " + toCopy);
                                                           await clientIntern.UploadFile(item, toCopy,
                                                                                   skipValue ? FtpRemoteExists.Skip : FtpRemoteExists.Overwrite,
                                                                                   true, FtpVerify.None,
                                                                                   progress, context.GetCancellationToken());
                                                           logger.LogInfo("Uploaded (" + localIndex + ") " + toCopy);
                                                       }
                                                       catch (Exception ex)
                                                       {
                                                           logger.LogError(ex.Message + "(" + localIndex + ") ");
                                                           if (!client.IsConnected)
                                                           {
                                                               logger.LogWarn("Reconnect client (" + localIndex + ")");
                                                               mainTask.Description = "Reconnecting";
                                                               client.Dispose();
                                                               client = await GetClientProgress(context, mainTask, logger);
                                                           }
                                                           logger.LogVerbose("Try to upload (" + localIndex + ") " + toCopy);
                                                           await clientIntern.UploadFile(item, toCopy,
                                                                                   skipValue ? FtpRemoteExists.Skip : FtpRemoteExists.Overwrite,
                                                                                   true, FtpVerify.None,
                                                                                   null, context.GetCancellationToken());
                                                           logger.LogInfo("Uploaded (" + localIndex + ") " + toCopy);
                                                       }
                                                       await client.Disconnect();
                                                       client.Dispose();
                                                       task.Increment(increment);
                                                       lock (lockObj)
                                                       {
                                                           progressList[localIndex] = task.Value;
                                                       }
                                                       refreshOverall();
                                                   }
                                                   task.Value = 100.0;
                                                   task.StopTask();
                                               }));
                                           }
                                           await Task.WhenAll(taskList);
                                           fileUpload = allFiles;
                                           overallTask.StopTask();
                                       }
                                       else
                                       {
                                           logger.LogVerbose("Single threaded upload");
                                           foreach (var f in local.Item2.OrderBy(o => o.Length).ToList())
                                           {
                                               var toCopy = pathValue ?? "";
                                               if (toCopy.Last() != '/')
                                               {
                                                   toCopy += "/";
                                               }

                                               toCopy += f.Replace(localPathValue, "").Substring(1).Replace("\\", "/");
                                               client = await GetClientProgress(context, mainTask, logger, true);
                                               try
                                               {
                                                   if (!client.IsConnected)
                                                   {
                                                       logger.LogWarn("Reconnect client");
                                                       mainTask.Description = "Reconnecting";
                                                       client.Dispose();
                                                       client = await GetClientProgress(context, mainTask, logger);
                                                   }
                                                   logger.LogVerbose("Try to upload " + toCopy);
                                                   await client.UploadFile(f, toCopy,
                                                                           FtpRemoteExists.Skip,
                                                                           true, FtpVerify.None,
                                                                           progress, context.GetCancellationToken());
                                                   logger.LogInfo("Uploaded file " + toCopy);
                                               }
                                               catch (Exception ex)
                                               {
                                                   AnsiConsole.WriteException(ex);
                                                   logger.LogError(ex.Message);
                                                   if (!client.IsConnected)
                                                   {
                                                       logger.LogWarn("Reconnect client");
                                                       mainTask.Description = "Reconnecting";
                                                       client.Dispose();
                                                       client = await GetClientProgress(context, mainTask, logger);
                                                   }
                                                   logger.LogVerbose("Try to upload " + toCopy);
                                                   await client.UploadFile(f, toCopy,
                                                                           FtpRemoteExists.Skip,
                                                                           true, FtpVerify.None,
                                                                           progress, context.GetCancellationToken());
                                                   logger.LogInfo("Uploaded file " + toCopy);
                                               }
                                               await client.Disconnect();
                                               client.Dispose();
                                               currentFile++;
                                               fileUpload++;
                                           }
                                       }

                                       logger.LogInfo("Directory uploaded (" + directoryCreated + " directories and " + fileUpload + " files)");
                                       context.ExitCode = 0;
                                   }
                                   else if (System.IO.File.Exists(localPathValue))
                                   {
                                       var client = await GetClientProgress(context, mainTask, logger);
                                       logger.LogVerbose("Try to upload file " + localPathValue + " to " + pathValue);
                                       await client.UploadFile(localPathValue, pathValue,
                                           FtpRemoteExists.Overwrite,
                                           true, FtpVerify.None,
                                           progress, context.GetCancellationToken());
                                       logger.LogInfo("File uploaded from " + localPathValue + " to " + pathValue);
                                       await client.Disconnect();
                                       client.Dispose();
                                   }
                                   else
                                   {
                                       logger.LogWarn("File not exists " + localPathValue);
                                       context.ExitCode = 2;
                                   }
                               }
                               catch (Exception ex)
                               {
                                   logger.LogError(ex.Message);
                                   if (ex.InnerException != null)
                                   {
                                       logger.LogError(ex.InnerException.Message);
                                   }
                                   context.ExitCode = 1;
                               }
                               mainTask.Value = 100.0;
                               mainTask.StopTask();
                           });
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
                           var currentFile = 1;
                           var allFiles = 1;
                           await OutputToDo(context, ctx, async (context, ctx, logger) =>
                           {
                               using var timestampHelper = new TimestampHelper(logger);
                               try
                               {
                                   var localPathValue = localPath != null ? context.ParseResult.GetValueForOption(localPath) : string.Empty;
                                   var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;
                                   var skipValue = skip != null ? context.ParseResult.GetValueForOption(skip) : true;

                                   ctx.Status = "Prepare Upload...";

                                   Progress<FtpProgress> progress = new(p =>
                                   {
                                       ctx.Status("Upload " + currentFile + " of " + allFiles + " (" + p.TransferSpeedToString() + ") " + p.RemotePath + " " + (int)p.Progress + "%");
                                       try
                                       {
                                           if ((int)p.Progress == 100)
                                           {
                                               logger.LogInfo("Upload " + p.RemotePath);
                                           }
                                       }
                                       catch (Exception ex)
                                       {
                                           logger.LogError(ex.Message);
                                           if (ex.InnerException != null)
                                           {
                                               logger.LogError(ex.InnerException.Message);
                                           }
                                       }
                                   });

                                   if (Directory.Exists(localPathValue))
                                   {
                                       var directoryCreated = 0;
                                       var fileUpload = 0;
                                       var local = IterateLocalDirectory(localPathValue, null, null);

                                       // Create folder
                                       var client = await GetClient(context, ctx, logger);
                                       foreach (var d in local.Item1.OrderBy(o => o.Length).ToList())
                                       {
                                           if (d.Replace(localPathValue, "").Length <= 0)
                                           {
                                               continue;
                                           }
                                           var toCreate = pathValue ?? "";
                                           if (toCreate.Last() != '/')
                                           {
                                               toCreate += "/";
                                           }

                                           toCreate += d.Replace(localPathValue, "").Substring(1).Replace("\\", "/");
                                           if (toCreate.Length > 0)
                                           {
                                               try
                                               {
                                                   if (!client.IsConnected)
                                                   {
                                                       ctx.Status("Reconnecting");
                                                       client.Dispose();
                                                       client = await GetClient(context, ctx, logger);
                                                   }
                                                   if (!await client.DirectoryExists(toCreate))
                                                   {
                                                       ctx.Status("Create Folder " + toCreate);
                                                       await client.CreateDirectory(toCreate, context.GetCancellationToken());
                                                   }
                                               }
                                               catch (Exception)
                                               {
                                                   if (!client.IsConnected)
                                                   {
                                                       ctx.Status("Reconnecting");
                                                       client.Dispose();
                                                       client = await GetClient(context, ctx, logger);
                                                   }
                                                   if (!await client.DirectoryExists(toCreate))
                                                   {
                                                       ctx.Status("Create Folder " + toCreate);
                                                       await client.CreateDirectory(toCreate, context.GetCancellationToken());
                                                   }
                                               }
                                           }
                                           directoryCreated++;
                                       }
                                       await client.Disconnect();
                                       client.Dispose();

                                       foreach (var f in local.Item2.OrderBy(o => o.Length).ToList())
                                       {
                                           var toCopy = pathValue ?? "";
                                           if (toCopy.Last() != '/')
                                           {
                                               toCopy += "/";
                                           }

                                           toCopy += f.Replace(localPathValue, "").Substring(1).Replace("\\", "/");
                                           client = await GetClient(context, ctx, logger, true);
                                           try
                                           {
                                               if (!client.IsConnected)
                                               {
                                                   logger.LogWarn("Reconnect client");
                                                   ctx.Status("Reconnecting");
                                                   client.Dispose();
                                                   client = await GetClient(context, ctx, logger);
                                               }
                                               logger.LogVerbose("Try to copy " + toCopy);
                                               await client.UploadFile(f, toCopy,
                                                                       FtpRemoteExists.Skip,
                                                                       true, FtpVerify.None,
                                                                       progress, context.GetCancellationToken());
                                               logger.LogInfo("Copy " + toCopy);
                                           }
                                           catch (Exception ex)
                                           {
                                               logger.LogError(ex.Message);
                                               if (ex.InnerException != null)
                                               {
                                                   logger.LogError(ex.InnerException.Message);
                                               }
                                               if (!client.IsConnected)
                                               {
                                                   logger.LogWarn("Reconnect client");
                                                   ctx.Status("Reconnecting");
                                                   client.Dispose();
                                                   client = await GetClient(context, ctx, logger);
                                               }
                                               logger.LogVerbose("Try to copy " + toCopy);
                                               await client.UploadFile(f, toCopy,
                                                                       FtpRemoteExists.Skip,
                                                                       true, FtpVerify.None,
                                                                       progress, context.GetCancellationToken());
                                               logger.LogInfo("Copy " + toCopy);
                                           }
                                           await client.Disconnect();
                                           client.Dispose();
                                           currentFile++;
                                           fileUpload++;
                                       }

                                       logger.LogInfo("Directory uploaded (" + directoryCreated + " directories and " + fileUpload + " files)");
                                       context.ExitCode = 0;
                                   }
                                   else if (System.IO.File.Exists(localPathValue))
                                   {
                                       var client = await GetClient(context, ctx, logger);
                                       logger.LogVerbose("Try to copy " + localPathValue);
                                       await client.UploadFile(localPathValue, pathValue,
                                           FtpRemoteExists.Overwrite,
                                           true, FtpVerify.None,
                                           progress, context.GetCancellationToken());
                                       logger.LogInfo("Copy " + localPathValue);
                                       await client.Disconnect();
                                       client.Dispose();
                                   }
                                   else
                                   {
                                       logger.LogWarn("File not exists " + localPathValue);
                                       context.ExitCode = 2;
                                   }
                               }
                               catch (Exception ex)
                               {
                                   logger.LogError(ex.Message);
                                   if (ex.InnerException != null)
                                   {
                                       logger.LogError(ex.InnerException.Message);
                                   }
                                   context.ExitCode = 1;
                               }
                           });
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
                           await OutputToDo(context, ctx, async (context, ctx, logger) =>
                           {
                               using var timestampHelper = new TimestampHelper(logger);
                               try
                               {
                                   var localPathValue = localPath != null ? context.ParseResult.GetValueForOption(localPath) : string.Empty;
                                   var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;
                                   var skipValue = skip != null ? context.ParseResult.GetValueForOption(skip) : true;

                                   using var client = await GetClient(context, ctx, logger);
                                   ctx.Status = "Prepare Download...";

                                   Progress<FtpProgress> progress = new(p =>
                                   {
                                       ctx.Status("Download " + (p.FileIndex + 1) + " of " + p.FileCount + " (" + p.TransferSpeedToString() + ") " + p.LocalPath + " " + (int)p.Progress + "%");
                                       try
                                       {
                                           if ((int)p.Progress == 100)
                                           {
                                               logger.LogInfo("Upload " + p.RemotePath);
                                           }
                                       }
                                       catch (Exception ex)
                                       {
                                           AnsiConsole.WriteException(ex);
                                       }
                                   });

                                   if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                                   {
                                       logger.LogVerbose("Try to download folder " + pathValue);
                                       await client.DownloadDirectory(localPathValue, pathValue,
                                           FtpFolderSyncMode.Update,
                                           skipValue ? FtpLocalExists.Skip : FtpLocalExists.Overwrite,
                                           FtpVerify.None, null, progress,
                                           context.GetCancellationToken());
                                       logger.LogInfo("Folder downloaded " + pathValue);
                                       context.ExitCode = 0;
                                   }
                                   else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                                   {
                                       logger.LogVerbose("Try to download file " + pathValue);
                                       await client.DownloadFile(localPathValue, pathValue,
                                           FtpLocalExists.Overwrite,
                                           FtpVerify.None,
                                           progress, context.GetCancellationToken());
                                       logger.LogInfo("File downloaded " + pathValue);
                                   }
                                   else
                                   {
                                       logger.LogWarn("File or Directory not exists");
                                       context.ExitCode = 2;
                                   }
                                   await client.Disconnect();
                               }
                               catch (Exception ex)
                               {
                                   logger.LogError(ex.Message);
                                   if (ex.InnerException != null)
                                   {
                                       logger.LogError(ex.InnerException.Message);
                                   }
                                   context.ExitCode = 1;
                               }
                           });
                       });
        }

        /// <summary>
        /// download folder or file with parallel streams
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task DownloadParallel(InvocationContext context)
        {
            await AnsiConsole.Progress()
                       .AutoClear(true)
                       .StartAsync(async ctx =>
                       {
                           var mainTask = ctx.AddTask("Starting...");
                           mainTask.StartTask();
                           mainTask.Value = 25;
                           await OutputToDoProgress(context, ctx, async (context, ctx, logger) =>
                           {
                               using var timestampHelper = new TimestampHelper(logger);
                               try
                               {
                                   var localPathValue = localPath != null ? context.ParseResult.GetValueForOption(localPath) : string.Empty;
                                   var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;
                                   var skipValue = skip != null ? context.ParseResult.GetValueForOption(skip) : true;
                                   var parallelTaskValue = parallelTasks != null ? context.ParseResult.GetValueForOption(parallelTasks) : 1;

                                   var client = await GetClientProgress(context, mainTask, logger);
                                   mainTask.Description = "Prepare Download...";
                                   mainTask.Value = 50;

                                   Progress<FtpProgress> progress = new(p =>
                                   {
                                       mainTask.Description = "Download " + (p.FileIndex + 1) + " of " + p.FileCount + " (" + p.TransferSpeedToString() + ") " + p.LocalPath + " " + (int)p.Progress + "%";
                                       try
                                       {
                                           if ((int)p.Progress == 100)
                                           {
                                               logger.LogInfo("Download " + p.LocalPath);
                                           }
                                       }
                                       catch (Exception ex)
                                       {
                                           logger.LogError(ex.Message);
                                           if (ex.InnerException != null)
                                           {
                                               logger.LogError(ex.InnerException.Message);
                                           }
                                       }
                                   });

                                   if (await client.DirectoryExists(pathValue, context.GetCancellationToken()))
                                   {
                                       mainTask.Description = "Count files to download...";
                                       var items = (await GetItems(client, pathValue ?? string.Empty, new List<FtpListItem>(), true, null, context.GetCancellationToken()))
                                                    .Where(w => w.Type == FtpObjectType.File).ToList();

                                       if (items.Count >= 20)
                                       {
                                           mainTask.Description = "Download...";
                                           mainTask.Value = 75;

                                           await client.Disconnect();
                                           client.Dispose();

                                           if (parallelTaskValue > 1 && items.Count > 20)
                                           {
                                               var countPerTask = items.Count / parallelTaskValue;
                                               var listOfList = new List<List<string>>();

                                               for (var i = 0; i < parallelTaskValue; i++)
                                               {
                                                   if (i == parallelTaskValue - 1)
                                                   {
                                                       var newList = items.Skip(i * countPerTask).ToList();
                                                       listOfList.Add(newList.Where(w => w.Type == FtpObjectType.File).Select(s => s.FullName).ToList());
                                                       logger.LogVerbose("Create new parallel list with " + newList.Count + " items");
                                                   }
                                                   else
                                                   {
                                                       var newList = items.Skip(i * countPerTask).Take(countPerTask).ToList();
                                                       listOfList.Add(newList.Where(w => w.Type == FtpObjectType.File).Select(s => s.FullName).ToList());
                                                       logger.LogVerbose("Create new parallel list with " + newList.Count + " items");
                                                   }
                                               }
                                               var taskList = new List<Task>();
                                               var progressList = new List<double>();
                                               object lockObj = new();
                                               object lockObj2 = new();

                                               var overallTask = ctx.AddTask(TrimPad("Overall", 60));
                                               var refreshOverall = () =>
                                               {
                                                   try
                                                   {
                                                       var overallProgress = double.MinValue;
                                                       lock (lockObj)
                                                       {
                                                           overallProgress = progressList.Aggregate((a, b) => (a + b) / 2);
                                                       }
                                                       if (overallProgress > double.MinValue)
                                                       {
                                                           overallTask.Value = overallProgress;
                                                       }
                                                   }
                                                   catch (Exception ex)
                                                   {
                                                       logger.LogError(ex.Message);
                                                   }
                                               };
                                               overallTask.StartTask();
                                               for (var i = 0; i < listOfList.Count; i++)
                                               {
                                                   var temp = i;
                                                   progressList.Add(0.0);
                                                   taskList.Add(Task.Run(async () =>
                                                   {
                                                       var localIndex = temp;
                                                       var localList = new List<string>(listOfList[localIndex]);
                                                       var increment = 100.0 / localList.Count;
                                                       var task = ctx.AddTask("Downloading ");
                                                       task.StartTask();
                                                       foreach (var item in localList)
                                                       {
                                                           var taskDescription = item.Substring(item.LastIndexOf("/"));
                                                           task.Description = TrimPad(taskDescription + "(0%)", 60);
                                                           Progress<FtpProgress> progress = new(p =>
                                                           {
                                                               task.Description = TrimPad(taskDescription + "(" + (int)p.Progress + "%)", 60);
                                                           });
                                                           var toCopy = localPathValue + item.Replace("/", "\\");
                                                           var clientIntern = await GetClientProgress(context, mainTask, logger, true);
                                                           try
                                                           {
                                                               if (!clientIntern.IsConnected)
                                                               {
                                                                   logger.LogWarn("Reconnect client (" + localIndex + ")");
                                                                   mainTask.Description = "Reconnecting";
                                                                   clientIntern.Dispose();
                                                                   clientIntern = await GetClientProgress(context, mainTask, logger);
                                                               }
                                                               logger.LogVerbose("Try to download (" + localIndex + ") " + toCopy);
                                                               await clientIntern.DownloadFile(toCopy, item,
                                                                                        skipValue ? FtpLocalExists.Skip : FtpLocalExists.Overwrite,
                                                                                        FtpVerify.None,
                                                                                        progress, context.GetCancellationToken());
                                                               logger.LogInfo($"Download({localIndex}) {item} => {toCopy}");
                                                           }
                                                           catch (Exception ex)
                                                           {
                                                               logger.LogError(ex.Message + "(" + localIndex + ") " + toCopy);
                                                               if (ex.InnerException != null)
                                                               {
                                                                   logger.LogError(ex.InnerException.Message + "(" + localIndex + ") ");
                                                               }
                                                               if (!clientIntern.IsConnected)
                                                               {
                                                                   logger.LogWarn("Reconnect client (" + localIndex + ")");
                                                                   mainTask.Description = "Reconnecting";
                                                                   client.Dispose();
                                                                   client = await GetClientProgress(context, mainTask, logger);
                                                               }
                                                               logger.LogVerbose("Try to download (" + localIndex + ") " + toCopy);
                                                               await clientIntern.DownloadFile(toCopy, item,
                                                                                         skipValue ? FtpLocalExists.Skip : FtpLocalExists.Overwrite,
                                                                                         FtpVerify.None,
                                                                                         progress, context.GetCancellationToken());
                                                               logger.LogInfo("Downloaded (" + localIndex + ") " + toCopy);
                                                           }
                                                           await clientIntern.Disconnect();
                                                           clientIntern.Dispose();
                                                           task.Increment(increment);
                                                           lock (lockObj)
                                                           {
                                                               progressList[localIndex] = task.Value;
                                                           }
                                                           refreshOverall();
                                                       }
                                                       task.Value = 100.0;
                                                       task.StopTask();
                                                   }));
                                               }
                                               await Task.WhenAll(taskList);
                                               overallTask.StopTask();
                                           }
                                           mainTask.Value = 100;
                                           AnsiConsole.WriteLine("Directory downloaded");
                                           logger.LogInfo("Directory downloaded");
                                           context.ExitCode = 0;
                                           return;
                                       }
                                       else
                                       {
                                           mainTask.Description = "Download...";
                                           mainTask.Value = 75;
                                           logger.LogVerbose("Try to download " + pathValue);
                                           await client.DownloadDirectory(localPathValue, pathValue,
                                               FtpFolderSyncMode.Update,
                                               skipValue ? FtpLocalExists.Skip : FtpLocalExists.Overwrite,
                                               FtpVerify.None, null, progress,
                                               context.GetCancellationToken());
                                           logger.LogInfo("Downloaded " + pathValue);
                                       }
                                       mainTask.Value = 100;
                                       AnsiConsole.WriteLine("Directory downloaded");
                                       context.ExitCode = 0;
                                   }
                                   else if (await client.FileExists(pathValue, context.GetCancellationToken()))
                                   {
                                       mainTask.Description = "Download...";
                                       mainTask.Value = 75;
                                       logger.LogVerbose("Try to download " + pathValue);
                                       await client.DownloadFile(localPathValue, pathValue,
                                           skipValue ? FtpLocalExists.Skip : FtpLocalExists.Overwrite,
                                           FtpVerify.None,
                                           progress, context.GetCancellationToken());
                                       logger.LogInfo("Downloaded " + pathValue);
                                       mainTask.Value = 100;
                                   }
                                   else
                                   {
                                       logger.LogWarn("File or Directory not exists");
                                       context.ExitCode = 2;
                                   }
                                   mainTask.Value = 100;
                                   await client.Disconnect();
                                   client.Dispose();
                                   client = null;
                               }
                               catch (Exception ex)
                               {
                                   logger.LogError(ex.Message);
                                   if (ex.InnerException != null)
                                   {
                                       logger.LogError(ex.InnerException.Message);
                                   }
                                   context.ExitCode = 1;
                               }
                           });
                       });
        }

        /// <summary>
        /// clear items in folder
        /// </summary>
        /// <param name="context">command line context</param>
        /// <returns></returns>
        internal static async Task Clear(InvocationContext context)
        {
            await AnsiConsole.Status()
                      .Spinner(Spinner.Known.Dots12)
                      .StartAsync("Clear...", async ctx =>
                      {
                          await OutputToDo(context, ctx, async (context, ctx, logger) =>
                          {
                              using var timestampHelper = new TimestampHelper(logger);
                              try
                              {
                                  var pathValue = path != null ? context.ParseResult.GetValueForOption(path) : string.Empty;
                                  var recursiveValue = recursive != null ? context.ParseResult.GetValueForOption(recursive) : false;
                                  var excludeValue = exclude != null ? context.ParseResult.GetValueForOption(exclude) : null;

                                  var client = await GetClient(context, ctx, logger);
                                  ctx.Status = "Prepare clear...";
                                  logger.LogVerbose("Count files and folders");
                                  var items = await GetItems(client, pathValue ?? string.Empty, new List<FtpListItem>(), recursiveValue, excludeValue, context.GetCancellationToken());
                                  // delete all files
                                  var index = 1;
                                  var files = items.Where(w => w.Type == FtpObjectType.File).OrderByDescending(o => o.FullName.Length).ToList();
                                  var deleted = 0;
                                  if (files != null)
                                  {
                                      logger.LogVerbose(files.Count + " files to delete");
                                      foreach (var item in files)
                                      {
                                          try
                                          {
                                              if (!client.IsConnected)
                                              {
                                                  logger.LogWarn("Reconnecting");
                                                  ctx.Status("Reconnecting");
                                                  client.Dispose();
                                                  client = await GetClient(context, ctx, logger);
                                              }
                                              logger.LogVerbose("Try to delete " + item.FullName);
                                              await client.DeleteFile(item.FullName, context.GetCancellationToken());
                                              ctx.Status("Delete file " + item.FullName + " (" + index + " of " + files.Count + ")");
                                              logger.LogVerbose("Delete " + item.FullName + " (" + index + " of " + files.Count + ")");
                                              index++;
                                              deleted++;
                                          }
                                          catch (Exception)
                                          {
                                              if (!client.IsConnected)
                                              {
                                                  ctx.Status("Reconnecting");
                                                  logger.LogWarn("Reconnecting");
                                                  client.Dispose();
                                                  client = await GetClient(context, ctx, logger);
                                              }
                                              logger.LogVerbose("Try to delete " + item.FullName);
                                              await client.DeleteFile(item.FullName, context.GetCancellationToken());
                                              ctx.Status("Delete file " + item.FullName + " (" + index + " of " + files.Count + ")");
                                              logger.LogVerbose("Delete " + item.FullName + " (" + index + " of " + files.Count + ")");
                                              index++;
                                              deleted++;
                                          }
                                      }
                                  }
                                  index = 1;
                                  var directories = items.Where(w => w.Type == FtpObjectType.Directory).OrderByDescending(o => o.FullName.Length).ToList();
                                  if (directories != null)
                                  {
                                      logger.LogVerbose(directories.Count + " directories to delete");
                                      foreach (var item in directories)
                                      {
                                          try
                                          {
                                              if (!client.IsConnected)
                                              {
                                                  ctx.Status("Reconnecting");
                                                  logger.LogWarn("Reconnecting");
                                                  client.Dispose();
                                                  client = await GetClient(context, ctx, logger);
                                              }
                                              var filesInPath = await client.GetNameListing(item.FullName, context.GetCancellationToken());
                                              if (filesInPath != null && filesInPath.Count() > 0)
                                              {
                                                  logger.LogVerbose(item.FullName + " contains files");
                                                  continue;
                                              }
                                              logger.LogVerbose("Try to delete " + item.FullName);
                                              await client.DeleteDirectory(item.FullName, context.GetCancellationToken());
                                              ctx.Status("Delete directory " + item.FullName + " (" + index + " of " + directories.Count + ")");
                                              logger.LogInfo("Delete directory " + item.FullName + " (" + index + " of " + directories.Count + ")");
                                              index++;
                                              deleted++;

                                          }
                                          catch (Exception)
                                          {
                                              if (!client.IsConnected)
                                              {
                                                  logger.LogWarn("Reconnecting");
                                                  ctx.Status("Reconnecting");
                                                  client.Dispose();
                                                  client = await GetClient(context, ctx, logger);
                                              }
                                              var filesInPath = await client.GetNameListing(item.FullName, context.GetCancellationToken());
                                              if (filesInPath != null && filesInPath.Count() > 0)
                                              {
                                                  logger.LogVerbose(item.FullName + " contains files");
                                                  continue;
                                              }
                                              logger.LogVerbose("Try to delete " + item.FullName);
                                              await client.DeleteDirectory(item.FullName, context.GetCancellationToken());
                                              ctx.Status("Delete directory " + item.FullName + " (" + index + " of " + directories.Count + ")");
                                              logger.LogInfo("Delete directory " + item.FullName + " (" + index + " of " + directories.Count + ")");
                                              index++;
                                              deleted++;
                                          }
                                      }
                                  }
                                  AnsiConsole.WriteLine("Delete " + deleted + " of " + items.Count() + " items");
                                  logger.LogInfo("Delete " + deleted + " of " + items.Count() + " items");
                                  await client.Disconnect();
                                  client.Dispose();
                              }
                              catch (Exception ex)
                              {
                                  logger.LogError(ex.Message);
                                  if (ex.InnerException != null)
                                  {
                                      logger.LogError(ex.InnerException.Message);
                                  }
                                  context.ExitCode = 1;
                              }
                          });
                      });
        }
        #endregion

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
            recursive = new Option<bool>("--recursive", () => false, "Go down the directory tree recursively");
            recursive.AddAlias("-r");
            exclude = new Option<string[]>("--exclude", "Exclude items in this list");
            exclude.AddAlias("-e");
            skip = new Option<bool>("--skip", () => true, "Skip (true) or overwrite (false) existing files");
            skip.AddAlias("-s");
            tree = new Option<bool>("--tree", () => false, "Show only directory tree");
            tree.AddAlias("-t");
            output = new Option<string>("--output", () => "", "Result output file");
            output.AddAlias("-o");
            outputLevel = new Option<LogLevel>("--outputLevel", () => LogLevel.None, "The output file log level");
            parallelTasks = new Option<int>("--parallel", () => 1, "The count of parallel upload streams");

            var rootCommand = new RootCommand("FTP Helper");

            rootCommand.AddGlobalOption(host);
            rootCommand.AddGlobalOption(user);
            rootCommand.AddGlobalOption(pwd);
            rootCommand.AddGlobalOption(output);
            rootCommand.AddGlobalOption(outputLevel);
            rootCommand.AddGlobalOption(log);

            var listCommand = new Command("list", "List path content on host.")
                                            {
                                                path,
                                                recursive,
                                                exclude,
                                                tree
                                            };

            var infoCommand = new Command("info", "Get Server Info's.");

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
                                                localPath,
                                                skip
                                            };

            var uploadParallelCommand = new Command("multiupload", "Upload directory to host with multiple parallel streams.")
                                            {
                                                path,
                                                localPath,
                                                skip,
                                                parallelTasks
                                            };

            var downloadCommand = new Command("download", "Download file or directory from host.")
                                            {
                                                path,
                                                localPath,
                                                skip
                                            };

            var downloadParallelCommand = new Command("multidownload", "Download file or directory from host with multiple parallel streams.")
                                            {
                                                path,
                                                localPath,
                                                skip,
                                                parallelTasks
                                            };

            var clearCommand = new Command("clear", "Clear items in folder.")
                                            {
                                                path,
                                                recursive,
                                                exclude
                                            };

            rootCommand.AddCommand(listCommand);
            rootCommand.AddCommand(infoCommand);
            rootCommand.AddCommand(deleteCommand);
            rootCommand.AddCommand(renameCommand);
            rootCommand.AddCommand(uploadCommand);
            rootCommand.AddCommand(uploadParallelCommand);
            rootCommand.AddCommand(downloadCommand);
            rootCommand.AddCommand(downloadParallelCommand);
            rootCommand.AddCommand(clearCommand);

            infoCommand.SetHandler(Info);
            listCommand.SetHandler(List);
            deleteCommand.SetHandler(Delete);
            renameCommand.SetHandler(Rename);
            uploadCommand.SetHandler(Upload);
            uploadParallelCommand.SetHandler(UploadParallel);
            downloadCommand.SetHandler(Download);
            downloadParallelCommand.SetHandler(DownloadParallel);
            clearCommand.SetHandler(Clear);

            return await rootCommand.InvokeAsync(args);
        }
    }
}