using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.RunTime
{
    public class EntityService
    {        
        public Queue<Entity> Items = new Queue<Entity>();

        public void Add(Entity entity)
        {
            if (entity == null)
                return;
            if (!Items.Contains(entity))
                Items.Enqueue(entity);            
        }

        public Entity Remove()
        {
            var entity = Items.Dequeue();
            if (entity == null)
                return null;

            entity.MovedToService = false;

            return entity;
        }

        public void Clear()
        {
            Items = new Queue<Entity>();
        }
    }
}
