using Api.Command;
using Api.ValidationRules;
using Domain.DataTransferObjects;
using FluentValidation.TestHelper;

namespace Api.Test.ValidationRules;

public class CreateRatingRequestValidationTests
{
    private readonly CreateRatingRequestValidation _sut;

    public CreateRatingRequestValidationTests()
    {
        _sut = new CreateRatingRequestValidation();
    }

    [Fact]
    public void when_dto_is_null_should_be_error()
    {
        //Arrange
        RatingDto? dto = null;
        CreateRatingRequest request = new() { Dto = dto };

        //Act
        var act = _sut.TestValidate(request);

        //Assert
        Assert.False(act.IsValid);
        act.ShouldHaveValidationErrorFor(x =>x.Dto);
    }
}