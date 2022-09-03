using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using maintenance_buddy_api.api;
using maintenance_buddy_api.api.commands;
using Newtonsoft.Json;
using Xunit;

namespace maintenance_buddy_api_integration_test;

public class VehicleEndpointTests
{
    [Fact]
    public async Task CreateVehicleCommand_CreatesANewVehicle()
    {
        // arrange
        var client = new IntegrationTest().client;

        // act
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // assert
        vehicle.Name.Should().Be("BMW R1100S");
    }

    [Fact]
    public async Task AddActionTemplateCommand_CreatesANewActionTemplate()
    {
        // Arrange
        var client = new IntegrationTest().client;
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));

        // Act
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));

        // Assert
        actionTemplate.Name.Should().Be("Oil change");
    }
    
    [Fact]
    public async Task ActionTemplatesQuery_ContainsCorrectActionTemplate()
    {
        // Arrange
        var client = new IntegrationTest().client;

        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));
        
        // get the action templates of a vehicle
        var actionTemplates = await GetActionTemplatesAsync(client, vehicle.Id);
        
        // Assert
        actionTemplates.Should().Contain(_ => _.Id.Equals(actionTemplate.Id));
    }
    
    [Fact]
    public async Task DeleteActionTemplateCommand_DeletesActionTemplate()
    {
        // Arrange
        var client = new IntegrationTest().client;
        
        // add vehicle
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));

        // delete action template
        var deleteActionTemplateCommand = new DeleteActionTemplateCommand(vehicle.Id, actionTemplate.Id);
        await client.PostAsync(Routes.DeleteActionTemplate, Serialize(deleteActionTemplateCommand));
        
        // get the action templates of a vehicle
        var actionTemplates = await GetActionTemplatesAsync(client, vehicle.Id);
        
        // Assert
        actionTemplates.Should().BeEmpty();
    }

    
    [Fact]
    public async Task AddAction_CreatesAction()
    {
        // Arrange
        var client = new IntegrationTest().client;
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));
        
        // act

        var actionDate = new DateTime(2022,9,3);
        var addActionCommand = new AddActionCommand(vehicle.Id, actionTemplate.Id, actionDate, 2000, "5W50");
        await client.PostAsync(Routes.AddAction, Serialize(addActionCommand));
        
        
        var actions = await GetActionsAsync(client, vehicle.Id, actionTemplate.Id);
        
        // Assert
        actions.Should().Contain(_ => _.Date.Equals(actionDate));
    }

    [Fact]
    public async Task DeleteAction_CreatesAction()
    {
        // Arrange
        var client = new IntegrationTest().client;
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));
        
        // act

        var actionDate = new DateTime(2022,9,3);
        var addActionCommand = new AddActionCommand(vehicle.Id, actionTemplate.Id, actionDate, 2000, "5W50");
        await client.PostAsync(Routes.AddAction, Serialize(addActionCommand));
        
        
        var actions = await GetActionsAsync(client, vehicle.Id, actionTemplate.Id);
        
        // Assert
        actions.Should().Contain(_ => _.Date.Equals(actionDate));
    }
    
    
    private async Task<IEnumerable<ActionDto>> GetActionsAsync(HttpClient client, string vehicleId, string actionTemplateId)
    {
        var response = await client.GetAsync($"{Routes.ActionsQuery}/?vehicleId={vehicleId}&actionTemplateId={actionTemplateId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<ActionDto>>(responseContent);
    }

    private async Task<IEnumerable<ActionTemplateDto>> GetActionTemplatesAsync(HttpClient client, string vehicleId)
    {
        var response = await client.GetAsync($"{Routes.ActionTemplateQuery}/?vehicleId={vehicleId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<ActionTemplateDto>>(responseContent);
    }

    private async Task<ActionTemplateDto> AddActionTemplateAsync(HttpClient client, AddActionTemplateCommand addActionTemplateCommand)
    {
        var response = await client.PostAsync(Routes.AddActionTemplate, Serialize(addActionTemplateCommand));
        var addActionTemplateResponseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ActionTemplateDto>(addActionTemplateResponseContent);
    }

    private async Task<VehicleDto> CreateVehicleAsync(HttpClient client, CreateVehicleCommand command)
    {
        var response = await client.PostAsync(Routes.CreateVehicle, Serialize(command));
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<VehicleDto>(responseContent);
    }
    
    private StringContent Serialize(object command)
    {
        var json = JsonConvert.SerializeObject(command);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}

record VehicleDto(string Id, string Name);

record ActionTemplateDto(string Id, string Name);

record ActionDto(DateTime Date, int Kilometer);