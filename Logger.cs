using System;

public class Logger
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
        if (IsLog) System.IO.File.AppendAllText(_FILE_NAME, l);
        System.Console.WriteLine(l);
    }

    public void Info(string s)
    {
        string l = GetFormattedOut(s, "INFO", TAG);
        if (IsLog) System.IO.File.AppendAllText(_FILE_NAME, l);
        System.Console.WriteLine(l);
    }

    public void Warn(string s)
    {
        string l = GetFormattedOut(s, "WARN", TAG);
        if (IsLog) System.IO.File.AppendAllText(_FILE_NAME, l);
        System.Console.WriteLine(l);
    }

    public void Error(string s, Exception e)
    {
        string l = GetFormattedOut(s, "ERROR", TAG);
        if (IsLog) System.IO.File.AppendAllText(_FILE_NAME, l);
        System.Console.WriteLine(l, e);
    }
}