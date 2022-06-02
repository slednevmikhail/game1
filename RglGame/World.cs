using RglGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RglGame
{
    public static class World
    {
        public static bool GameStarted;
        public static List<Room> Rooms = new List<Room> { new Room(0) };
        public static HashSet<int> ClearedRooms = new HashSet<int> { 0 , 1 };
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
            SetBossRoom();
            GenerateEntities(8);
            Player.CurrentRoom = Rooms[0];
            for (int i = 1; i < Rooms.Count; i++)
            {
                if (!Rooms[i].IsBoss)
                {
                    Rooms[i].SpawnEnemies();
                }
                else Rooms[i].SpawnBoss();
            }
        }
        public static void ConnectRooms()
        {
            foreach (var currentRoom in Rooms)
            {
                foreach (var d in new int[] { 1, -1, 10, -10 })
                {
                    if (Rooms.Select(room => room.coord).Contains(currentRoom.coord + d))
                        currentRoom.Connect(currentRoom.coord + d);
                }
            }
        }
        public static void GenerateEntities(int amount)
        {
            for (int i = 0; i <= amount; i++)
            {
                if (!Rooms[i].IsBoss)
                {
                    Rooms[i].Walls = RoomSamples.WallSamples[i];
                    Rooms[i].GetDoorRects();
                }
            }
        }
        public static void SetBossRoom()
        {
            int furtherRoomIndex= 0;
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Math.Abs(Rooms[i].coord/10 + Rooms[i].coord%10) > 
                    Math.Abs(Rooms[furtherRoomIndex].coord/10 + Rooms[furtherRoomIndex].coord%10))
                {
                    furtherRoomIndex = i;
                }
            }
            Rooms[furtherRoomIndex].IsBoss = true;
        }
    };

}
