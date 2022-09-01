using System;
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
    public async Task VehicleIsCreated()
    {
        // arrange
        var client = new IntegrationTest().client;

        // act
        var response = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // assert
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("BMW R1100S");
    }

    [Fact]
    public async Task ActionTemplateIsAdded()
    {
        // Arrange
        var client = new IntegrationTest().client;
        var createVehicleResponse = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var responseContent = await createVehicleResponse.Content.ReadAsStringAsync();
        var vehicle = JsonConvert.DeserializeAnonymousType(responseContent, new {Id = ""});

        // Act
        var addActionTemplateCommand = new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365) );
        var addActionTemplateResponse = await client.PostAsync(Routes.AddActionTemplate, Serialize(addActionTemplateCommand));

        // Assert
        var actionTemplateResponseContent = await addActionTemplateResponse.Content.ReadAsStringAsync();
        actionTemplateResponseContent.Should().Contain("Oil change");
    }

    private async Task<HttpResponseMessage> CreateVehicleAsync(HttpClient client, CreateVehicleCommand command)
    {
        return await client.PostAsync(Routes.CreateVehicle, Serialize(command));
    }
    
    private StringContent Serialize(object command)
    {
        var json = JsonConvert.SerializeObject(command);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}