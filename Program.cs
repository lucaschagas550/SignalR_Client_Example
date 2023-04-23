using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

var uri = new Uri("https://localhost:7141/connection");
var userId = "";

//Console.Write("Digite seu nome: ");
//var user = Console.ReadLine();

var connection = new HubConnectionBuilder()
    .WithUrl(uri.AbsoluteUri)
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (sender, message) =>
{
    Console.WriteLine($"{sender} => {message} \n");
});

connection.On<Person>("ReceiveObject", (message) =>
{
    Console.WriteLine($"Date: {DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss.fff")}");
    Debug.WriteLine($"Date: {DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss.fff")}");

    Console.WriteLine($"{message.Name}, {message.Age} \n");
    Debug.WriteLine($"{message.Name}, {message.Age}");
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