using MGStudio.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.BaseObjects
{
    public class BaseGameObject : ProjectItem
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
