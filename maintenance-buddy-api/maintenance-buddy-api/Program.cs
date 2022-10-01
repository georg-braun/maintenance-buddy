using maintenance_buddy_api;
using maintenance_buddy_api.api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddBearerAuthentication();
builder.RequireAuthenticatedUsers();

builder.Services.AddCors();
builder.Services.AddControllers();


builder.Services.AddDbContext<VehicleContext>(op =>
{
    op.UseNpgsql(builder.Configuration["DbConnectionString"]);
    op.LogTo(Console.Write);
});
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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapGet(Routes.Status, () => "Ok").AllowAnonymous();

// vehicle
app.MapPost(Routes.CreateVehicle, VehicleEndpoint.CreateVehicle);
app.MapGet(Routes.VehiclesQuery, VehicleEndpoint.VehiclesQuery);
app.MapGet(Routes.DeleteVehicle, VehicleEndpoint.DeleteVehicle);
app.MapPost(Routes.RenameVehicle, VehicleEndpoint.RenameVehicle);
app.MapPost(Routes.ChangeVehicleKilometer, VehicleEndpoint.ChangeVehicleKilometer);

// action template
app.MapPost(Routes.AddActionTemplate, VehicleEndpoint.AddActionTemplate);
app.MapPost(Routes.DeleteActionTemplate, VehicleEndpoint.DeleteActionTemplate);
app.MapGet(Routes.ActionTemplateQuery, VehicleEndpoint.ActionTemplatesQuery);
app.MapPost(Routes.ChangeActionTemplateName, VehicleEndpoint.ChangeActionTemplateName);
app.MapPost(Routes.ChangeActionTemplateKilometerInterval, VehicleEndpoint.ChangeActionTemplateKilometerInterval);
app.MapPost(Routes.ChangeActionTemplateTimeInterval, VehicleEndpoint.ChangeActionTemplateTimeInterval);

// maintenance action
app.MapPost(Routes.AddAction, VehicleEndpoint.AddAction);
app.MapPost(Routes.DeleteAction, VehicleEndpoint.DeleteAction);
app.MapPost(Routes.ChangeActionKilometer, VehicleEndpoint.ChangeActionKilometer);
app.MapPost(Routes.ChangeActionNote, VehicleEndpoint.ChangeActionNote);
app.MapPost(Routes.ChangeActionDate, VehicleEndpoint.ChangeActionDate);
app.MapGet(Routes.ActionsQuery, VehicleEndpoint.ActionsQuery);
app.MapGet(Routes.ActionsOfVehicleQuery, VehicleEndpoint.ActionsOfVehicleQuery);

// pending actions
app.MapGet(Routes.VehiclePendingActions, VehicleEndpoint.VehiclePendingActionsQuery);

app.Run();

// add class to get an anchor for the integration tests.
public partial class Program {}