public class DashboardService(IDashboardRepository dashboardRepository) : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;

    public async Task<SummaryResponse> GetSummaryAsync(GetSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var summaryData = await _dashboardRepository.GetSummaryDataAsync(query.From, query.To, query.Timezone, query.Currency, cancellationToken);
        return summaryData;
    }
}
