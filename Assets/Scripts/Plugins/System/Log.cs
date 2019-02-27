using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILogTarget
{
    void Info(string source, string message);

    void Warning(string source, string message);

    void Error(string source, string message, Exception ex);
}


/// <summary>
/// Log configuration settings
/// </summary>
public class LogSettings
{
    public bool WriteInfo;
    public bool WriteWarning;
    public bool WriteError;
}


/// <summary>
/// Singleton object to receive log message from any place in application
/// </summary>
public class Log
{
    public ILogTarget Target { get; set; }

    public LogSettings Settings { get; set; }

    private static Log _instance;
    public static Log Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Log();

                //create default settings and target
                _instance.Target = new LogToConsole();

                _instance.Settings = new LogSettings()
                {
                    WriteWarning = true,
                    WriteError = true
                };
            }

            return _instance;
        }
    }


    public void Info(string source, string message)
    {
        if( Target != null && Settings.WriteInfo)
        {
            Target.Info(source, message);
        }
    }

    public void Warning(string source, string message)
    {
        if (Target != null && Settings.WriteWarning)
        {
            Target.Warning(source, message);
        }
    }

    public void Error(string source, string message, Exception ex)
    {
        if (Target != null && Settings.WriteError)
        {
            Target.Error(source, message, ex);
        }
    }
}
