using Api.Command;
using Api.ValidationRules;
using Domain.DataTransferObjects;
using FluentValidation.TestHelper;

namespace Api.Test.ValidationRules;

public class UpdateRatingRequestValidationTests
{
    private readonly UpdateRatingRequestValidation _sut;

    public UpdateRatingRequestValidationTests()
    {
        _sut = new UpdateRatingRequestValidation();
    }

    [Fact]
    public void when_id_is_empty_guid_should_be_error()
    {
        //Arrange
        Guid id = Guid.Empty;
        ScoreDto score = new(2);
        UpdateRatingRequest request = new() { Id = id, Score = score };

        //Act
        var act = _sut.TestValidate(request);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void when_score_dto_is_null_should_be_error()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        ScoreDto score = null;
        UpdateRatingRequest request = new() { Id = id, Score = score };

        //Act
        var act = _sut.TestValidate(request);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.Score);
    }

    [Fact]
    public void when_score_less_than_1_should_be_error()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        ScoreDto score = new(0);
        UpdateRatingRequest request = new() { Id = id, Score = score };

        //Act
        var act = _sut.TestValidate(request);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.Score.Score);
    }

    [Fact]
    public void when_score_greater_than_5_should_be_error()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        ScoreDto score = new(6);
        UpdateRatingRequest request = new() { Id = id, Score = score };

        //Act
        var act = _sut.TestValidate(request);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.Score.Score);
    }
}