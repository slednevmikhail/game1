using System;
using System.Collections.Generic;
using System.Drawing;

namespace RglGame
{
    public class Bullet
    {
        public bool IsHit;
        public bool movementMode;
        public static Size size = new Size(10, 10);
        public int speed;
        public Rectangle hitbox = new Rectangle();
        public static int shootTiming = 0;
        public static int shootCooldown = 13;
        public Bullet(Rectangle spawnCoord, bool isX, int speed)
        {
            if (Player.CurrentRoom.roomTimer - shootTiming > shootCooldown)
            {
                this.speed = speed;
                hitbox = spawnCoord;
                movementMode = isX;
                shootTiming = Player.CurrentRoom.roomTimer;               
            }
        }
        public void MoveBullet()
        {
            if (movementMode)
                hitbox.X += speed;
            else
                hitbox.Y += speed;

        }
        public static void ControlBullets(List<Bullet> bullets)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet();
                bullets[i].ControlHit(Player.CurrentRoom , i);
                if (bullets[i].IsHit || !CollisionChecker.IsInsideRoom(bullets[i].hitbox.X, bullets[i].hitbox.Y, bullets[i].hitbox)
                    || CollisionChecker.IsIntersectsWalls(bullets[i].hitbox.X, bullets[i].hitbox.Y, bullets[i].hitbox))
                {
                    Player.CurrentRoom.roomBullets.RemoveAt(i);
                }
            }
        }
        public void ControlHit(Room currentRoom, int bulletIndex)
        {
            for (int i = 0; i < currentRoom.Enemies.Count; i++)
                if (hitbox.IntersectsWith(currentRoom.Enemies[i].hitbox))
                {
                    if (movementMode)
                    currentRoom.Enemies[i].hitbox.X += 10 * speed.CompareTo(0);
                    else
                    currentRoom.Enemies[i].hitbox.Y += 10 * speed.CompareTo(0);

                    currentRoom.Enemies[i].GetDamage();
                    IsHit = true;
                }
        }
    }
}
