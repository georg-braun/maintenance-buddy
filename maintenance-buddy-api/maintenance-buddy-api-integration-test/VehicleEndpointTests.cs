using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using maintenance_buddy_api.api;
using maintenance_buddy_api.api.commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace maintenance_buddy_api_integration_test;

public class VehicleEndpointTests
{
    [Fact]
    public async Task VehicleCreateEndpointIsSuccessful()
    {
        // arrange
        var client = new IntegrationTest().client;

        // act
        var createVehicleCommand = new CreateVehicleCommand("BMW R1100S", 39000);
        var createVehicleResponse = await client.PostAsync(Routes.CreateVehicle, Serialize(createVehicleCommand));
        
        // assert
        var content = await createVehicleResponse.Content.ReadAsStringAsync();
        content.Should().Contain("BMW R1100S");
    }
    
    private StringContent Serialize(object command)
    {
        var json = JsonConvert.SerializeObject(command);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}