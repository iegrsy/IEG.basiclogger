using System;
using System.Collections.Concurrent;
using System.Threading;

namespace IEG
{
    public class Logger : IDisposable
    {
        private static readonly string _FILE_NAME = ".log.log";

        private static string GetFormattedOut(string s, string t, string tag)
        {
            return $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}\t{t}\t{tag}\t[{System.Threading.Thread.CurrentThread.ManagedThreadId}]\t{s}\n";
        }

        public string TAG { get; private set; }

        public bool IsLog { get; set; }

        public Logger(string tag)
        {
            TAG = tag;
        }

        public void Debug(string s)
        {
            string l = GetFormattedOut(s, "DEBUG", TAG);
            if (IsLog) Write2File(l);
            System.Console.WriteLine(l);
        }

        public void Info(string s)
        {
            string l = GetFormattedOut(s, "INFO", TAG);
            if (IsLog) Write2File(l);
            System.Console.WriteLine(l);
        }

        public void Warn(string s)
        {
            string l = GetFormattedOut(s, "WARN", TAG);
            if (IsLog) Write2File(l);
            System.Console.WriteLine(l);
        }

        public void Error(string s, Exception e)
        {
            string l = GetFormattedOut(s, "ERROR", TAG);
            if (IsLog) Write2File(l);
            System.Console.WriteLine(l, e);
        }

        private ConcurrentQueue<string> logQ = new ConcurrentQueue<string>();

        private void Write2File(string msg)
        {
            if (msg == null || String.IsNullOrEmpty(msg))
                return;

            AddLog(msg);
        }

        private void AddLog(string log)
        {
            logQ.Enqueue(log);
            if (logQ.Count > 10)
                CleanQ();
        }

        private static readonly object _lock = new object();
        private void CleanQ()
        {
            if (logQ?.Count <= 0)
                return;

            new Thread(() =>
            {
                try
                {
                    string log;
                    while (logQ.TryDequeue(out log))
                    {
                        lock (_lock)
                        {
                            System.IO.File.AppendAllText(_FILE_NAME, log);
                        }
                    }
                }
                catch (System.Exception) { }
            }).Start();
        }

        public void Dispose()
        {
            CleanQ();
        }
    }
}