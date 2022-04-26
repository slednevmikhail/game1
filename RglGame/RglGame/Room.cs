using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RglGame
{
    public class Room
    {
        public List<(Rectangle , Room)> Doors = new List<(Rectangle, Room)>();
        public Rectangle bounds = new Rectangle(-550, -310, 1100, 620);
        public void CheckCollision()
        {
            foreach (var door in Doors)
                if (door.Item1.Contains(Player.position))
                    Player.ChangeRoom(door.Item2);
        }
        public int coord;
        public Room(int pos)
        {
            coord = pos;
        }
        public HashSet<int> adjacentRooms = new HashSet<int>();
        public void Connect(int RoomIndex)
        {
            Doors.Add((new Rectangle(bounds.X * ((RoomIndex - coord) % 10) -30, bounds.Y * ((RoomIndex - coord) / 10) - 30, 60, 60) ,
                World.Rooms.First(room => room.coord == RoomIndex)));
        }
    };
}
