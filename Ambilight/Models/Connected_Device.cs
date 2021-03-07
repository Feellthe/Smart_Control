using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight
{
    public class Connected_Device

    {
        public string mac { get; set; }
        public string name { get; set; }
        public byte[] Image { get; set; }
        public string ip { get; set; }
        public string type { get; set; }
        public int port { get; set; }
    }
}
