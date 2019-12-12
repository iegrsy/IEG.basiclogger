# IEG.basiclogger

## Install Pack From NuGet

[nuget link](https://www.nuget.org/packages/BasicLogger/)

## Sample Use

```c#
public class Program
{
    private static readonly Logger Log = new Logger(typeof(Program).Name) { IsLog = true };

    public static void Main(string[] args)
    {
        Log.Debug("Debug log");
        Log.Info("Debug log");
        Log.Warn("Debug log");
        Log.Error("Debug log", null);
    }
}
```
