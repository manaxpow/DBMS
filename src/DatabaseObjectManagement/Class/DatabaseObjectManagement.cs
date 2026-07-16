using System;

public class DatabaseObjectManagement
{
    private ISchemaManager _schemaManager;
    private ITableManager _tableManager;
    private IIndexDefinitionManager _indexDefinitionManager;
    private IViewManager _viewManager;
    private IConstraintManager _constraintManager;
    private ITriggerManager _triggerManager;
    private IStoredProcedureManager _storedProcedureManager;
    private IFunctionManager _functionManager;
    private ISystemCatalog _systemCatalog;

    public DatabaseObjectManagement(
        ISchemaManager schemaManager,
        ITableManager tableManager,
        IIndexDefinitionManager indexDefinitionManager,
        IViewManager viewManager,
        IConstraintManager constraintManager,
        ITriggerManager triggerManager,
        IStoredProcedureManager storedProcedureManager,
        IFunctionManager functionManager,
        ISystemCatalog systemCatalog)
    {
        _schemaManager = schemaManager;
        _tableManager = tableManager;
        _indexDefinitionManager = indexDefinitionManager;
        _viewManager = viewManager;
        _constraintManager = constraintManager;
        _triggerManager = triggerManager;
        _storedProcedureManager = storedProcedureManager;
        _functionManager = functionManager;
        _systemCatalog = systemCatalog;
    }

    public void Initialize()
    {
    }

    public void ResolveObject()
    {
    }
}
