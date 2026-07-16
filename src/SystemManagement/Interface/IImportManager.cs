using System;

public interface IImportManager
{
    ImportResult ImportData(ImportRequest request);
    bool ValidateImportPlan(ImportPlan plan);
}
