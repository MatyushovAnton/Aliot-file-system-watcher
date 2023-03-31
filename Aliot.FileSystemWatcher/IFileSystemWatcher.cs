using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliot.FileSystemWatcher
{
    /// <summary>
    /// Определяет механизм для "слежения" за изменениями в определенной директории
    /// </summary>
    internal interface IFileSystemWatcher : IDisposable
    {
        #region Methods
        // Делаю данный метод асинхронным для возможности реализации long-pooling, если таковая потребуется по той или иной причине
        Task StartWatchAsync(string path, CancellationToken cancellationToken);
        #endregion

        #region Events
        event EventHandler<NewFileAppearedEventArgs> NewFileAppeared;
        #endregion
    }
}
