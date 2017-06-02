using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace MGStudio.RunTime
{
    public class ProjectGame
    {
        public List<Room> Rooms = new List<Room>();
        public Room ActiveRoom = null;

        public Assembly ScriptAssembly;

        public void MoveToRoom(Room room)
        {
            ActiveRoom = room;
        }

        public ProjectGame(Assembly scriptAssembly)
        {
            ScriptAssembly = scriptAssembly;

            ActiveRoom = Rooms.FirstOrDefault();
        }
    }
}
