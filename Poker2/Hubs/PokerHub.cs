using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace Poker2.Hubs;

public interface ITableClient
{
    public Task ReceiveMessage(string userName, string message);
}

public class PokerHub : Hub<ITableClient>
{
    private readonly IDistributedCache _cache;
    public PokerHub(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task joinTable(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.tableId.ToString());

        var stringConnection = JsonSerializer.Serialize(connection);

        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

        await Clients
            .Groups(connection.tableId.ToString())
            .ReceiveMessage("User", $"{connection.userName} присоединился к столу");
    }

    public async Task SendCard(string card)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
        if (connection is not null)
        {
            await Clients
                .Groups(connection.tableId.ToString())
                .ReceiveMessage(connection.userName, card);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);
        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection is not null)
        {
            await _cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.tableId.ToString());
            
            await Clients
                .Groups(connection.tableId.ToString())
                .ReceiveMessage("User", $"{connection.userName} покинул стол");
            
        }
        await base.OnDisconnectedAsync(exception);
    }
}