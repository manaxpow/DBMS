using Microsoft.AspNetCore.Mvc;

public class TablesController(ITableService tableService) : ControllerBase
{
    private readonly ITableService _tableService = tableService;


}
