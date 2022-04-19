using System.Collections.Generic;
using System.Drawing;

namespace RglGame
{
    public class Room
    {
        public static Size size = new Size(1100, 620);
        public Entity[] Entities;
        public HashSet<Door> doors;
        public int X;
        public int Y;
        public Room(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Room ChangeRoom(Door door)
        {
            return World.Rooms[door.To];
        }
        public class Entity
        {
        }
    };
}
