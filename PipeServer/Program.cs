using CSPipe;

using (PipeServer server = new PipeServer()
{
    PipeName = "my_pipe_server"
})
{
    server.MessageReceived += (sender, e) =>
    {
        Console.WriteLine($"[{sender.handle.DangerousGetHandle()}]: {e}");
    };
    server.Start();

    Console.ReadKey();
}