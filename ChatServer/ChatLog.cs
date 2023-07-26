using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatLog : IDisposable
    {
        private string path;
        private static readonly object syncRoot;
        private static FileStream? logFile;
        private static readonly ChatLog instance;
        public static ChatLog Instance => instance;
        public string Path
        {
            get => path;
            set
            {
                if (path != value)
                {
                    path = value;
                    if (logFile != null)
                        logFile.Close();
                    lock (syncRoot)
                    {
                        logFile = new(path, FileMode.Open, FileAccess.ReadWrite);
                    }
                }
            }
        }
        static ChatLog()
        {
            syncRoot = new();
            instance = new();
        }
        private ChatLog()
        {
            path = string.Empty;
            logFile = null;
        }
        public void Write(byte[] buffer, int count)
        {
            lock (syncRoot)
            {
                logFile?.Write(buffer, offset: 0, count);
            }
        }
        public void ReadToEnd(byte[] buffer)
        {
            lock (syncRoot)
            {
                checked
                {
                    logFile?.Read(
                        buffer,
                        offset: 0,
                        (int)logFile.Length
                    );
                }
            }
        }
        public void Dispose()
        {
            logFile?.Flush();
            logFile?.Dispose();
        }
    }
}
