using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RglGame
{
    public class Room
    {
        public List<Bullet> roomBullets = new List<Bullet>();
        public List<(Rectangle , Room)> Doors = new List<(Rectangle, Room)>();
        public Rectangle[] DoorRects = new Rectangle[0];
        public List<Rectangle> Walls = new List<Rectangle>();
        public Rectangle bounds = new Rectangle(-550, -310, 1100, 620);
        public List<Enemy> Enemies = new List<Enemy>();
        public int coord;
        public bool isCleared 
        {
            get
            {
                return Enemies.Count == 0;
            } 
        }
        public int roomTimer = 0;
        public Room(int pos)
        {
            coord = pos;
        }
        
        public void Connect(int RoomIndex)
        {
            Doors.Add((new Rectangle(bounds.X * ((RoomIndex - coord) % 10) -30, bounds.Y * ((RoomIndex - coord) / 10) - 30, 60, 60) ,
                World.Rooms.First(room => room.coord == RoomIndex)));
        }
        public void GetDoorRects()
        {
            DoorRects = Doors.Select(d => d.Item1).ToArray();
        }
        public void SpawnEnemies()
        {
            Enemies.Add(new CloseEnemy(new Rectangle(200, 200, 50, 50)));
            Enemies.Add(new CloseEnemy(new Rectangle(-200, -200, 50, 50)));
            Enemies.Add(new CloseEnemy(new Rectangle(200, -200, 50, 50)));
            if (!CollisionChecker.IsIntersectsWalls(0, 0, new Rectangle(0, 0, 20, 20) , this))
            {
                Enemies.Add(new SpiderEnemy(new Rectangle(0, 0, 30, 30)));
                Enemies.Add(new SpiderEnemy(new Rectangle(0, 0, 30, 30)));
            }
        }
    };
}
