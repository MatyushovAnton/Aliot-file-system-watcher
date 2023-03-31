using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliot.FileSystemWatcher
{
    internal class NewFileAppearedEventArgs : EventArgs
    {
        #region Ctor
        internal NewFileAppearedEventArgs(string fileName) => FileName = fileName;
        #endregion

        #region Properties
        internal string FileName { get; }
        #endregion
    }
}