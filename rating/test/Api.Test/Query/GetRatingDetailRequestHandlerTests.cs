using System.Linq.Expressions;
using Api.Query;
using Api.Query.Handler; 
using Core.ResponseContract;
using Domain.DataTransferObjects;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Api.Test.Query;

public sealed class GetRatingDetailRequestHandlerTests
{
    private readonly Mock<IRatingRepository> _repository;
    private readonly CancellationToken _cancellationToken;
    private readonly GetRatingDetailRequestHandler _sut;

    public GetRatingDetailRequestHandlerTests()
    {
        _repository = new Mock<IRatingRepository>();
        _cancellationToken = CancellationToken.None;
        _sut = new GetRatingDetailRequestHandler(_repository.Object);
    }

    [Fact]
    public async ValueTask when_entity_not_found_should_error_response()
    {
        //Arrange
        GetRatingDetailRequest request = new() { Id = Guid.NewGuid() };

        RatingEntity? entity = null;

        _repository.Setup(x =>
                x.GetAsync(It.IsAny<Expression<Func<RatingEntity, bool>>>(), _cancellationToken))
            .ReturnsAsync(entity);

        const int expectedStatusCode = StatusCodes.Status404NotFound;

        //Act
        var act = await _sut.Handle(request, _cancellationToken);

        //Assert
        Assert.False(act.Success);
        var response = Assert.IsType<ErrorResponse>(act);
        Assert.Equal(expectedStatusCode, response.Status);
        Assert.Equal($"https://httpstatuses.com/{expectedStatusCode}", response.Type);
        Assert.Equal(ResponseReason.NotFound, response.Reason);
    }

    [Fact]
    public async ValueTask when_entity_found_should_be_success_response()
    {
        //Arrange
        GetRatingDetailRequest request = new() { Id = Guid.NewGuid() };

        RatingEntity entity = new()
        {
            Id = Guid.NewGuid(),
            CraftsmanId = 1,
            CustomerId = 1,
            Score = 4,
            TaskId = 1
        };

        _repository.Setup(x =>
                x.GetAsync(It.IsAny<Expression<Func<RatingEntity, bool>>>(), _cancellationToken))
            .ReturnsAsync(entity);

        const int expectedStatusCode = StatusCodes.Status200OK;

        //Act
        var act = await _sut.Handle(request, _cancellationToken);

        //Assert
        Assert.True(act.Success);
        var response = Assert.IsType<DataResponse>(act);
        Assert.Equal(expectedStatusCode, response.Status);
        Assert.Equal($"https://httpstatuses.com/{expectedStatusCode}", response.Type);
        Assert.Equal(ResponseReason.Ok, response.Reason);
        var hasValue = response.Extensions.TryGetValue("data", out var dto);
        Assert.True(hasValue);
        var data = Assert.IsType<RatingDto>(dto);
        Assert.Equal(entity.CraftsmanId, data.CraftsmanId);
        Assert.Equal(entity.CustomerId, data.CustomerId);
        Assert.Equal(entity.TaskId, data.TaskId);
        Assert.Equal(entity.Score, data.Score);
    }
}