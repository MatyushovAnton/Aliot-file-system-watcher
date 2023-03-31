using Aliot.FileSystemWatcher;

var monitoringDirectory = Settings.Default.Directory;

Console.WriteLine("Initializing watcher...");

// Composition root
IFileSystemWatcher watcher = new NativeFileSystemWatcher();

using (watcher)
{
    watcher.NewFileAppeared += NotifyFileAppeared;

    Console.WriteLine("Starting to watch in directory:");
    WriteColored(Path.GetFullPath(monitoringDirectory), ConsoleColor.Yellow);

    var cts = new CancellationTokenSource();
    try
    {
        var watchingTask = watcher.StartWatchAsync(monitoringDirectory, cts.Token);
        watchingTask.ContinueWith(originTask => WriteColored($"Something went wrong while monitoring...\r\n{originTask.Exception}", ConsoleColor.Red), TaskContinuationOptions.OnlyOnFaulted);
    }
    catch(Exception ex)
    {
        WriteColored($"An error occured while attempting to start monitoring specified directory:\r\n {ex}", ConsoleColor.Red);
    }

    Console.WriteLine("\r\n\r\nPress any key to exit...");
    Console.ReadKey();
    cts.Cancel();
}

#region Methods
/* Данную локигу, вероятно, стоило бы "убрать" в отдельный компонент
 для упрощения возможности последующей замены оповещения с вывода уведомления в консоль на какое-либо другое поведение,
но т.к. задачей это не регламентируется, будем делать максимально просто (KISS) :) */
void NotifyFileAppeared(object sender, NewFileAppearedEventArgs args)
{
    var relativeFilePath = Path.GetRelativePath(monitoringDirectory, args.FileName);
    Console.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}]: New file appeared in specified directory:");
    WriteColored($"--{relativeFilePath}", ConsoleColor.DarkGreen);
}

void WriteColored(string message, ConsoleColor color)
{
    var prevColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ForegroundColor= prevColor;
}
#endregion