using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Common.Constants;
using Common.Records;
using Microsoft.Extensions.Azure;

namespace API;

public static class ApiEndpoints
{
    public static void MapSendRequest(this WebApplication app)
    {
        app.MapPost("/add-match/", async (GameMatchup gameMatchup, IAzureClientFactory<ServiceBusClient> azureClientFactory) =>
        {
            try
            {
                await SendMessageAsync(gameMatchup, azureClientFactory);
            }
            catch (Exception ex)    
            {
                return Results.BadRequest(ex.Message);
            }

            return Results.Ok();
        });
    }
    
    private static async Task SendMessageAsync(GameMatchup gameMatchup, IAzureClientFactory<ServiceBusClient> azureClientFactory)
    {
        var gameMatchupAsJson = JsonSerializer.Serialize(gameMatchup);
        
        var serviceBusSender = azureClientFactory
            .CreateClient(QueueNames.ODDS_COLLECTOR_QUEUE)
            .CreateSender(QueueNames.ODDS_COLLECTOR_QUEUE);
        
        var serviceBusMessage = new ServiceBusMessage(gameMatchupAsJson);
        await serviceBusSender.SendMessageAsync(serviceBusMessage);
    }
}

