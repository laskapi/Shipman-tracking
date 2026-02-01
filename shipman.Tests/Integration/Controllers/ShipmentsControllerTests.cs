using shipman.Server.Application.Dtos;
using shipman.Server.Domain.Enums;
using shipman.Tests.Integration.Factories;
using shipman.Tests.TestUtils;
using System.Net;
using System.Net.Http.Json;

namespace shipman.Tests.Integration.Controllers;

public class ShipmentsControllerTests
    : IClassFixture<TestApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;

    public ShipmentsControllerTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateShipment_ReturnsCreatedAndValidDto()
    {
        var dto = DtoFactory.CreateShipment();

        var response = await _client.PostAsJsonAsync("/api/shipments", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);

        Assert.NotNull(result);
        Assert.Equal("Berlin", result!.Origin);
        Assert.Equal("Paris", result.Destination);
        Assert.Equal(ServiceType.Standard, result.ServiceType);
    }
    [Fact]
    public async Task GetShipmentById_ReturnsShipment()
    {
        var dto = DtoFactory.CreateShipment();

        var createResponse = await _client.PostAsJsonAsync("/api/shipments", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);
        var response = await _client.GetAsync($"/api/shipments/{created!.Id}");
        var result = await response.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(created.Id, result!.Id);
        Assert.Equal("Berlin", result.Origin);
    }
    [Fact]
    public async Task GetAllShipments_ReturnsPagedResultWithItemsList()
    {
        var dto1 = DtoFactory.CreateShipment();
        var dto2 = DtoFactory.CreateShipment("Bob");

        await _client.PostAsJsonAsync("/api/shipments", dto1);
        await _client.PostAsJsonAsync("/api/shipments", dto2);

        var response = await _client.GetAsync("/api/shipments");
        var paged = await response.Content.ReadFromJsonAsync<PagedResultDto<ShipmentDetailsDto>>(TestJson.Options);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paged);
        Assert.NotNull(paged.Items);
        Assert.True(paged.Items.Count >= 2);
    }
    [Fact]
    public async Task UpdateShipment_UpdatesSelectedFields()
    {
        var createDto = DtoFactory.CreateShipment(); 
               
        var createResponse = await _client.PostAsJsonAsync("/api/shipments", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);
        var updateDto = new UpdateShipmentDto
        {
            Destination = "Rome",
            Weight = 3.0m
        };
        var updateResponse = await _client.PutAsJsonAsync($"/api/shipments/{created!.Id}", updateDto);
        var updated = await updateResponse.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        Assert.NotNull(updated);
        Assert.Equal("Rome", updated!.Destination);
        Assert.Equal(3.0m, updated.Weight);
        Assert.Equal("Berlin", updated.Origin);
        Assert.Equal(ServiceType.Standard, updated.ServiceType);
    }

    [Fact]
    public async Task DeleteShipment_RemovesShipment()
    {
        var dto = DtoFactory.CreateShipment();

        var createResponse = await _client.PostAsJsonAsync("/api/shipments", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);

        var deleteResponse = await _client.DeleteAsync($"/api/shipments/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/shipments/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
