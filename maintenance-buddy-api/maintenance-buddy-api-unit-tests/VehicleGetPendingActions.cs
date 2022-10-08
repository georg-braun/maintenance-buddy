using System;
using System.Linq;
using FluentAssertions;
using maintenance_buddy_api.domain;
using Xunit;

namespace maintenance_buddy_api_unit_tests;

public class VehiclePendingActions
{
    [Fact]
    public void GetPendingActions_WhenThereAreNoPreviousActions()
    {
        // arrange
        var checkDate = new DateTime(2022, 10, 1);
        var vehicle = VehicleFactory.Create("BMW R1100S", 39000);
        var actionTemplate = vehicle.AddActionTemplate("oil exhange", 5000, TimeSpan.FromDays(365));
        
        // act
        var pendingActions = vehicle.GetPendingActions(checkDate);
        
        // assert
        pendingActions.Should().HaveCount(1);
    }
    
    
    [Fact]
    public void GetPendingActions_WithPreviousActions()
    {
        // arrange
        var checkDate = new DateTime(2022, 10, 10);
        var vehicle = VehicleFactory.Create("BMW R1100S", 40000);
        var actionTemplate = vehicle.AddActionTemplate("oil exhange", 5000, TimeSpan.FromDays(365));
        actionTemplate.AddAction(30000, new DateTime(2022, 9, 1), "");
        actionTemplate.AddAction(39000, new DateTime(2022, 10, 1), "");
        
        // act
        var pendingActions = vehicle.GetPendingActions(checkDate);
        
        // assert
        pendingActions.Should().HaveCount(1);
        pendingActions.First().KilometerTillAction.Should().Be(4000);
        pendingActions.First().TimeTillAction.Should().Be(TimeSpan.FromDays(356));
    }

}