using Microsoft.Win32.SafeHandles;
using CSPipe.Helper;
using CSPipe.Models;
using System.Text;
using static CSPipe.Helper.Delegates;

namespace CSPipe
{
    public class PipeServer : BasePipe, IDisposable
    {

        public event svMessageReceivedHandler MessageReceived;

        protected const uint DUPLEX = (0x00000003);
        protected const uint FILE_FLAG_OVERLAPPED = (0x40000000);
       
        
        protected Task listenTask;
        protected bool running;
        protected List<svPipeClient> clients = new List<svPipeClient>();

        public bool Running
        {
            get { return this.running; }
        }

        public void Start()
        {

            this.listenTask = new Task(_ActionTask);
            this.listenTask.Start();

            this.running = true;
        }


        private void _ActionTask()
        {
            Console.WriteLine("Server startup");
            Console.WriteLine($"Name: {this.PipeName}");
            while (true)
            {
                SafeFileHandle clientHandle =
                Native.CreateNamedPipe(
                     this.pipeName,
                     DUPLEX | FILE_FLAG_OVERLAPPED,
                     0,
                     255,
                     BUFFER_SIZE,
                     BUFFER_SIZE,
                     0,
                     IntPtr.Zero);
                if (clientHandle.IsInvalid)
                    return;
                int success = Native.ConnectNamedPipe(clientHandle, IntPtr.Zero);
                if (success == 0)
                    return;
                svPipeClient client = new svPipeClient();
                client.handle = clientHandle;
                Console.WriteLine($"Connected: {clientHandle.DangerousGetHandle()}");



                lock (clients)
                    this.clients.Add(client);

                Thread readThread = new Thread(new ParameterizedThreadStart(Read));
                readThread.Start(client);
            }
        }


        private void Read(object clientObj)
        {
            svPipeClient client = (svPipeClient)clientObj;
            client.stream = new FileStream(client.handle, FileAccess.ReadWrite, BUFFER_SIZE, true);
            byte[] buffer = new byte[BUFFER_SIZE];
            ASCIIEncoding encoder = new ASCIIEncoding();

            while (true)
            {
                int bytesRead = 0;

                try
                {
                    bytesRead = client.stream.Read(buffer, 0, BUFFER_SIZE);
                }
                catch
                {
                    break;
                }
                if (bytesRead == 0)
                    break;
                if (this.MessageReceived != null)
                    this.MessageReceived(client, encoder.GetString(buffer, 0, bytesRead));
            }
            client.stream.Close();
            client.handle.Close();
            lock (this.clients)
                this.clients.Remove(client);
        }


        public void SendData(svPipeClient client , string message)
        {
            lock (this.clients)
            {
                UTF8Encoding encoder = new UTF8Encoding();
                byte[] messageBuffer = encoder.GetBytes(message);   
                client.stream.Write(messageBuffer, 0, messageBuffer.Length);
                client.stream.Flush();
            }

          
        }



        public void Dispose()
        {
            this.listenTask.Dispose();
        }
    }
}
