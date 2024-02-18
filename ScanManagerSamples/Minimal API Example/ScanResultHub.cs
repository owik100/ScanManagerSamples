using Microsoft.AspNetCore.SignalR;

public class ScanResultHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}