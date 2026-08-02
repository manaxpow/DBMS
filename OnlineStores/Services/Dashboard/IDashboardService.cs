public interface IDashboardService
{
    Task<SummaryResponse> GetSummaryAsync(GetSummaryQuery query, CancellationToken cancellationToken = default);
}
