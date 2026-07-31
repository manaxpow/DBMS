using Microsoft.AspNetCore.Mvc;

public class TablesController(ITableService tableService) : ControllerBase
{
    private readonly ITableService _tableService = tableService;

    public TablesController(ITableRepository tableRepository, ITableService tableService)
    {
        _tableRepository = tableRepository;
        _tableService = tableService;
    }
}
