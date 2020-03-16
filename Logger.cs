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

        private static int _LOG_LEVEL = 0;
        public static int LogLevel { get { return _LOG_LEVEL; } set { _LOG_LEVEL = value; } }

        public string TAG { get; private set; }

        public bool IsLog { get; set; } = false;

        public Logger(string tag)
        {
            TAG = tag;
        }

        public void Log(string s)
        {
            string l = GetFormattedOut(s, "LOG", TAG);
            if (IsLog) Write2File(l);
            System.Console.WriteLine(l);
        }

        public void Debug(string s)
        {
            if (_LOG_LEVEL <= 1)
            {
                string l = GetFormattedOut(s, "DEBUG", TAG);
                if (IsLog) Write2File(l);
                System.Console.WriteLine(l);
            }
        }

        public void Info(string s)
        {
            if (_LOG_LEVEL <= 2)
            {
                string l = GetFormattedOut(s, "INFO", TAG);
                if (IsLog) Write2File(l);
                System.Console.WriteLine(l);
            }
        }

        public void Warn(string s)
        {
            if (_LOG_LEVEL <= 3)
            {
                string l = GetFormattedOut(s, "WARN", TAG);
                if (IsLog) Write2File(l);
                System.Console.WriteLine(l);
            }
        }

        public void Error(string s, Exception e)
        {
            if (_LOG_LEVEL <= 4)
            {
                string l = GetFormattedOut(s, "ERROR", TAG);
                if (IsLog) Write2File($"{l} {e}");
                System.Console.WriteLine(l, e);
            }
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