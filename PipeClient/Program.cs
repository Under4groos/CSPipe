



using CSPipe;


using (PipeClient client = new PipeClient()
{
    PipeName = "my_pipe_server"
})
{
    client.MessageReceived += (string msg) =>
    {
 
        if(msg.StartsWith("\\??\\") && (msg = msg.Substring("\\??\\".Length)).Length > 0)
        {
            Console.WriteLine($"{msg}");
        }

       
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