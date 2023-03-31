using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Aliot.FileSystemWatcher
{
    /// <summary>
    /// Реализация <see cref="IFileSystemWatcher"/> с использованием стандартного механизма .NET 
    /// </summary>
    internal class NativeFileSystemWatcher : IFileSystemWatcher
    {
        #region Ctor
        internal NativeFileSystemWatcher() => _watcher = new System.IO.FileSystemWatcher();
        #endregion

        #region Fields
        private readonly System.IO.FileSystemWatcher _watcher;
        #endregion

        #region Implementation of IDisposable
        public void Dispose() => _watcher.Dispose();
        #endregion

        #region Implementation of IFileSystemWatcher
        public event EventHandler<NewFileAppearedEventArgs> NewFileAppeared;
        public Task StartWatchAsync(string path, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                throw new ArgumentException("Specified argument does not represent correct directory path.", nameof(path));
            if (_watcher.EnableRaisingEvents)
                throw new InvalidOperationException("Already running.");
            
            cancellationToken.Register(() => 
             _watcher.EnableRaisingEvents = false);
            _watcher.Path = path;
            _watcher.NotifyFilter = NotifyFilters.FileName;
            _watcher.Created +=  OnCreated;
            _watcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }
        #endregion

        #region Methods
        private void OnCreated(object sender, FileSystemEventArgs ea) =>
            NewFileAppeared?.Invoke(this, new NewFileAppearedEventArgs(ea.FullPath));
        #endregion
    }
}
