public interface IReportService
{
    Task<RevenueReportResponse> GetRevenueReportAsync(GetRevenueReportQuery query, CancellationToken cancellationToken = default);
}
