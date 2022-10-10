using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using FluentAssertions;
using maintenance_buddy_api.api;
using maintenance_buddy_api.api.commands;
using maintenance_buddy_api.api.dto;
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
    public async Task DeleteVehicle()
    {
        // arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // act
        await DeleteVehicleAsync(client, vehicle.Id);

        // assert
        var vehicles = await GetVehiclesAsync(client);
        vehicles.Should().HaveCount(0);
    }

    [Fact]
    public async Task RenameVehicle()
    {
        // arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // act
        await RenameVehicleAsync(client, new RenameVehicleCommand(vehicle.Id, "Opel Astra"));

        // assert
        var vehicles = await GetVehiclesAsync(client);
        vehicles.First().Name.Should().Be("Opel Astra");
    }
    
    [Fact]
    public async Task RenameVehicleWithInvalidVehicleIdReturnsBadRequest()
    {
        // arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // act
        var response = await RenameVehicleAsync(client, new RenameVehicleCommand(string.Empty, "Opel Astra"));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ChangeVehicleKilometer()
    {
        // arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        
        // act
        await ChangeVehicleKilometerAsync(client, new ChangeVehicleKilometerCommand(vehicle.Id, 40000));

        // assert
        var vehicles = await GetVehiclesAsync(client);
        vehicles.First().Kilometer.Should().Be(40000);
    }
    
    [Fact]
    public async Task ChangeActionProperties()
    {
        /// Arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var oilActionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));

        // act
        var addOilActionCommand = new AddActionCommand(vehicle.Id, oilActionTemplate.Id, new DateTime(2022,9,3), 2000, "5W50");
        var action = await AddActionAsync(client, addOilActionCommand);
        await client.PostAsync(Routes.ChangeActionKilometer, Serialize(new ChangeActionKilometerCommand(vehicle.Id, oilActionTemplate.Id, action.Id, 4000)));
        await client.PostAsync(Routes.ChangeActionDate, Serialize(new ChangeActionDateCommand(vehicle.Id, oilActionTemplate.Id, action.Id, new DateTime(2022,10,3))));
        await client.PostAsync(Routes.ChangeActionNote, Serialize(new ChangeActionNoteCommand(vehicle.Id, oilActionTemplate.Id, action.Id, "10W40")));
        
        // Assert
        var actions = await GetActionsOfVehicleAsync(client, vehicle.Id);
        actions.Should().HaveCount(1);
        actions.First().Kilometer.Should().Be(4000);
        actions.First().Date.Should().Be(new DateTime(2022,10,3));
        actions.First().Note.Should().Be("10W40");
    }
    
    [Fact]
    public async Task ChangeActionTemplateProperties()
    {
        /// Arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 39000));
        var oilActionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365,0,0,0)));

        // act
        await client.PostAsync(Routes.ChangeActionTemplateName, Serialize(new ChangeActionTemplateNameCommand(vehicle.Id, oilActionTemplate.Id, "Tires")));
        await client.PostAsync(Routes.ChangeActionTemplateKilometerInterval, Serialize(new ChangeActionTemplateKilometerIntervalCommand(vehicle.Id, oilActionTemplate.Id, 1000)));
        await client.PostAsync(Routes.ChangeActionTemplateTimeInterval, Serialize(new ChangeActionTemplateTimeIntervalCommand(vehicle.Id, oilActionTemplate.Id, new TimeSpan(300,0,0,0))));
        
        // Assert
        var actionTemplate = await GetActionTemplatesAsync(client, vehicle.Id);
        actionTemplate.Should().HaveCount(1);
        actionTemplate.First().Name.Should().Be("Tires");
        actionTemplate.First().TimeInterval.Should().Be(new TimeSpan(300,0,0,0));
        actionTemplate.First().KilometerInterval.Should().Be(1000);
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
        var actionDate = new DateTime(2022,9,3).ToUniversalTime();
        var addActionCommand = new AddActionCommand(vehicle.Id, actionTemplate.Id, actionDate, 2000, "5W50");
        await client.PostAsync(Routes.AddAction, Serialize(addActionCommand));
        
        
        var actions = await GetActionsAsync(client, vehicle.Id, actionTemplate.Id);
        
        // Assert
        actions.First().Date.Should().Be(actionDate);
        actions.First().Note.Should().Be("5W50");
        actions.First().Kilometer.Should().Be(2000);
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
        await DeleteAction(client, vehicle.Id, actionTemplate.Id, action.Id);

        // Assert
        var actions = await GetActionsAsync(client, vehicle.Id, actionTemplate.Id);
        actions.Should().NotContain(_ => _.Date.Equals(actionDate));
    }
    
    [Fact]
    public async Task Vehicle_GetPendingActions()
    {
        // Arrange
        var client = new IntegrationTest().GetClient();
        var vehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("BMW R1100S", 20000));
        var actionTemplate = await AddActionTemplateAsync(client, new AddActionTemplateCommand(vehicle.Id, "Oil change", 5000, new TimeSpan(365)));
        
        var actionDate = DateTime.Today.ToUniversalTime();
        var action = await AddActionAsync(client, new AddActionCommand(vehicle.Id, actionTemplate.Id, actionDate, 12000, "5W50"));
        
        // act
        var pendingActions = await GetPendingActionsAsync(client, vehicle.Id);

        // Assert
        pendingActions.Should().HaveCount(1);
        pendingActions.First().KilometerTillAction.Should().Be(-3000);
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
    public async Task GetVehicle()
    {
        // arrange
        var client = new IntegrationTest().GetClient();
        var createdVehicle = await CreateVehicleAsync(client, new CreateVehicleCommand("Opel Astra", 60000));

        // act
        var vehicle = await GetVehicleAsync(client, createdVehicle.Id);
        
        // assert
        vehicle.Name.Should().Be("Opel Astra");
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

    private async Task<MaintenanceActionDto> AddActionAsync(HttpClient client, AddActionCommand addActionCommand)
    {
        var response =  await client.PostAsync(Routes.AddAction, Serialize(addActionCommand));

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<MaintenanceActionDto>(responseContent);
    }


    private async Task<IEnumerable<MaintenanceActionDto>> GetActionsAsync(HttpClient client, string vehicleId, string actionTemplateId)
    {
        var response = await client.GetAsync($"{Routes.ActionsByTemplate}/?vehicleId={vehicleId}&actionTemplateId={actionTemplateId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<MaintenanceActionDto>>(responseContent);
    }
    
    private async Task<IEnumerable<MaintenanceActionDto>> GetActionsOfVehicleAsync(HttpClient client, string vehicleId)
    {
        var response = await client.GetAsync($"{Routes.ActionsByVehicle}/?vehicleId={vehicleId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<MaintenanceActionDto>>(responseContent);
    }
    
    private async Task<IEnumerable<PendingAction>> GetPendingActionsAsync(HttpClient client, string vehicleId)
    {
        var response = await client.GetAsync($"{Routes.VehiclePendingActions}/?vehicleId={vehicleId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<PendingAction>>(responseContent);
    }

    private async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(HttpClient client)
    {
        var response = await client.GetAsync(Routes.VehiclesQuery);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<VehicleDto>>(responseContent);
    }
    
    private async Task<VehicleDto> GetVehicleAsync(HttpClient client, string vehicleId)
    {
        var response = await client.GetAsync($"{Routes.GetVehicle}/?vehicleId={vehicleId}");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<VehicleDto>(responseContent);
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
    
    private async Task<HttpResponseMessage> DeleteVehicleAsync(HttpClient client, string vehicleId)
    {
        return await client.GetAsync($"{Routes.DeleteVehicle}/?vehicleId={vehicleId}");
    }

    private async Task<HttpResponseMessage> RenameVehicleAsync(HttpClient client, RenameVehicleCommand command)
    {
        return await client.PostAsync(Routes.RenameVehicle, Serialize(command));
    }
    
    
    private async Task<HttpResponseMessage> ChangeVehicleKilometerAsync(HttpClient client, ChangeVehicleKilometerCommand command)
    {
        return await client.PostAsync(Routes.ChangeVehicleKilometer, Serialize(command));
    }
    
    
    private StringContent Serialize(object command)
    {
        var json = JsonConvert.SerializeObject(command);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}

record VehicleDto(string Id, string Name, int Kilometer);



