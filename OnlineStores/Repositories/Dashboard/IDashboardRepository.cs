public interface IDashboardRepository
{
    Task<SummaryResponse> GetSummaryDataAsync(DateTime? from, DateTime? to, string? timezone, string? currency, CancellationToken cancellationToken = default);
}
