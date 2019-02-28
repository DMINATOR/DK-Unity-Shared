using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logs messages to console
/// </summary>
public class LogToConsole : ILogTarget
{
    public LogSettings Settings { get; set; }

    public void Error(string source, string message, Exception ex)
    {
        Debug.LogError($"[{source}] - {message}");

        if( ex != null )
        {
            Debug.LogException(ex);
        }
    }

    public void Info(string source, string message)
    {
        Debug.Log($"[{source}] - {message}");
    }

    public void Warning(string source, string message)
    {
        Debug.LogWarning($"[{source}] - {message}");
    }
}
