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
// 6. Security Management
// =======================
const secFiles = {};

// Interfaces
secFiles['Interface/IAuthenticationManager.cs'] = `using System;\n\npublic interface IAuthenticationManager\n{\n    AuthenticationResult Authenticate(Credential cred);\n    SessionId CreateLoginSession(UserId userId);\n}`;
secFiles['Interface/IAuthorizationManager.cs'] = `using System;\n\npublic interface IAuthorizationManager\n{\n    bool CheckPermission(UserId userId, SecuredResource res, Privilege priv);\n    void GrantPermission(UserId userId, SecuredResource res, Privilege priv);\n}`;
secFiles['Interface/IPrincipalManager.cs'] = `using System;\n\npublic interface IPrincipalManager\n{\n    UserId CreateUser(string username, string password);\n    void AssignRole(UserId userId, RoleId roleId);\n}`;
secFiles['Interface/IConnectionManager.cs'] = `using System;using System.Collections.Generic;\n\npublic interface IConnectionManager\n{\n    ConnectionId OpenConnection(ConnectionContext ctx);\n    void CloseConnection(ConnectionId connId);\n    List<ConnectionId> GetActiveConnections();\n}`;

// Root & Implementation Classes
secFiles['Class/SecurityManagement.cs'] = `using System;\n\npublic class SecurityManagement\n{\n    private IAuthenticationManager _authenticationManager;\n    private IAuthorizationManager _authorizationManager;\n    private IPrincipalManager _principalManager;\n    private IConnectionManager _connectionManager;\n\n    public SecurityManagement(\n        IAuthenticationManager authenticationManager,\n        IAuthorizationManager authorizationManager,\n        IPrincipalManager principalManager,\n        IConnectionManager connectionManager)\n    {\n        _authenticationManager = authenticationManager;\n        _authorizationManager = authorizationManager;\n        _principalManager = principalManager;\n        _connectionManager = connectionManager;\n    }\n\n    public void Initialize()\n    {\n    }\n\n    public void EnforceSecurity()\n    {\n    }\n}`;

secFiles['Class/AuthenticationManager.cs'] = `using System;\n\npublic class AuthenticationManager : IAuthenticationManager\n{\n    private object _hasher;\n    private IPrincipalManager _principalManager;\n\n    public AuthenticationManager(IPrincipalManager principalManager)\n    {\n        _principalManager = principalManager;\n    }\n\n    public AuthenticationResult Authenticate(Credential cred)\n    {\n        return default;\n    }\n\n    public SessionId CreateLoginSession(UserId userId)\n    {\n        return default;\n    }\n\n    public void ValidateToken()\n    {\n    }\n}`;

secFiles['Class/AuthorizationManager.cs'] = `using System;\n\npublic class AuthorizationManager : IAuthorizationManager\n{\n    private object _evaluator;\n    private IPrincipalManager _principalManager;\n\n    public AuthorizationManager(IPrincipalManager principalManager)\n    {\n        _principalManager = principalManager;\n    }\n\n    public bool CheckPermission(UserId userId, SecuredResource res, Privilege priv)\n    {\n        return default;\n    }\n\n    public void GrantPermission(UserId userId, SecuredResource res, Privilege priv)\n    {\n    }\n\n    public void ResolveRoles()\n    {\n    }\n}`;

secFiles['Class/PrincipalManager.cs'] = `using System;\n\npublic class PrincipalManager : IPrincipalManager\n{\n    private object _repo;\n\n    public UserId CreateUser(string username, string password)\n    {\n        return default;\n    }\n\n    public void AssignRole(UserId userId, RoleId roleId)\n    {\n    }\n\n    public void UpdateStatus()\n    {\n    }\n}`;

secFiles['Class/ConnectionManager.cs'] = `using System;using System.Collections.Generic;\n\npublic class ConnectionManager : IConnectionManager\n{\n    private object _registry;\n    private IAuthenticationManager _authenticationManager;\n\n    public ConnectionManager(IAuthenticationManager authenticationManager)\n    {\n        _authenticationManager = authenticationManager;\n    }\n\n    public ConnectionId OpenConnection(ConnectionContext ctx)\n    {\n        return default;\n    }\n\n    public void CloseConnection(ConnectionId connId)\n    {\n    }\n\n    public List<ConnectionId> GetActiveConnections()\n    {\n        return default;\n    }\n\n    public void LimitConnections()\n    {\n    }\n}`;

// Domain Models
secFiles['Class/Credential.cs'] = `using System;\n\npublic class Credential\n{\n}`;
secFiles['Class/AuthenticationResult.cs'] = `using System;\n\npublic class AuthenticationResult\n{\n}`;
secFiles['Class/SessionId.cs'] = `using System;\n\npublic record SessionId(long Id);`;
secFiles['Class/UserId.cs'] = `using System;\n\npublic record UserId(long Id);`;
secFiles['Class/SecuredResource.cs'] = `using System;\n\npublic class SecuredResource\n{\n}`;
secFiles['Class/Privilege.cs'] = `using System;\n\npublic class Privilege\n{\n}`;
secFiles['Class/RoleId.cs'] = `using System;\n\npublic record RoleId(long Id);`;
secFiles['Class/ConnectionContext.cs'] = `using System;\n\npublic class ConnectionContext\n{\n}`;
secFiles['Class/ConnectionId.cs'] = `using System;\n\npublic record ConnectionId(long Id);`;

