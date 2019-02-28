using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface ILogTarget
{
    LogSettings Settings { get; set; }

    void Info(string source, string message);

    void Warning(string source, string message);

    void Error(string source, string message, Exception ex);
}


/// <summary>
/// Log configuration settings
/// </summary>
[System.Serializable]
public class LogSettings
{
    public bool WriteInfo;
    public bool WriteWarning;
    public bool WriteError;
}

[System.Serializable]
public class LogTargetSetting
{
    //Indicates if this setting is active
    public bool Active;

    //Name of the type
    public string TypeName;

    //If active what logging options are supported
    public LogSettings Settings;
}


[System.Serializable]
public class LogConfigurationData : DKAsset
{
    public List<LogTargetSetting> Targets;

    public LogSettings GlobalSettings;
}


public class LogTargets : IEnumerable<ILogTarget>
{
    private Dictionary<Type, ILogTarget> _targets = new Dictionary<Type, ILogTarget>();

    public void Add(ILogTarget target)
    {
        if (_targets.ContainsKey(target.GetType()))
        {
            throw new Exception($"Cannot add {nameof(target)} as it already exists.");
        }
        else
        {
            _targets.Add(target.GetType(), target);
        }
    }

    public void Remove(ILogTarget target)
    {
        if (_targets.ContainsKey(target.GetType()))
        {
            _targets.Remove(target.GetType());
        }
        else
        {
            throw new Exception($"Cannot remove {nameof(target)} as it doesn't exists.");
        }
    }

    public IEnumerator<ILogTarget> GetEnumerator()
    {
        return _targets.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _targets.Values.GetEnumerator();
    }

    public ILogTarget this[ILogTarget index]
    {
        get
        {
            return _targets[index.GetType()];
        }

        set
        {
            _targets[index.GetType()] = index;
        }
    }

}

/// <summary>
/// Singleton object to receive log message from any place in application
/// </summary>
public class Log
{
    //Available targets for Log output
    public LogTargets ActiveTargets { get; set; } = new LogTargets();

    //Supported logging targets
    public Dictionary<string, ILogTarget> SupportedTargets { get; set; } = new Dictionary<string, ILogTarget>();

    //internally stored configuration data
    private LogConfigurationData _configurationData = new LogConfigurationData();

    private static Log _instance;
    public static Log Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Log();

                //create default target to console
                _instance.SupportedTargets.Add(typeof(LogToConsole).Name, new LogToConsole());

                _instance.Load();
            }
            //else - instance created, re-use

            return _instance;
        }
    }

    /// <summary>
    /// Loads default settings
    /// </summary>
    public void Load()
    {
        _configurationData = AssetLoader.Load<LogConfigurationData>();

        //load defaults if no custom configuration was specified
        if( (_configurationData == null) || (_configurationData.Targets == null) )
        {
            _configurationData = Log.GetDefaults();
        }

        ActiveTargets = new LogTargets();

        foreach (var target in _configurationData.Targets)
        {
            if (target.Active)
            {
                if (SupportedTargets.Keys.Contains(target.TypeName))
                {
                    //match found, create this type
                    ActiveTargets.Add(SupportedTargets[target.TypeName]);
                    ActiveTargets[SupportedTargets[target.TypeName]].Settings = target.Settings;
                }
                //else - match not found - skip this type
            }
            //else - inactive
        }
    }

    public static LogConfigurationData GetDefaults()
    {
        return new LogConfigurationData()
        {
            GlobalSettings = new LogSettings()
            {
                WriteError = true,
                WriteInfo = true,
                WriteWarning = true
            },

            Targets = new List<LogTargetSetting>()
            {
                new LogTargetSetting()
                {
                    Active = true,

                    Settings = new LogSettings()
                    {
                        WriteError = true,
                        WriteInfo = true,
                        WriteWarning = true
                    },

                    TypeName = "LogToConsole"
                }
            }
        };
    }

    public void Info(string source, string message)
    {
        if (_configurationData.GlobalSettings.WriteInfo)
        {
            foreach (ILogTarget target in ActiveTargets)
            {
                if (target.Settings.WriteInfo)
                {
                    target.Info(source, message);
                }
            }
        }
    }

    public void Warning(string source, string message)
    {
        if (_configurationData.GlobalSettings.WriteWarning)
        {
            foreach (ILogTarget target in ActiveTargets)
            {
                if (target.Settings.WriteWarning)
                {
                    target.Warning(source, message);
                }
            }
        }
    }

    public void Error(string source, string message, Exception ex = null)
    {
        if (_configurationData.GlobalSettings.WriteError)
        {
            foreach (ILogTarget target in ActiveTargets)
            {
                if (target.Settings.WriteError)
                {
                    target.Error(source, message, ex);
                }
            }
        }
    }
}
