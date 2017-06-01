using MGStudio.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.BaseObjects
{
    public class BaseSprite : ProjectItem
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int OriginX { get; set; }
        public int OriginY { get; set; }

        public bool PreciseCollisionChecking { get; set; }
        public bool SeperateCollisionMasks { get; set; }
    }
}
