using MediatR;
using Models;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportQuery(BuyReport Report) : IRequest<BuyReportDto?>;