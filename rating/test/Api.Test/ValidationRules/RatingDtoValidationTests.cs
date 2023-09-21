using Api.ValidationRules;
using Domain.DataTransferObjects;
using FluentValidation.TestHelper;

namespace Api.Test.ValidationRules;

public class RatingDtoValidationTests
{
    private readonly RatingDtoValidation _sut;

    public RatingDtoValidationTests()
    {
        _sut = new RatingDtoValidation();
    }

    [Fact]
    public void when_score_less_than_1_should_be_error()
    {
        //Arrange
        RatingDto dto = new()
        {
            Score = 0,
            CustomerId = 1,
            CraftsmanId = 1,
            TaskId = 1
        };

        //Act
        var act = _sut.TestValidate(dto);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.Score);
    }

    [Fact]
    public void when_score_greater_than_5_should_be_error()
    {
        //Arrange
        RatingDto dto = new()
        {
            Score = 6,
            CustomerId = 1,
            CraftsmanId = 1,
            TaskId = 1
        };

        //Act
        var act = _sut.TestValidate(dto);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.Score);
    }

    [Fact]
    public void when_craftsman_id_less_than_1_should_be_error()
    {
        //Arrange
        RatingDto dto = new()
        {
            Score = 4,
            CustomerId = 1,
            CraftsmanId = 0,
            TaskId = 1
        };

        //Act
        var act = _sut.TestValidate(dto);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.CraftsmanId);
    }

    [Fact]
    public void when_customer_id_less_than_1_should_be_error()
    {
        //Arrange
        RatingDto dto = new()
        {
            Score = 4,
            CustomerId = 0,
            CraftsmanId = 1,
            TaskId = 1
        };

        //Act
        var act = _sut.TestValidate(dto);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Fact]
    public void when_task_id_less_than_1_should_be_error()
    {
        //Arrange
        RatingDto dto = new()
        {
            Score = 4,
            CustomerId = 1,
            CraftsmanId = 1,
            TaskId = 0
        };

        //Act
        var act = _sut.TestValidate(dto);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x => x.TaskId);
    }
}