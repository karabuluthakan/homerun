using Api.Extensions;
using Core.ResponseContract;
using Core.ResponseContract.Abstract; 
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
        var data = entity.ToDto();
        return DataResponse.Successful(data, Instance);
    }
}