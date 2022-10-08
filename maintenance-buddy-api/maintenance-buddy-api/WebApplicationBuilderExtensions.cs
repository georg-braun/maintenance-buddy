using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace maintenance_buddy_api;

public static class WebApplicationBuilderExtensions
{

    public static WebApplicationBuilder AddBearerAuthentication(this WebApplicationBuilder builder)
    {
        var authority = builder.Configuration["AuthProvider:Authority"];
        var audience = builder.Configuration["AuthProvider:Audience"];
    
        if (string.IsNullOrEmpty(authority) || string.IsNullOrEmpty(audience))
        {
            Console.WriteLine("At least one authentication parameter is empty!");    
        }
        
        Console.WriteLine($"Domain: {authority}");
        Console.WriteLine($"Audience: {audience}");


        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
            {
                jwtOptions.Authority = authority;
                jwtOptions.Audience = audience;
                jwtOptions.RequireHttpsMetadata = false;
            });

        return builder;
    }

    public static WebApplicationBuilder RequireAuthenticatedUsers(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(o =>
        {
            o.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return builder;
    }
}