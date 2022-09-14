using System;
using System.Net.Http;
using System.Net.Http.Headers;
using maintenance_buddy_api_integration_test;
using Microsoft.Data.Sqlite;
using Xunit.Sdk;

namespace budget_backend_integration_tests.backend;

/// <summary>
///     Create a configured backend and offer a client for interaction.
/// </summary>
public class IntegrationTest : IDisposable
{
    private SqliteConnection _connection;

    public IntegrationTest()
    {
        StartSqliteConnection();

        if (_connection is null)
            throw new NullException(_connection);
        
        var appFactory = new SqLiteWebApplicationFactory<Program>(_connection);
        client = appFactory.CreateClient();
        
        // add the bearer token
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TestTokenIssuer.GenerateBearerToken());
    }

    public HttpClient client { get; set; }

    public void Dispose()
    {
        // Sqlite in-memory data is cleared with connection dispose.
        _connection.Dispose();
    }

    private void StartSqliteConnection()
    {
        // Open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
    }
}