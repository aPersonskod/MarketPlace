namespace Orchestrator.Application.Features.Arguments;

public class CreateBuyReportArguments
{
    public Guid CartId { get; set; }
    public decimal AmountToPay { get; set; }
    public string AuthToken { get; set; }
}