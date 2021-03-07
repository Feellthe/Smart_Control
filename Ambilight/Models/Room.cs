using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight.Models
{
    public class Room
    {
        public string name { get; set; }
        public string location { get; set; }
        public byte[] image { get; set; }

        public List<Connected_Device> connected_devices = new List<Connected_Device>();
        
    }
}
