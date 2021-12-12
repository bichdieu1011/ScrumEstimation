using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ScrumEstimationServerApp
{
    public class NotifyHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> _connectionGroup = new ConcurrentDictionary<string, List<string>>();

        public async Task Broadcast(string id, string name, string data)
        {
            await Clients.All.SendAsync("Broadcast", id, name, data);
        }

        public async Task SendToRoom(string roomId, string method, object data)
        {
            var connectionId = _connectionGroup[roomId];
            await Clients.Clients(connectionId).SendAsync(method, data);
        }


        public async Task Connected(string roomId, string userName)
        {
            try
            {
                var connection = Context.ConnectionId;

                if (!_connectionGroup.ContainsKey(roomId))
                {
                    userName = userName.ToLower();

                    _connectionGroup.TryAdd(roomId, new List<string> { connection });
                    await Groups.AddToGroupAsync(Context.ConnectionId, userName);
                }
                else
                {
                    var room = _connectionGroup[roomId];
                    if (!room.Contains(connection))
                    {
                        room.Add(connection);
                        _connectionGroup[roomId] = room;
                    }
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
