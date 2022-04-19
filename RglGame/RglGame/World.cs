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
        public static List<(int,int)> ExistingRoomsTuples = new List<(int, int)> { (0,0) };
        public static new Dictionary<(int, int), Room> Rooms = new Dictionary<(int, int), Room>();
        
        public static void GenerateRooms(int roomNumber)
        {
            Rooms.Add((0, 0), new Room(0, 0));
            for (int i = 0; i < roomNumber; i++)
            {
                var vacantRooms = new HashSet<(int, int)>();
                foreach (var currentRoom in Rooms.Keys)
                {
                    for (int dx = -1; dx < 2; dx++)
                        for (int dy = -1; dy < 2; dy++)
                            if (dx != dy && dx * -1 != dy)
                                if (!Rooms.ContainsKey((currentRoom.Item1 + dx, currentRoom.Item2 + dy)))
                                    vacantRooms.Add((currentRoom.Item1 + dx, currentRoom.Item2 + dy));

                }
                InitRooms(vacantRooms);
            }
        }
        public static void InitRooms(HashSet<(int,int)> vacantRooms)
        {
            Random rnd = new Random();
            var rndTuple = vacantRooms.ElementAt(rnd.Next(0, vacantRooms.Count));
            Rooms.Add((rndTuple.Item1, rndTuple.Item2) , new Room(rndTuple.Item1, rndTuple.Item2));
            
        }
        //public static void GenerateDoors()
        //{
        //    foreach (var t in Rooms.Keys)
        //        for (int dx = -1; dx < 2; dx++)
        //            for (int dy = -1; dy < 2; dy++)
        //                if (dx != dy && dx * -1 != dy)
        //                    if (Rooms.ContainsKey((t.Item1 + dx, t.Item2 + dy)))
        //                    {
        //                        Rooms[(t.Item1, t.Item2)].doors.Add(new Door { To = (t.Item1 + dx, t.Item2 + dy)});
        //                    }                            
        //}

    };
    public class Door
    {
        public (int,int) To;
        
        //относительно центра комнаты
    };
}
