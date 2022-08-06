using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace maintenance_buddy_api_integration_test;

public class BackendStatusTests
{
    [Fact]
    public async Task StatusEndpointsReturnsValidValue()
    {
        // arrange
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        
        // act
        var response = await client.GetAsync("/status");

        // assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Ok", content);
    }
}