writeFiles('SecurityManagement', secFiles);


// =======================
// 10. System Management
// =======================
const sysFiles = {};

// Interfaces
sysFiles['Interface/ISystemConfigurationManager.cs'] = `using System;\n\npublic interface ISystemConfigurationManager\n{\n    ConfigurationValue GetSetting(string key);\n    void UpdateSetting(string key, ConfigurationValue val);\n    ConfigurationSnapshot LoadGlobalConfig();\n}`;
sysFiles['Interface/ISystemHealthMonitor.cs'] = `using System;\n\npublic interface ISystemHealthMonitor\n{\n    SystemHealthReport PerformHealthCheck();\n    void RegisterHealthCheck(IHealthCheck check);\n}`;
sysFiles['Interface/IImportManager.cs'] = `using System;\n\npublic interface IImportManager\n{\n    ImportResult ImportData(ImportRequest request);\n    bool ValidateImportPlan(ImportPlan plan);\n}`;
sysFiles['Interface/IExportManager.cs'] = `using System;\n\npublic interface IExportManager\n{\n    ExportResult ExportData(ExportRequest request);\n}`;
sysFiles['Interface/IHealthCheck.cs'] = `using System;\n\npublic interface IHealthCheck\n{\n}`;

// Root & Implementation Classes
sysFiles['Class/SystemManagement.cs'] = `using System;\n\npublic class SystemManagement\n{\n    private ISystemConfigurationManager _systemConfigurationManager;\n    private ISystemHealthMonitor _systemHealthMonitor;\n    private IImportManager _importManager;\n    private IExportManager _exportManager;\n\n    public SystemManagement(\n        ISystemConfigurationManager systemConfigurationManager,\n        ISystemHealthMonitor systemHealthMonitor,\n        IImportManager importManager,\n        IExportManager exportManager)\n    {\n        _systemConfigurationManager = systemConfigurationManager;\n        _systemHealthMonitor = systemHealthMonitor;\n        _importManager = importManager;\n        _exportManager = exportManager;\n    }\n\n    public void Initialize()\n    {\n    }\n\n    public void ShutdownSystem()\n    {\n    }\n}`;

sysFiles['Class/SystemConfigurationManager.cs'] = `using System;\n\npublic class SystemConfigurationManager : ISystemConfigurationManager\n{\n    private object _loader;\n\n    public ConfigurationValue GetSetting(string key)\n    {\n        return default;\n    }\n\n    public void UpdateSetting(string key, ConfigurationValue val)\n    {\n    }\n\n    public ConfigurationSnapshot LoadGlobalConfig()\n    {\n        return default;\n    }\n\n    public void MergeConfigs()\n    {\n    }\n}`;

sysFiles['Class/SystemHealthMonitor.cs'] = `using System;using System.Collections.Generic;\n\npublic class SystemHealthMonitor : ISystemHealthMonitor\n{\n    private List<IHealthCheck> _checks;\n    private ISystemConfigurationManager _systemConfigurationManager;\n\n    public SystemHealthMonitor(ISystemConfigurationManager systemConfigurationManager)\n    {\n        _systemConfigurationManager = systemConfigurationManager;\n    }\n\n    public SystemHealthReport PerformHealthCheck()\n    {\n        return default;\n    }\n\n    public void RegisterHealthCheck(IHealthCheck check)\n    {\n    }\n\n    public void EvaluateStatus()\n    {\n    }\n}`;

sysFiles['Class/ImportManager.cs'] = `using System;\n\npublic class ImportManager : IImportManager\n{\n    private object _planner;\n    private ISystemConfigurationManager _systemConfigurationManager;\n\n    public ImportManager(ISystemConfigurationManager systemConfigurationManager)\n    {\n        _systemConfigurationManager = systemConfigurationManager;\n    }\n\n    public ImportResult ImportData(ImportRequest request)\n    {\n        return default;\n    }\n\n    public bool ValidateImportPlan(ImportPlan plan)\n    {\n        return default;\n    }\n\n    public void ParseFormat()\n    {\n    }\n}`;

sysFiles['Class/ExportManager.cs'] = `using System;\n\npublic class ExportManager : IExportManager\n{\n    private object _planner;\n\n    public ExportResult ExportData(ExportRequest request)\n    {\n        return default;\n    }\n\n    public void FormatOutput()\n    {\n    }\n}`;

// Domain Models
sysFiles['Class/ConfigurationValue.cs'] = `using System;\n\npublic class ConfigurationValue\n{\n}`;
sysFiles['Class/ConfigurationSnapshot.cs'] = `using System;\n\npublic class ConfigurationSnapshot\n{\n}`;
sysFiles['Class/SystemHealthReport.cs'] = `using System;\n\npublic class SystemHealthReport\n{\n}`;
sysFiles['Class/ImportRequest.cs'] = `using System;\n\npublic class ImportRequest\n{\n}`;
sysFiles['Class/ImportResult.cs'] = `using System;\n\npublic class ImportResult\n{\n}`;
sysFiles['Class/ImportPlan.cs'] = `using System;\n\npublic class ImportPlan\n{\n}`;
sysFiles['Class/ExportRequest.cs'] = `using System;\n\npublic class ExportRequest\n{\n}`;
sysFiles['Class/ExportResult.cs'] = `using System;\n\npublic class ExportResult\n{\n}`;

writeFiles('SystemManagement', sysFiles);
