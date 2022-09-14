using maintenance_buddy_api;
using maintenance_buddy_api.api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddBearerAuthentication();
builder.RequireAuthenticatedUsers();

builder.Services.AddControllers();
builder.Services.AddDbContext<VehicleContext>(op => op.LogTo(Console.Write) );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.MapGet(Routes.Status, () => "Ok");
app.MapPost(Routes.CreateVehicle, VehicleEndpoint.CreateVehicle);
app.MapPost(Routes.AddActionTemplate, VehicleEndpoint.AddActionTemplate);
app.MapPost(Routes.DeleteActionTemplate, VehicleEndpoint.DeleteActionTemplate);
app.MapPost(Routes.AddAction, VehicleEndpoint.AddAction);
app.MapPost(Routes.DeleteAction, VehicleEndpoint.DeleteAction);

app.MapGet(Routes.ActionTemplateQuery, VehicleEndpoint.ActionTemplatesQuery);
app.MapGet(Routes.ActionsQuery, VehicleEndpoint.ActionsQuery);

app.Run();

// add class to get an anchor for the integration tests.
public partial class Program {}