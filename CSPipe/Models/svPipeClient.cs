using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPipe.Models
{
    public struct svPipeClient
    {
        public SafeFileHandle handle;
        public FileStream stream;
    }
}
