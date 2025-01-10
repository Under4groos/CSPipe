using Microsoft.Win32.SafeHandles;
using CSPipe.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSPipe.Helper.Delegates;

namespace CSPipe
{
    public class PipeClient : BasePipe , IDisposable
    {
        protected const uint GENERIC_READ = (0x80000000);
        protected const uint GENERIC_WRITE = (0x40000000);
        protected const uint OPEN_EXISTING = 3;
        protected const uint FILE_FLAG_OVERLAPPED = (0x40000000);
        public event clMessageReceivedHandler MessageReceived;
        
        
        protected FileStream stream;
        protected SafeFileHandle handle;
        protected Task readThread;
        protected bool connected;


        public bool Connected
        {
            get { return this.connected; }
        }

      

       
        public void Connect()
        {
            this.handle =
               Native.CreateFile(
                  this.pipeName,
                  GENERIC_READ | GENERIC_WRITE,
                  0,
                  IntPtr.Zero,
                  OPEN_EXISTING,
                  FILE_FLAG_OVERLAPPED,
                  IntPtr.Zero);

            //could not create handle - server probably not running
            if (this.handle.IsInvalid)
                return;

            
            this.readThread = new Task(Read);
            this.readThread.Start();

            this.connected = true;
        }
        public void Read()
        {
            this.stream = new FileStream(this.handle, FileAccess.ReadWrite, BUFFER_SIZE, true);
            byte[] readBuffer = new byte[BUFFER_SIZE];
            ASCIIEncoding encoder = new ASCIIEncoding();
            while (true)
            {
                int bytesRead = 0;

                try
                {
                    bytesRead = this.stream.Read(readBuffer, 0, BUFFER_SIZE);
                }
                catch
                {
                    break;
                }
                if (bytesRead == 0)
                    continue;
                if (this.MessageReceived != null)
                    this.MessageReceived(encoder.GetString(readBuffer, 0, bytesRead));
            }
            this.stream.Close();
            this.handle.Close();
        }
        public void SendMessage(string message)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] messageBuffer = encoder.GetBytes(message);

            this.stream.Write(messageBuffer, 0, messageBuffer.Length);
            this.stream.Flush();
        }

        public void Dispose()
        {
             
        }
    }
}
