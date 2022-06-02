using System;
using System.Collections.Generic;
using System.Drawing;

namespace RglGame
{
    public class Bullet
    {
        public int SaveSpeed;
        public bool IsHit;
        public bool MovementMode;
        public int Speed;
        public Rectangle Target;
        public Rectangle Hitbox = new Rectangle();
        public bool IsEnemyTarget;
        public Bullet(Rectangle spawnCoord, bool isX, int speed, bool isEnemyTarget)
        {
            this.Speed = speed;
            Hitbox = spawnCoord;
            MovementMode = isX;
            this.IsEnemyTarget = isEnemyTarget;
        }
        public void MoveBullet()
        {
            if (!TimeFreezeAbility.IsActive)
            {
                if (MovementMode)
                    Hitbox.X += Speed;
                else
                    Hitbox.Y += Speed;
            }

        }
        public static void ControlBullets(List<Bullet> bullets)
        {
            var indexesToRemove = new List<int>();
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet();
                bullets[i].ControlHit(Player.CurrentRoom , i);
                if (bullets[i].IsHit || !CollisionChecker.IsInsideRoom(bullets[i].Hitbox.X, bullets[i].Hitbox.Y, bullets[i].Hitbox)
                    || (CollisionChecker.IsIntersectsWalls(bullets[i].Hitbox.X, bullets[i].Hitbox.Y, bullets[i].Hitbox) && bullets[i].IsEnemyTarget))
                {
                    indexesToRemove.Add(i - indexesToRemove.Count);
                }
            }
            foreach (var i in indexesToRemove)
            {
                Player.CurrentRoom.RoomBullets.RemoveAt(i);
            }
        }
        public void ControlHit(Room currentRoom, int bulletIndex)
        {
            if (IsEnemyTarget)
            {
                for (int i = 0; i < currentRoom.Enemies.Count; i++)
                    if (Hitbox.IntersectsWith(currentRoom.Enemies[i].Hitbox))
                    {
                        if (!IsHit)
                        {
                            currentRoom.Enemies[i].GetDamage(MovementMode, this);
                            IsHit = true;
                            if (Player.HitCount < 15)
                                Player.HitCount++;
                        }
                    }
            }
            else if (!IsHit)
            {
                if (Hitbox.IntersectsWith(Player.Hitbox) && !Player.IsInvincible)
                {
                    Player.GetDamage();
                    Player.Hitbox.Y += 10;
                    IsHit = true;
                }
            }
        }
    }
}
