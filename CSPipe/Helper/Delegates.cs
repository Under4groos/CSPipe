
using CSPipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPipe.Helper
{
    public class Delegates
    {
        public delegate void svMessageReceivedHandler(svPipeClient client, string message);
        public delegate void clMessageReceivedHandler(string message);
    }
}
