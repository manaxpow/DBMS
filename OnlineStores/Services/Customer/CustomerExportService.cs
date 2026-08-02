using System.Text;

public class CustomerExportService : ICustomerExportService
{
    public Task<ExportFileResponse> ExportCustomersAsync(IEnumerable<Customer> customers, ExportCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var format = query.Format?.ToLower() ?? "csv";
        var fileName = $"customers_export_{DateTime.UtcNow:yyyyMMddHHmmss}.{format}";
        
        if (format == "xlsx")
        {
            // Mock XLSX export
            var content = Encoding.UTF8.GetBytes("Mock XLSX content");
            return Task.FromResult(new ExportFileResponse(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName));
        }
        else
        {
            // Mock CSV export
            var sb = new StringBuilder();
            sb.AppendLine("Id,CompanyName,Domain,Status,Category,CreatedAt");
            foreach (var c in customers)
            {
                sb.AppendLine($"{c.Id},{c.CompanyName},{c.Domain},{c.Status},{c.Category},{c.CreatedAt:O}");
            }
            
            var content = Encoding.UTF8.GetBytes(sb.ToString());
            return Task.FromResult(new ExportFileResponse(content, "text/csv", fileName));
        }
    }
}
