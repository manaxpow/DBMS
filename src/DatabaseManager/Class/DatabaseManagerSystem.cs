using System;
using System.Collections.Generic;

public class DatabaseManagerSystem
{
    private IDatabaseRegistry _registry;
    private IDatabaseLifecycleManager _lifecycleManager;
    private IDatabaseMetadataManager _metadataManager;
    private IDatabaseConfigurationManager _configurationManager;

    public DatabaseManagerSystem(
        IDatabaseRegistry registry,
        IDatabaseLifecycleManager lifecycleManager,
        IDatabaseMetadataManager metadataManager,
        IDatabaseConfigurationManager configurationManager)
    {
        _registry = registry;
        _lifecycleManager = lifecycleManager;
        _metadataManager = metadataManager;
        _configurationManager = configurationManager;
    }

    public void Initialize()
    {
    }

    public List<DatabaseId> GetDatabases()
    {
        return default;
    }
}
