namespace Orchestrator.Application.Dtos;

public record CreateBuyReportDto(Guid CartId, string AuthToken);
public record CreateBuyReportBodyDto(Guid CartId);