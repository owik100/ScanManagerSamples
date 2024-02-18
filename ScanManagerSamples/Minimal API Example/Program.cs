using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();
app.UseStaticFiles();

app.MapHub<ScanResultHub>("/ScanResut");

app.MapGet("/", () =>  Results.Ok());

app.MapPost("/POSTTEST", async ([FromBody] string scan, IHubContext<ScanResultHub> context) =>
{
    await context.Clients.All.SendAsync("ReceiveMessage", scan);
    return Results.Created();
});

app.Run();
