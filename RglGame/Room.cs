using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace RglGame
{
    public class Room
    {
        public List<Bullet> RoomBullets = new List<Bullet>();
        public List<(Rectangle , Room)> Doors = new List<(Rectangle, Room)>();
        public Rectangle[] DoorRects = new Rectangle[0];
        public List<Rectangle> Walls = new List<Rectangle>();
        public Rectangle Bounds = new Rectangle(-550, -310, 1100, 620);
        public List<Enemy> Enemies = new List<Enemy>();
        public List<Rectangle> EnemyBullets = new List<Rectangle>();
        public List<AttackZones> AttackZones = new List<AttackZones>();
        public int coord;
        public bool IsBoss = false;
        public bool isCleared
        {
            get
            {
                return Enemies.Count == 0;
            }
        }
        public int RoomTimer = 0;
        public Room(int pos)
        {
            coord = pos;
        }
        
        public void Connect(int RoomIndex)
        {
            Doors.Add((new Rectangle(Bounds.X * ((RoomIndex - coord) % 10) -30, Bounds.Y * ((RoomIndex - coord) / 10) - 30, 60, 60) ,
                World.Rooms.First(room => room.coord == RoomIndex)));
        }
        public void GetDoorRects()
        {
            DoorRects = Doors.Select(d => d.Item1).ToArray();
        }
        public void SpawnEnemies(bool CloseOnly = false)
        {
            var randomPreset = new Random();
            randomSpawn(true, (200, 200));
            if (randomPreset.Next(5) > 0 && !CloseOnly)
            {
                randomSpawn(false, (482, 242));
            }
            if (!CloseOnly)
            {
                if (!CollisionChecker.IsIntersectsWalls(0, 0, new Rectangle(0, 0, 20, 20), this))
                {
                    Enemies.Add(new SpiderEnemy(new Rectangle(0, 0, 30, 30)));
                    Enemies.Add(new SpiderEnemy(new Rectangle(0, 0, 30, 30)));
                    Enemies.Add(new SpiderEnemy(new Rectangle(0, 0, 30, 30)));
                    Enemies.Add(new SpiderEnemy(new Rectangle(0, 0, 30, 30)));
                }
            }
        }
        public void SpawnBoss()
        {
            Enemies.Add(new BossEnemy());
        }
        public void randomSpawn(bool isClose, (int,int) currentSpot)
        {
            var rnd = new Random();
            var enemyAmount = rnd.Next(3) + 2;
            for (int i = 0; i < enemyAmount; i++)
            {
                if (isClose)
                    Enemies.Add(new CloseEnemy(new Rectangle(currentSpot.Item1, currentSpot.Item2, 50, 50)));
                else
                    Enemies.Add(new CloneEnemy(new Rectangle(currentSpot.Item1, currentSpot.Item2, 40, 40)));
                if (i % 2 == 0)
                    currentSpot.Item1 *= -1;
                else
                    currentSpot.Item2 *= -1;
            }
        }
    }
}
