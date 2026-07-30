using MassTransit;
using Microsoft.Extensions.Logging;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Arguments;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Activities;

public class CreateBuyReportActivity(IBuyReportRepository buyReportRepository, ILogger<CreateBuyReportActivity> logger) 
    : IExecuteActivity<CreateBuyReportArguments>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<CreateBuyReportArguments> context)
    {
        try
        {
            var args = context.Arguments;
            await buyReportRepository.CreateBuyReportAsync(
                new CreateBuyReportDto(args.CartId, args.AuthToken));
            return context.Completed();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while create buy report");
            throw;
        }
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