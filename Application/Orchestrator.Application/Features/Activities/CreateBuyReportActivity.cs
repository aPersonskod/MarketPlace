using MassTransit;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Arguments;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Activities;

public class CreateBuyReportActivity(IBuyReportRepository buyReportRepository) 
    : IExecuteActivity<CreateBuyReportArguments>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<CreateBuyReportArguments> context)
    {
        var args = context.Arguments;
        await buyReportRepository.CreateBuyReportAsync(
            new CreateBuyReportDto(args.CartId, args.AuthToken));
        return context.Completed();
    }
}

public class CreateBuyReportActivityDefinition 
    : ExecuteActivityDefinition<CreateBuyReportActivity, CreateBuyReportArguments>
{
    public CreateBuyReportActivityDefinition()
    {
        ExecuteEndpointName = "create-buy-report-execute-queue";
    }
}