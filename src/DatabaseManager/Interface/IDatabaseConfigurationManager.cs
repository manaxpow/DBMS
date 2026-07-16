using System;

public interface IDatabaseConfigurationManager
{
    DatabaseConfiguration LoadConfiguration(DatabaseId dbId);
    void SaveConfiguration(DatabaseId dbId, DatabaseConfiguration config);
}
