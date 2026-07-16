using System;

public interface IExportManager
{
    ExportResult ExportData(ExportRequest request);
}
