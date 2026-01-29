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
        var dto = new CreateShipmentDto
        {
            Sender = "Alice",
            Receiver = "Bob",
            Origin = "Berlin",
            Destination = "Paris",
            Weight = 2.5m,
            ServiceType = ServiceType.Standard
        };

        var response = await _client.PostAsJsonAsync("/api/shipments", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ShipmentDetailsDto>(TestJson.Options);

        Assert.NotNull(result);
        Assert.Equal("Berlin", result!.Origin);
        Assert.Equal("Paris", result.Destination);
        Assert.Equal(ServiceType.Standard, result.ServiceType);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
