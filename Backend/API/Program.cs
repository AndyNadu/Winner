using API;
using Common.Configurations;
using Common.Constants;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

builder.Host
    .ConfigureKeyVault();

builder.Services
    .AddAzureClients(configureClients =>
    {
        var configuration = builder.Configuration[KeyVaultSecretNames.API_SEND_SERVICE_BUS_CONNECTION]
                            ?? throw new Exception(ErrorStrings.MISSING_SERVICE_BUS_CONNECTION_STRING);
        configureClients
            .AddServiceBusClient(configuration)
            .WithName(QueueNames.ODDS_COLLECTOR_QUEUE);
    });

var app = builder.Build();
app.UseHttpsRedirection();


app.MapSendRequest();

await app.RunAsync();