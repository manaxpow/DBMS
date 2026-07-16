using System;

public class SystemConfigurationManager : ISystemConfigurationManager
{
    private object _loader;

    public ConfigurationValue GetSetting(string key)
    {
        return default;
    }

    public void UpdateSetting(string key, ConfigurationValue val)
    {
    }

    public ConfigurationSnapshot LoadGlobalConfig()
    {
        return default;
    }

    public void MergeConfigs()
    {
    }
}
