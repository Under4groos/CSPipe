# CSPipe

Используется для простого обмена сообщениями между приложениями-клиентами. Она позволяет приложениям взаимодействовать друг с другом, передавая данные в виде сообщений. 
При желании можно передавать Объекты в виде Json.

[GitHub CPipe](https://github.com/Under4groos/CPipe)

## Server
```C#
using (PipeServer server = new PipeServer()
{
    PipeName = "my_pipe_server"
})
{
    server.MessageReceived += (sender, e) =>
    {
        Console.WriteLine($"[{sender.handle.DangerousGetHandle()}]: {e}");
        server.SendData(sender ,"ok");
    };
    server.Start();

    Console.ReadKey();
}
```
## Client
```C#
using (PipeClient client = new PipeClient()
{
    PipeName = "my_pipe_server"
})
{
    client.MessageReceived += (string msg) =>
    {
        Console.WriteLine($"[SV->CL]: {msg}");
    };
    client.Connect();

    while (true)
    {
        if (client.Connected)
        {
            client.SendData(Console.ReadLine());
            continue;
        }
        Task.Delay(1000).Wait();
    }
}
```