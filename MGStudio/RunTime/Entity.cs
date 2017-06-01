using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.RunTime
{
    public class Entity
    {
        public bool Visible { get; set; } = true;
        public bool Solid { get; set; } = false;
        public bool Persistent { get; set; } = false;
        public int Depth { get; set; } = 0;

        public float X { get; set; }
        public float Y { get; set; }
        public float Rotation { get; set; }
    }
}
