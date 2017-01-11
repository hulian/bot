using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT_BOT_State
{
    public class BotState
    {
        private string model;

        private List<Card> handCards = new List<Card>();

        public BotState(string logFile )
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = logFile;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;


        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine(e.FullPath+":"+e.ChangeType);
        }
    }
}
