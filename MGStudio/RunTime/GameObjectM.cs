using MGStudio.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.RunTime
{
    public class GameObjectM : BaseGameObject
    {
        public SpriteM ActiveSprite { get; set; } = null;
        public SpriteM ActiveMask { get; set; } = null;
        public GameObjectM ParentGameObject { get; set; }
        public List<GameObjectEventsM> Events { get; set; } = new List<GameObjectEventsM>();

        Assembly objectType;
    }
}
