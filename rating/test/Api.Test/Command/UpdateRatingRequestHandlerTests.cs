using System.Linq.Expressions;
using Api.Command;
using Api.Command.Handler;
using Core.ResponseContract;
using Domain.DataTransferObjects;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Test.Command;

public class UpdateRatingRequestHandlerTests
{
    private readonly Mock<IRatingRepository> _repository;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<UpdateRatingRequestHandler>> _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly UpdateRatingRequestHandler _sut;

    public UpdateRatingRequestHandlerTests()
    {
        _repository = new Mock<IRatingRepository>();
        _logger = new Mock<ILogger<UpdateRatingRequestHandler>>();
        _mediator = new Mock<IMediator>();
        _cancellationToken = CancellationToken.None;
        _sut = new UpdateRatingRequestHandler(_repository.Object, _logger.Object, _mediator.Object);
    }

    [Fact]
    public async ValueTask when_entity_not_found_should_be_error_response()
    {
        //Arrange
        UpdateRatingRequest request = new() { Id = Guid.NewGuid(), Score = new ScoreDto(4) };

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

        _repository.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<RatingEntity, bool>>>(), _cancellationToken), Times.Once);
        _repository.Verify(x => x.UpdateScoreAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
        _mediator.Verify(x => x.Publish(It.IsAny<object>(), _cancellationToken), Times.Never);
        _logger.Verify(x => x.LogCritical(It.IsAny<Exception>(), "RATING_RESOURCE_NOT_UPDATED"), Times.Never);
    }

    [Fact]
    public async ValueTask when_entity_not_updated_should_be_error_response()
    {
        //Arrange
        UpdateRatingRequest request = new() { Id = Guid.NewGuid(), Score = new ScoreDto(4) };

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

        const int expectedStatusCode = StatusCodes.Status500InternalServerError;

        Exception? exception = new("An error occured!");

        _repository.Setup(x => x.UpdateScoreAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(exception);

        //Act
        var act = await _sut.Handle(request, _cancellationToken);

        //Assert
        Assert.False(act.Success);
        var response = Assert.IsType<ErrorResponse>(act);
        Assert.Equal(expectedStatusCode, response.Status);
        Assert.Equal($"https://httpstatuses.com/{expectedStatusCode}", response.Type);
        Assert.Equal(ResponseReason.DataLoss, response.Reason);

        _repository.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<RatingEntity, bool>>>(), _cancellationToken), Times.Once);
        _repository.Verify(x => x.UpdateScoreAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        _mediator.Verify(x => x.Publish(It.IsAny<object>(), _cancellationToken), Times.Never);
        _logger.Verify(x => x.LogCritical(exception, "RATING_RESOURCE_NOT_UPDATED"), Times.Once);
    }

    [Fact]
    public async ValueTask when_entity_updated_should_be_success_response()
    {
        //Arrange
        UpdateRatingRequest request = new() { Id = Guid.NewGuid(), Score = new ScoreDto(4) };

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

        const int expectedStatusCode = StatusCodes.Status204NoContent;

        Exception? exception = null;

        _repository.Setup(x => x.UpdateScoreAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(exception);

        //Act
        var act = await _sut.Handle(request, _cancellationToken);

        //Assert
        Assert.False(act.Success);
        var response = Assert.IsType<ErrorResponse>(act);
        Assert.Equal(expectedStatusCode, response.Status);
        Assert.Equal($"https://httpstatuses.com/{expectedStatusCode}", response.Type);
        Assert.Equal(ResponseReason.NoContent, response.Reason);

        _repository.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<RatingEntity, bool>>>(), _cancellationToken), Times.Once);
        _repository.Verify(x => x.UpdateScoreAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);
        _mediator.Verify(x => x.Publish(It.IsAny<object>(), _cancellationToken), Times.Once);
        _logger.Verify(x => x.LogCritical(exception, "RATING_RESOURCE_NOT_UPDATED"), Times.Never);
    }
}