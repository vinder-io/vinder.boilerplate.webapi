namespace Vinder.Sample.TestSuite.Fixtures;

public sealed class WebApplicationFixture : IAsyncLifetime
{
    private readonly MongoDatabaseFixture _databaseFixture;

    public HttpClient HttpClient { get; private set; } = default!;
    public IServiceProvider Services { get; private set; } = default!;

    private WebApplicationFactory<Program> _factory = default!;

    public WebApplicationFixture()
    {
        _databaseFixture = new MongoDatabaseFixture();
    }

    public async ValueTask InitializeAsync()
    {
        await _databaseFixture.InitializeAsync();

        Environment.SetEnvironmentVariable("Settings__Database__ConnectionString", _databaseFixture.ConnectionString);
        Environment.SetEnvironmentVariable("Settings__Database__DatabaseName", _databaseFixture.DatabaseName);

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoClient));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoDatabase));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    services.AddSingleton(_ => _databaseFixture.Client);
                    services.AddSingleton(_ => _databaseFixture.Database);
                });
            });

        HttpClient = _factory.CreateClient();
        Services = _factory.Services;
    }

    public async ValueTask DisposeAsync()
    {
        HttpClient.Dispose();

        await _factory.DisposeAsync();
        await _databaseFixture.CleanDatabaseAsync();
        await _databaseFixture.DisposeAsync();
    }

    public async ValueTask<HttpClient> AuthenticateAsync(string username, string password, string tenant)
    {
        var client = _factory.CreateClient();
        var request = new { Username = username, Password = password };

        var response = await client.PostAsJsonAsync("api/v1/identity/authenticate", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            throw new InvalidOperationException(error is null
                ? "Error in serialization"
                : $"{error.Description} - ({error.Code})");
        }

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        if (result is null)
            throw new InvalidOperationException("Error in serialization");

        var token = result.AccessToken;
        return client.WithAuthorization(token).WithTenantHeader(tenant);
    }

    public async ValueTask<HttpClient> AuthenticateClientAsync(string clientId, string clientSecret)
    {
        var client = _factory.CreateClient();
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret
        };

        var response = await client.PostAsync("api/v1/openid/connect/token", new FormUrlEncodedContent(parameters));
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            throw new InvalidOperationException(error is null
                ? "Error in serialization"
                : $"{error.Description} - ({error.Code})");
        }

        var result = await response.Content.ReadFromJsonAsync<ClientAuthenticationResult>();
        if (result is null)
            throw new InvalidOperationException("Error in serialization");

        var token = result.AccessToken;
        return client.WithAuthorization(token);
    }
}
