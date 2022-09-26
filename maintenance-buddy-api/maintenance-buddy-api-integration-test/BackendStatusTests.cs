using System.Threading.Tasks;
using budget_backend_integration_tests.backend;
using maintenance_buddy_api.api;
using Xunit;

namespace maintenance_buddy_api_integration_test;

public class BackendStatusTests
{
    [Fact]
    public async Task StatusEndpointsReturnsValidValue()
    {
        // arrange
        var client = new IntegrationTest().GetClient();

        // act
        var response = await client.GetAsync(Routes.Status);

        // assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Ok", content);
    }
}