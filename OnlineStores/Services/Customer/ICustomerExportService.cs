public interface ICustomerExportService
{
    Task<ExportFileResponse> ExportCustomersAsync(IEnumerable<Customer> customers, ExportCustomersQuery query, CancellationToken cancellationToken = default);
}
