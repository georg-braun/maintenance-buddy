using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using maintenance_buddy_api.api;
using maintenance_buddy_api.api.commands;
using maintenance_buddy_api.domain;
using Newtonsoft.Json;
using Xunit;

namespace maintenance_buddy_api_integration_test;

public class VehicleEndpointTests
{
    [Fact]
    public async Task CreateVehicleCommand_CreatesANewVehicle()
    {
        // arrange
        var client = new IntegrationTest().GetClient();

        // act
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // assert
        vehicle.Name.Should().Be("BMW R1100S");
    }

    [Fact]
    public async Task AddActionTemplateCommand_CreatesANewActionTemplate()
    {
        // Arrange
        var client = new IntegrationTest().GetClient();
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
        var client = new IntegrationTest().GetClient();

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
        var client = new IntegrationTest().GetClient();
        
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
        var client = new IntegrationTest().GetClient();
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
    public async Task GetAllActionsOfAVehicle()
    {
        // Arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var oilActionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));
        var tireActionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Tires", 5000, new TimeSpan(365)));
        
        // act
        var actionDate = new DateTime(2022,9,3);
        var addOilActionCommand = new AddActionCommand(vehicle.Id, oilActionTemplate.Id, actionDate, 2000, "5W50");
        await client.PostAsync(Routes.AddAction, Serialize(addOilActionCommand));
        var addTireActionCommand = new AddActionCommand(vehicle.Id, tireActionTemplate.Id, actionDate, 2000, "Bridgestone");
        await client.PostAsync(Routes.AddAction, Serialize(addTireActionCommand));

        
        var actions = await GetActionsOfVehicleAsync(client, vehicle.Id);
        
        // Assert
        actions.Should().HaveCount(2);
        actions.Should().AllSatisfy(_ => _.ActionTemplateId.Should().NotBeEmpty());
    }

    [Fact]
    public async Task DeleteAction_DeletesAction()
    {
        // Arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));
        
        var actionDate = new DateTime(2022,9,3);
        var action = await AddActionAsync(client, new AddActionCommand(vehicle.Id, actionTemplate.Id, actionDate, 2000, "5W50"));
        
        // act
        await DeleteAction(client, vehicle.Id, actionTemplate.Id, action.Id.ToString());

        // Assert
        var actions = await GetActionsAsync(client, vehicle.Id, actionTemplate.Id);
        actions.Should().NotContain(_ => _.Date.Equals(actionDate));
    }

    [Fact]
    public async Task GetAllVehicles()
    {
        // arrange
        var client = new IntegrationTest().GetClient();
        await CreateVehicleAsync(client, new CreateVehicleCommand("Opel Astra", 60000));
        await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 40000));
        
        // act
        var vehicles = await GetVehiclesAsync(client);
        
        // assert
        vehicles.Should().HaveCount(2);
        vehicles.Should().Contain(_ => _.Name.Equals("Opel Astra"));
        vehicles.Should().Contain(_ => _.Name.Equals("BMW R1100S"));
    }
    
    [Fact]
    public async Task UserGetsOnlyTheirData()
    {
        // arrange
        var integrationTest = new IntegrationTest();
        var client = integrationTest.GetClient();
        integrationTest.SetClientSession("George");
        await CreateVehicleAsync(client, new CreateVehicleCommand("Opel Astra", 60000));
        await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 40000));
        
        
        integrationTest.SetClientSession("Veronika");
        await CreateVehicleAsync(client, new CreateVehicleCommand("Opel Adam", 70000));
        await CreateVehicleAsync(client, new CreateVehicleCommand("Cube Stereo Hybrid Race", 1000));
        
        // act
        integrationTest.SetClientSession("George");
        var vehicles = await GetVehiclesAsync(client);


        // assert
        vehicles.Should().HaveCount(2);
        vehicles.Should().Contain(_ => _.Name.Equals("Opel Astra"));
        vehicles.Should().Contain(_ => _.Name.Equals("BMW R1100S"));
        vehicles.Should().NotContain(_ => _.Name.Equals("Opel Adam"));
        vehicles.Should().NotContain(_ => _.Name.Equals("Cube Stereo Hybrid Race"));
    }
    
    [Fact]
    public async Task UserCantAddAnActionToAVehicleHeDoesntBelongTo()
    {
        // arrange
        var integrationTest = new IntegrationTest();
        var client = integrationTest.GetClient();
        integrationTest.SetClientSession("George");
        await CreateVehicleAsync(client, new CreateVehicleCommand("Opel Astra", 60000));
        await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 40000));
        
        
        integrationTest.SetClientSession("Veronika");
        await CreateVehicleAsync(client, new CreateVehicleCommand("Opel Adam", 70000));
        await CreateVehicleAsync(client, new CreateVehicleCommand("Cube Stereo Hybrid Race", 1000));
        
        integrationTest.SetClientSession("George");
        var georgesVehicles = await GetVehiclesAsync(client);
        
        // act
        // test that Veronika can't add an action template to Georges vehicles
        integrationTest.SetClientSession("Veronika");
        var aVehicleOfGeorge = georgesVehicles.First();
        var response = await client.PostAsync(Routes.AddActionTemplate, 
            Serialize(new AddActionTemplateCommand(aVehicleOfGeorge.Id, "Oil", 5000, TimeSpan.FromDays(365))));
        

        // assert
        integrationTest.SetClientSession("George");
        var actionTemplates = await GetActionTemplatesAsync(client, aVehicleOfGeorge.Id);

        actionTemplates.Should().HaveCount(0);
    }

    private async Task DeleteAction(HttpClient client, string vehicleId, string actionTemplateId, string actionId)
    {
        var deleteActionCommand = new DeleteActionCommand(vehicleId, actionTemplateId, actionId);
        await client.PostAsync(Routes.DeleteAction, Serialize(deleteActionCommand));
    }

    private async Task<MaintenanceAction> AddActionAsync(HttpClient client, AddActionCommand addActionCommand)
    {
        var response =  await client.PostAsync(Routes.AddAction, Serialize(addActionCommand));

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<MaintenanceAction>(responseContent);
    }


    private async Task<IEnumerable<MaintenanceAction>> GetActionsAsync(HttpClient client, string vehicleId, string actionTemplateId)
    {
        var response = await client.GetAsync($"{Routes.ActionsQuery}/?vehicleId={vehicleId}&actionTemplateId={actionTemplateId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<MaintenanceAction>>(responseContent);
    }
    
    private async Task<IEnumerable<MaintenanceAction>> GetActionsOfVehicleAsync(HttpClient client, string vehicleId)
    {
        var response = await client.GetAsync($"{Routes.ActionsOfVehicleQuery}/?vehicleId={vehicleId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<MaintenanceAction>>(responseContent);
    }

    private async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(HttpClient client)
    {
        var response = await client.GetAsync(Routes.VehiclesQuery);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<VehicleDto>>(responseContent);
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

