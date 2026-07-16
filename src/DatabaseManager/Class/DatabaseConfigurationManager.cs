using System;

public class DatabaseConfigurationManager : IDatabaseConfigurationManager
{

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
