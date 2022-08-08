using System;
using System.Linq;
using FluentAssertions;
using maintenance_buddy_api.domain;
using Xunit;

namespace maintenance_buddy_api_unit_tests;

public class VehicleAggregateTests
{
    [Fact]
    public void ActionTemplatesAreAddedToVehicle()
    {
        // arrange
        var vehicle = VehicleFactory.Create("BMW R1100S", 39000);
        
        // act
        vehicle.AddActionTemplate("Ölwechsel", 5000, TimeSpan.FromDays(365));
        
        // assert
        var actionTemplates = vehicle.GetActionTemplates();
        actionTemplates.Should().Contain(_ => _.Name.Equals("Ölwechsel"));
    }
    
    [Fact]
    public void GetActionTemplateByNameReturnValidActionTemplate()
    {
        // arrange
        const string actionTemplateName = "Ölwechsel";
        var vehicle = VehicleFactory.Create("BMW R1100S", 39000);
        vehicle.AddActionTemplate(actionTemplateName, 5000, TimeSpan.FromDays(365));
        var actionTemplate = vehicle.GetActionTemplate(actionTemplateName);
        
        // assert
        actionTemplate.Id.Should().NotBeEmpty();
        actionTemplate.Name.Should().Be(actionTemplateName);
    }
    
    [Fact]
    public void ValidActionIsCreated()
    {
        // arrange
        var vehicle = VehicleFactory.Create("BMW R1100S", 39000);
        vehicle.AddActionTemplate("Ölwechsel", 5000, TimeSpan.FromDays(365));
        var oilTemplate = vehicle.GetActionTemplate("Ölwechsel");
        oilTemplate.AddAction(oilTemplate.Id, 39000, new DateTime(2022,8,8));
        
        // assert
        var actions = oilTemplate.GetActions();
        actions.Should().Contain(_ => _.Id != Guid.Empty);
        actions.Should().Contain(_ => _.Kilometer == 39000);
    }
}