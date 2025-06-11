using System;

public static class Logger {
    public static void Log(string message) {
#if DEBUG
        System.Diagnostics.Logger.Log(message);
#endif
        System.Diagnostics.Trace.WriteLine(message);
    }

    public static void Log(Exception ex) {
#if DEBUG
        System.Diagnostics.Logger.Log(ex.Message);
#endif
        System.Diagnostics.Trace.WriteLine(ex.Message);
    }

    public static void LogError(System.Exception ex, string message) {
#if DEBUG
        System.Diagnostics.Logger.Log("ERROR: " + message);
#endif
        System.Diagnostics.Trace.TraceError(message);
    }


    public static void LogWarning(string message) {
#if DEBUG
        System.Diagnostics.Logger.Log("WARNING: " + message);
#endif
        System.Diagnostics.Trace.TraceWarning(message);
    }

    public static void LogInfo(string message) {
#if DEBUG
        System.Diagnostics.Logger.Log("INFO: " + message);
#endif
        System.Diagnostics.Trace.TraceInformation(message);
    }
}
