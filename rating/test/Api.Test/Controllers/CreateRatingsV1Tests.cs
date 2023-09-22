using System.Net;
using System.Net.Http.Json;
using Api.Test.TestContract;
using Core.ResponseContract;
using Domain.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Test.Controllers;

public class CreateRatingsV1Tests : BaseIntegrationTest
{
    private const string RequestUri = "/api/v1.0/ratings";

    public CreateRatingsV1Tests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async ValueTask when_customer_id_invalid_should_be_badRequest()
    {
        //Arrange
        const int invalidValue = 0;
        const int defaultValue = 1;
        RatingDto body = new()
        {
            CustomerId = invalidValue,
            CraftsmanId = defaultValue,
            TaskId = defaultValue,
            Score = defaultValue
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync(RequestUri, body);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async ValueTask when_craftsman_id_invalid_should_be_badRequest()
    {
        //Arrange
        const int invalidValue = 0;
        const int defaultValue = 1;
        RatingDto body = new()
        {
            CustomerId = defaultValue,
            CraftsmanId = invalidValue,
            TaskId = defaultValue,
            Score = defaultValue
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync(RequestUri, body);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async ValueTask when_task_id_invalid_should_be_badRequest()
    {
        //Arrange
        const int invalidValue = 0;
        const int defaultValue = 1;
        RatingDto body = new()
        {
            CustomerId = defaultValue,
            CraftsmanId = defaultValue,
            TaskId = invalidValue,
            Score = defaultValue
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync(RequestUri, body);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async ValueTask when_score_invalid_should_be_badRequest()
    {
        //Arrange
        const int invalidValue = 0;
        const int defaultValue = 1;
        RatingDto body = new()
        {
            CustomerId = defaultValue,
            CraftsmanId = defaultValue,
            TaskId = defaultValue,
            Score = invalidValue
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync(RequestUri, body);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async ValueTask when_entity_created_should_be_created()
    {
        //Arrange
        const int defaultValue = 2;
        RatingDto body = new()
        {
            CustomerId = defaultValue,
            CraftsmanId = defaultValue,
            TaskId = defaultValue,
            Score = defaultValue
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync(RequestUri, body);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdResult = Assert.IsType<CreatedResult>(response);
        var data = Assert.IsType<CreatedResponse>(createdResult.Value);
        var hasId = data.Extensions.TryGetValue("id", out var id);
        Assert.True(hasId);
        Assert.IsType<Guid>(id);
    }
}