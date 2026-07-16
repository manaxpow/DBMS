using System;

public class DatabaseConfigurationManager : IDatabaseConfigurationManager
{
    private object _loader;

    public DatabaseConfiguration LoadConfiguration(DatabaseId dbId)
    {
        return default;
    }

    public void SaveConfiguration(DatabaseId dbId, DatabaseConfiguration config)
    {
    }

    public void ValidateConfig()
    {
    }
}
