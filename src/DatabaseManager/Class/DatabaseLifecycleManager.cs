using System;

public class DatabaseLifecycleManager : IDatabaseLifecycleManager
{
    private IDatabaseRegistry _registry;
    private IDatabaseMetadataManager _metadataManager;
    private IDatabaseConfigurationManager _configurationManager;

    public DatabaseLifecycleManager(
        IDatabaseRegistry registry,
        IDatabaseMetadataManager metadataManager,
        IDatabaseConfigurationManager configurationManager)
    {
        _registry = registry;
        _metadataManager = metadataManager;
        _configurationManager = configurationManager;
    }

    public DatabaseId CreateDatabase(string name)
    {
        return default;
    }

    public void DropDatabase(DatabaseId dbId)
    {
    }

    public void StartDatabase(DatabaseId dbId)
    {
    }

    public void StopDatabase(DatabaseId dbId)
    {
    }

    public void CoordinateStartup()
    {
    }
}
