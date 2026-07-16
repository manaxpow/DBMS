using System;

public class ImportManager : IImportManager
{
    private object _planner;
    private ISystemConfigurationManager _systemConfigurationManager;

    public ImportManager(ISystemConfigurationManager systemConfigurationManager)
    {
        _systemConfigurationManager = systemConfigurationManager;
    }

    public ImportResult ImportData(ImportRequest request)
    {
        return default;
    }

    public bool ValidateImportPlan(ImportPlan plan)
    {
        return default;
    }

    public void ParseFormat()
    {
    }
}
