namespace Orchestrator.Application.Features.SagaLogs;

public class WalletSpendLog
{
    public Guid CartId { get; set; }
    public decimal AmountToPay { get; set; }
    public string AuthToken { get; set; }
}