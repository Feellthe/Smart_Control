using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight
{
    public enum mode { RGB, White, Scene };
    public class Couleur
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        private int Intensity = 50;
        private int White_Color = 4500;
        private int Scene_Speed = 50;
        
        
}
}
