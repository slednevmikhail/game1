using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Timers;

namespace RglGame
{
    public static class Player
    {
        public static bool isInvincible = false;
        public static Rectangle hitbox = new Rectangle(hitbox.X, hitbox.Y, 40, 40);
        public static int speed = 9;
        public static int health = 3;
        public static int skillCD = 0;
        public static int InvisStartTime = -1;
        public static Room CurrentRoom;
        public static void ChangeRoom(Room goal)
        {
            CurrentRoom.roomBullets = new List<Bullet>();
            hitbox.X = (goal.coord - CurrentRoom.coord) % 10 * CurrentRoom.bounds.Width / 3;
            hitbox.Y = (goal.coord - CurrentRoom.coord) / 10 * CurrentRoom.bounds.Height /3; 
            CurrentRoom = goal;
            Enemy.ControlAI();
        }
        public static void OnDamage()
        {
            health--;
            InvisStartTime = CurrentRoom.roomTimer;
            isInvincible = true;
        }
        public static void ControlState(int time)
        {
            if (time - InvisStartTime >50 && InvisStartTime != -1)
            {
                isInvincible = false;
                InvisStartTime = -1;
            }
        }
        public static void Shoot(bool isX, int bulletSpeed)
        {
            CurrentRoom.roomBullets.Add(new Bullet(new Rectangle(hitbox.X + hitbox.Width/4 , hitbox.Y + hitbox.Height/4, 20, 20), isX, bulletSpeed));
        }
    }
    public static class PlayerMovement
    {
        public static void MoveUp()
        {
            if (IsPossibleToMove(Player.hitbox.X, Player.hitbox.Y - Player.speed, Player.hitbox))
                Player.hitbox.Y -= Player.speed;
            CollisionChecker.ContactDoor(Player.hitbox);
        }

        public static void MoveDown()
        {
            if (IsPossibleToMove(Player.hitbox.X, Player.hitbox.Y + Player.speed, Player.hitbox))
                Player.hitbox.Y += Player.speed;
            CollisionChecker.ContactDoor(Player.hitbox);
        }
        public static void MoveLeft()
        {
            if (IsPossibleToMove(Player.hitbox.X - Player.speed, Player.hitbox.Y, Player.hitbox))
                Player.hitbox.X -= Player.speed;
            CollisionChecker.ContactDoor(Player.hitbox);
        }
        public static void MoveRight()
        {
            if (IsPossibleToMove(Player.hitbox.X + Player.speed, Player.hitbox.Y, Player.hitbox))
                Player.hitbox.X += Player.speed;
            CollisionChecker.ContactDoor(Player.hitbox);
        }
        public static bool IsPossibleToMove(int x, int y, Rectangle hitbox)
        {
            return CollisionChecker.IsInsideRoom(x, y, hitbox)
                && !CollisionChecker.IsIntersectsWalls(x, y, hitbox);
        }
    }


}
