using Core.ResponseContract.Abstract;
using Domain.DataTransferObjects;
using MediatR;

namespace Api.Command;

public sealed class UpdateRatingRequest : IRequest<IResponse>
{
    public Guid Id { get; set; }
    public ScoreDto Score { get; set; }
}