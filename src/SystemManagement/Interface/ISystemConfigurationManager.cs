using System;

public interface ISystemConfigurationManager
{
    ConfigurationValue GetSetting(string key);
    void UpdateSetting(string key, ConfigurationValue val);
    ConfigurationSnapshot LoadGlobalConfig();
}
