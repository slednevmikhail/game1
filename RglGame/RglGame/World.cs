using RglGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Graph;

namespace RglGame
{
    public static class World
    {
        public static List<Room> Rooms = new List<Room> { new Room(0) };
        public static void GenerateRooms(int RoomsCount)
        {
            var rnd = new Random();
            for (int i = 0; i < RoomsCount; i++)
            {
                var vacantRooms = new HashSet<int>();
                foreach (var currentRoom in Rooms)
                {
                    foreach (var d in new int[] { 1, -1, 10, -10 })
                    {
                        if (!Rooms.Select(room => room.coord).Contains(currentRoom.coord + d))
                            vacantRooms.Add(currentRoom.coord + d);
                    }
                }
                Rooms.Add(new Room(vacantRooms.ElementAt(rnd.Next(vacantRooms.Count))));
            }
        }
        public static void GenerateMap()
        {
            GenerateRooms(8);
            ConnectRooms();
            Player.CurrentRoom = Rooms[0];
        }
        public static void ConnectRooms()
        {
            foreach (var currentRoom in Rooms)
                foreach (var d in new int[] { 1, -1, 10, -10 })
                {
                    if (Rooms.Select(room => room.coord).Contains(currentRoom.coord + d))
                        currentRoom.Connect(currentRoom.coord + d);
                }
        }
    };

}
