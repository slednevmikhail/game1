using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Timers;

namespace RglGame
{
    public static class Player
    {
        public static bool IsInvincible = false;
        public static Rectangle Hitbox = new Rectangle(Hitbox.X, Hitbox.Y, 40, 40);
        public static int speed = 9;
        public static int health = 10;
        public static int skillCD = 0;
        public static int InvisStartTime = -1;
        public static Room CurrentRoom;
        public static int shootTiming = 0;
        public static int shootCooldown = 13;
        public static int HitCount;
        public static void ChangeRoom(Room goal)
        {
            CurrentRoom.RoomBullets = new List<Bullet>();
            Hitbox.X = (goal.coord - CurrentRoom.coord) % 10 * CurrentRoom.Bounds.Width / 3;
            Hitbox.Y = (goal.coord - CurrentRoom.coord) / 10 * CurrentRoom.Bounds.Height / 3;
            CurrentRoom = goal;
            Enemy.ControlAI();
        }
        public static void GetDamage()
        {
            if (!IsInvincible)
            {
                health--;
                InvisStartTime = CurrentRoom.RoomTimer;
                IsInvincible = true;
                HitCount -= 3;
            }
        }
        public static void ControlState(int time)
        {
            if (time - InvisStartTime >50 && InvisStartTime != -1)
            {
                IsInvincible = false;
                InvisStartTime = -1;
            }
            Player.ControlAbilities();
        }
        public static void Shoot(bool isX, int bulletSpeed)
        {
            if (CurrentRoom.RoomTimer - shootTiming > shootCooldown && !TimeFreezeAbility.IsActive)
            {
                CurrentRoom.RoomBullets.Add(new Bullet(new Rectangle(Hitbox.X + Hitbox.Width / 4, Hitbox.Y + Hitbox.Height / 4, 20, 20), isX, bulletSpeed, true));
                shootTiming = CurrentRoom.RoomTimer;
            }
        }
        public static void TryFreezeTime(int time)
        {
                if (HitCount >= 15)
                {
                    TimeFreezeAbility.Activate();
                    HitCount -= 15;
                }
        }
        public static void ControlAbilities()
        {
            TimeFreezeAbility.UpdateState();
        }
    }
    public static class PlayerMovement
    {
        public static void MoveUp()
        {
            if (IsPossibleToMove(Player.Hitbox.X, Player.Hitbox.Y - Player.speed, Player.Hitbox))
                Player.Hitbox.Y -= Player.speed;
            CollisionChecker.ContactDoor(Player.Hitbox);
        }

        public static void MoveDown()
        {
            if (IsPossibleToMove(Player.Hitbox.X, Player.Hitbox.Y + Player.speed, Player.Hitbox))
                Player.Hitbox.Y += Player.speed;
            CollisionChecker.ContactDoor(Player.Hitbox);
        }
        public static void MoveLeft()
        {
            if (IsPossibleToMove(Player.Hitbox.X - Player.speed, Player.Hitbox.Y, Player.Hitbox))
                Player.Hitbox.X -= Player.speed;
            CollisionChecker.ContactDoor(Player.Hitbox);
        }
        public static void MoveRight()
        {
            if (IsPossibleToMove(Player.Hitbox.X + Player.speed, Player.Hitbox.Y, Player.Hitbox))
                Player.Hitbox.X += Player.speed;
            CollisionChecker.ContactDoor(Player.Hitbox);
        }
        public static bool IsPossibleToMove(int x, int y, Rectangle hitbox)
        {
            return CollisionChecker.IsInsideRoom(x, y, hitbox)
                && !CollisionChecker.IsIntersectsWalls(x, y, hitbox);
        }
    }


}
