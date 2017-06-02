using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.RunTime
{
    public class Room
    {
        public int TileWidth { get; set; } = 32;
        public int TileHeight { get; set; } = 32;

        public Entity[,] ActivatedEntities;

        public List<Entity> MovedObjects = new List<Entity>();
        public List<Entity> VisibleObjects = new List<Entity>();

        public virtual IEnumerable<Entity> GetEntity(float x, float y)
        {
            
        }

        public Entity CreateEntity(Type type, int x, int y)
        {

        }
    }
}
