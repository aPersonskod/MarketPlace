using System.Text.Json.Serialization;

namespace BuyReport.Application.Dtos;

public class PaginatedDetailedBuyReportsDto
{
    [JsonPropertyName("recordsCount")]
    public int RecordsCount { get; set; }
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
    [JsonPropertyName("reports")]
    public IEnumerable<DetailedBuyReportDto> Reports { get; set; }
}