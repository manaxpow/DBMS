const fs = require('fs');
const path = require('path');

const srcDir = path.join('c:', 'Users', 'ADMIN', 'Desktop', 'DBMS', 'src');

function writeFiles(componentName, files) {
    const baseDir = path.join(srcDir, componentName);
    fs.mkdirSync(path.join(baseDir, 'Interface'), { recursive: true });
    fs.mkdirSync(path.join(baseDir, 'Enum'), { recursive: true });
    fs.mkdirSync(path.join(baseDir, 'Class'), { recursive: true });

    for (const [relativePath, content] of Object.entries(files)) {
        const fullPath = path.join(baseDir, relativePath);
        fs.writeFileSync(fullPath, content.trim() + '\n', 'utf8');
        console.log('Created: ' + path.join(componentName, relativePath));
    }
}

// =======================
// 7. Database Manager
// =======================
const dmFiles = {};

// Interfaces
dmFiles['Interface/IDatabaseRegistry.cs'] = `using System;\n\npublic interface IDatabaseRegistry\n{\n    void RegisterDatabase(DatabaseDescriptor desc);\n    void UnregisterDatabase(DatabaseId dbId);\n    DatabaseDescriptor GetDatabase(DatabaseId dbId);\n}`;
dmFiles['Interface/IDatabaseLifecycleManager.cs'] = `using System;\n\npublic interface IDatabaseLifecycleManager\n{\n    DatabaseId CreateDatabase(string name);\n    void DropDatabase(DatabaseId dbId);\n    void StartDatabase(DatabaseId dbId);\n    void StopDatabase(DatabaseId dbId);\n}`;
dmFiles['Interface/IDatabaseMetadataManager.cs'] = `using System;\n\npublic interface IDatabaseMetadataManager\n{\n    DatabaseMetadata GetMetadata(DatabaseId dbId);\n    void UpdateMetadata(DatabaseId dbId, DatabaseMetadata meta);\n}`;
dmFiles['Interface/IDatabaseConfigurationManager.cs'] = `using System;\n\npublic interface IDatabaseConfigurationManager\n{\n    DatabaseConfiguration LoadConfiguration(DatabaseId dbId);\n    void SaveConfiguration(DatabaseId dbId, DatabaseConfiguration config);\n}`;

// Root & Implementation Classes
dmFiles['Class/DatabaseManagerSystem.cs'] = `using System;\nusing System.Collections.Generic;\n\npublic class DatabaseManagerSystem\n{\n    private IDatabaseRegistry _registry;\n    private IDatabaseLifecycleManager _lifecycleManager;\n    private IDatabaseMetadataManager _metadataManager;\n    private IDatabaseConfigurationManager _configurationManager;\n\n    public DatabaseManagerSystem(\n        IDatabaseRegistry registry,\n        IDatabaseLifecycleManager lifecycleManager,\n        IDatabaseMetadataManager metadataManager,\n        IDatabaseConfigurationManager configurationManager)\n    {\n        _registry = registry;\n        _lifecycleManager = lifecycleManager;\n        _metadataManager = metadataManager;\n        _configurationManager = configurationManager;\n    }\n\n    public void Initialize()\n    {\n    }\n\n    public List<DatabaseId> GetDatabases()\n    {\n        return default;\n    }\n}`;

dmFiles['Class/DatabaseRegistry.cs'] = `using System;\n\npublic class DatabaseRegistry : IDatabaseRegistry\n{\n    private object _lookup; // Replacing with generic object as DatabaseLookupService wasn't strictly specified in the domain list\n\n    public void RegisterDatabase(DatabaseDescriptor desc)\n    {\n    }\n\n    public void UnregisterDatabase(DatabaseId dbId)\n    {\n    }\n\n    public DatabaseDescriptor GetDatabase(DatabaseId dbId)\n    {\n        return default;\n    }\n\n    public void ResolveName()\n    {\n    }\n}`;

dmFiles['Class/DatabaseLifecycleManager.cs'] = `using System;\n\npublic class DatabaseLifecycleManager : IDatabaseLifecycleManager\n{\n    private object _bootstrapper; // Bootstrapper omitted from formal domain model list\n    private IDatabaseRegistry _registry;\n    private IDatabaseMetadataManager _metadataManager;\n    private IDatabaseConfigurationManager _configurationManager;\n\n    public DatabaseLifecycleManager(\n        IDatabaseRegistry registry,\n        IDatabaseMetadataManager metadataManager,\n        IDatabaseConfigurationManager configurationManager)\n    {\n        _registry = registry;\n        _metadataManager = metadataManager;\n        _configurationManager = configurationManager;\n    }\n\n    public DatabaseId CreateDatabase(string name)\n    {\n        return default;\n    }\n\n    public void DropDatabase(DatabaseId dbId)\n    {\n    }\n\n    public void StartDatabase(DatabaseId dbId)\n    {\n    }\n\n    public void StopDatabase(DatabaseId dbId)\n    {\n    }\n\n    public void CoordinateStartup()\n    {\n    }\n}`;

dmFiles['Class/DatabaseMetadataManager.cs'] = `using System;\n\npublic class DatabaseMetadataManager : IDatabaseMetadataManager\n{\n    private object _repo;\n\n    public DatabaseMetadata GetMetadata(DatabaseId dbId)\n    {\n        return default;\n    }\n\n    public void UpdateMetadata(DatabaseId dbId, DatabaseMetadata meta)\n    {\n    }\n\n    public void SyncVersion()\n    {\n    }\n}`;

dmFiles['Class/DatabaseConfigurationManager.cs'] = `using System;\n\npublic class DatabaseConfigurationManager : IDatabaseConfigurationManager\n{\n    private object _loader;\n\n    public DatabaseConfiguration LoadConfiguration(DatabaseId dbId)\n    {\n        return default;\n    }\n\n    public void SaveConfiguration(DatabaseId dbId, DatabaseConfiguration config)\n    {\n    }\n\n    public void ValidateConfig()\n    {\n    }\n}`;

// Domain Models
dmFiles['Class/DatabaseId.cs'] = `using System;\n\npublic record DatabaseId(long Id);`;
dmFiles['Class/DatabaseDescriptor.cs'] = `using System;\n\npublic class DatabaseDescriptor\n{\n    public DatabaseId Id { get; set; }\n    public string Name { get; set; }\n}`;
dmFiles['Class/DatabaseMetadata.cs'] = `using System;\n\npublic class DatabaseMetadata\n{\n    public DatabaseId Id { get; set; }\n    public DateTime CreatedAt { get; set; }\n}`;
dmFiles['Class/DatabaseConfiguration.cs'] = `using System;\n\npublic class DatabaseConfiguration\n{\n    public DatabaseId Id { get; set; }\n    public int MaxConnections { get; set; }\n}`;

writeFiles('DatabaseManager', dmFiles);
