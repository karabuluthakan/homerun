using Core.ResponseContract.Abstract;
using MediatR;

namespace Api.Query;

public sealed class GetRatingDetailRequest : IRequest<IResponse>
{
    public Guid Id { get; set; }
}