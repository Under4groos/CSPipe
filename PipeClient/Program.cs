



using CSPipe;


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
            client.SendMessage(Console.ReadLine());
            continue;
        }
        Task.Delay(1000).Wait();
    }
}