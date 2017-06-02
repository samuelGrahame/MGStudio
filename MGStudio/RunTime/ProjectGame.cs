using System.Collections.Generic;

namespace MGStudio.RunTime
{
    public class ProjectGame
    {
        public List<Room> Rooms = new List<Room>();
        public Room ActiveRoom = null;

        public void MoveToRoom(Room room)
        {
            ActiveRoom = room;
        }
    }
}
