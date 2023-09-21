using Core.ResponseContract;
using Core.ResponseContract.Abstract;
using Domain.DataTransferObjects;
using Domain.Repository;
using MediatR; 

namespace Api.Query.Handler;

public class GetRatingDetailRequestHandler : IRequestHandler<GetRatingDetailRequest, IResponse>
{
    private readonly IRatingRepository _repository;
    private const string Instance = nameof(GetRatingDetailRequestHandler);

    public GetRatingDetailRequestHandler(
        IRatingRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<IResponse> Handle(GetRatingDetailRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.Id.Equals(request.Id), cancellationToken);
        if (entity is null) return ErrorResponse.NotFound(Instance);
        var data = new RatingDto
        {
            CustomerId = entity.CustomerId,
            Score = entity.Score,
            CraftsmanId = entity.CraftsmanId,
            TaskId = entity.TaskId
        };
        return DataResponse.Successful(data, Instance);
    }
}