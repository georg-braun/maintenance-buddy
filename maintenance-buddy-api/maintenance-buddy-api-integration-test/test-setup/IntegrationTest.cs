using System;
using System.Collections.Generic;
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
    private readonly SqLiteWebApplicationFactory<Program> _appFactory;
    private Dictionary<string, string> TokenByUser = new();
    private HttpClient? _client;

    public IntegrationTest()
    {
        StartSqliteConnection();

        if (_connection is null)
            throw new NullException(_connection);
        
        _appFactory = new SqLiteWebApplicationFactory<Program>(_connection);
    }

    

    /// <summary>
    ///     Creates a new client.
    /// </summary>
    /// <returns></returns>
    public HttpClient GetClient()
    {
        _client ??= _appFactory.CreateClient();
        SetClientSession("default");
        return _client;
    }
    
    /// <summary>
    ///     Get the client and sets the corresponding token.
    ///     WARNING: The client session is shared! The session of preceding is modified
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public void SetClientSession(string user)
    {
        var token = string.Empty;
        if (!TokenByUser.TryGetValue(user, out token))
        {
            token = TestTokenIssuer.GenerateBearerToken();
           TokenByUser.Add(user, token); 
        }
        
        _client ??= _appFactory.CreateClient();

        // add the bearer token of the requested user
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
    }

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