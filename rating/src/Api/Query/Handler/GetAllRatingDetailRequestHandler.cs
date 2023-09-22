using Api.Extensions;
using Core.ResponseContract;
using Core.ResponseContract.Abstract;
using Domain.DataTransferObjects;
using Domain.Repository;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Api.Query.Handler;

public sealed class GetAllRatingDetailRequestHandler : IRequestHandler<GetAllRatingDetailRequest, IResponse>
{
    private const string Instance = nameof(GetAllRatingDetailRequestHandler);
    private readonly IRatingRepository _repository;

    public GetAllRatingDetailRequestHandler(IRatingRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<IResponse> Handle(
        GetAllRatingDetailRequest request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken: cancellationToken);
        var count = entities.Count < 4 ? 4 : entities.Count;
        List<RatingDto> data = new(count);
        if (!entities.IsNullOrEmpty()) data.AddRange(entities.Select(x => x.ToDto()).ToList());
        return DataResponse.Successful(data, Instance);
    }
}