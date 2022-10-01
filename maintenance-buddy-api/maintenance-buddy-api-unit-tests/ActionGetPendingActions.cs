using System;
using FluentAssertions;
using maintenance_buddy_api.domain;
using Xunit;

namespace maintenance_buddy_api_unit_tests;

public class ActionGetPendingActions
{
    [Fact]
    public void GetPendingAction_WhenThereAreNoPreviousActions()
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
    public void ActionIsPending_KilometerExceeded()
    {
        // arrange
        var actionTemplate = ActionTemplate.Create(Guid.NewGuid(), "oil exchange", 5000, TimeSpan.FromDays(20));
        actionTemplate.AddAction(7000, new DateTime(2022, 10, 1), "");

        // act
        var forDate = new DateTime(2022, 10, 10);
        var vehicleKilometer = 20000;
        var pendingAction = actionTemplate.GetPendingAction(forDate, vehicleKilometer);

        // assert
        pendingAction.KilometerTillAction.Should().Be(-8000);
        pendingAction.TimeTillAction.Should().Be(TimeSpan.FromDays(11));
        pendingAction.ActionTemplateId.Should().Be(actionTemplate.Id);
        pendingAction.Exceeded.Should().BeFalse();
    }
    
    [Fact]
    public void ActionIsPending_TimeExceeded()
    {
        // arrange
        var actionTemplate = ActionTemplate.Create(Guid.NewGuid(), "oil exchange", 5000, TimeSpan.FromDays(5));
        actionTemplate.AddAction(6000, new DateTime(2022, 10, 10), "");

        // act
        var forDate = new DateTime(2022, 10, 20);
        var vehicleKilometer = 7000;
        var pendingAction = actionTemplate.GetPendingAction(forDate, vehicleKilometer);

        // assert
        pendingAction.KilometerTillAction.Should().Be(4000);
        pendingAction.TimeTillAction.Should().Be(TimeSpan.FromDays(-5));
        pendingAction.ActionTemplateId.Should().Be(actionTemplate.Id);
        pendingAction.Exceeded.Should().BeTrue();
    }
    
    [Fact]
    public void ActionIsPending_NothingExceeded()
    {
        // arrange
        var actionTemplate = ActionTemplate.Create(Guid.NewGuid(), "oil exchange", 5000, TimeSpan.FromDays(5));
        actionTemplate.AddAction(6000, new DateTime(2022, 10, 10), "");

        // act
        var forDate = new DateTime(2022, 10, 12);
        var vehicleKilometer = 7000;
        var pendingAction = actionTemplate.GetPendingAction(forDate, vehicleKilometer);

        // assert
        pendingAction.KilometerTillAction.Should().Be(4000);
        pendingAction.TimeTillAction.Should().Be(TimeSpan.FromDays(3));
        pendingAction.ActionTemplateId.Should().Be(actionTemplate.Id);
        pendingAction.Exceeded.Should().BeFalse();

    }
}