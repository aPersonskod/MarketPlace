using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportByIdQuery(Guid ReportId) : IRequest<BuyReportDto?>;