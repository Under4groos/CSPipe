using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPipe
{
    public class BasePipe
    {
        protected const int BUFFER_SIZE = 4096;


        protected string pipeName;
        public string PipeName
        {
            get { return this.pipeName; }
            set { this.pipeName = "\\\\.\\pipe\\" + value; }
        }
    }
}
