using Core.ResponseContract.Abstract;
using Domain.DataTransferObjects;
using MediatR;

namespace Api.Command;

public sealed class CreateRatingRequest : IRequest<IResponse>
{
    public RatingDto Dto { get; set; }
}