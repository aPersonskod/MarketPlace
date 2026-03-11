using MediatR;
using Models;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportQuery(BuyReport Report, string AccessToken) : IRequest<BuyReportDto?>;