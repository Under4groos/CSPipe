



using CSPipe;


using (PipeClient client = new PipeClient()
{
    PipeName = "my_pipe_server"
})
{
    client.MessageReceived += (string msg) =>
    {
        msg = msg.Trim().Replace("\0" , "");
        if (msg == "dbg" || msg == "ret" || msg == "-")
            return;
        Console.WriteLine($"[SV->CL]: {msg}");
    };
    client.Connect();
    Thread.Sleep(1000);
    while (true)
    {
        if (client.Connected)
        {
            client.SendMessage("dbg");
            Thread.Sleep(100);
            continue;
        }
        else
        {
            client.Connect();
        }
         
    }
}