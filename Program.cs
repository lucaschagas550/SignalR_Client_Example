using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

var uri = new Uri("http://localhost:5090/connection");
var userId = "";

//Console.Write("Digite seu nome: ");
//var user = Console.ReadLine();

var connection = new HubConnectionBuilder()
    .WithUrl(uri.AbsoluteUri)
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (sender, message) =>
{
    Console.WriteLine($"{sender} => {message}");
});

connection.On<Person>("ReceiveObject", (message) =>
{
    Console.WriteLine($"{message.Name}, {message.Age}, {message.Date} ");
    Debug.WriteLine($"{message.Name}, {message.Age}, {message.Date} ");
});

connection.On<string>("Connected", (Id) =>
{
    Console.WriteLine($"UserId => {Id}");
    userId = Id;
});

await connection.StartAsync();

Console.WriteLine("Digite sua mensagem para uma unica conexao: ");
var uniqueMessage = Console.ReadLine();
await connection.InvokeAsync("SendMessageToClient", userId, uniqueMessage).ConfigureAwait(false);

while (true)
{
    Console.Write("Digite sua mensagem: ");
    var message = Console.ReadLine();

    await connection.InvokeAsync("SendMessage", userId, message).ConfigureAwait(false);
}