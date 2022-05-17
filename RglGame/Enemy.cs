using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading;

namespace RglGame
{
    public class Enemy
    {
        public int health;
        public int speed;
        public Rectangle hitbox = new Rectangle();
        public static void ControlAI()
        {
            for (int i = 0; i < Player.CurrentRoom.Enemies.Count; i++)
            {
                Player.CurrentRoom.Enemies[i].Move();
                if (Player.CurrentRoom.Enemies[i].health <= 0)
                {
                    Player.CurrentRoom.Enemies.RemoveAt(i);
                    i--;
                }
            }
        }
        public virtual void Move()
        {

        }
        public void MoveX(int speed)
        {
            hitbox.X += speed;
        }
        public void MoveY(int speed)
        {
            hitbox.Y += speed;
        }
        public void GetDamage()
        {
            health--;
        }
    }
    public class CloseEnemy:Enemy
    {
        public CloseEnemy(Rectangle hitbox)
        {
            this.hitbox = hitbox;
            health = 4;
            speed = 3;
        }
        public override void Move()
        {
            if (hitbox.X - Player.hitbox.X > 0 && !CollisionChecker.IsIntersectsWalls(hitbox.X - speed, hitbox.Y, hitbox))
                MoveX(-speed);
            if (hitbox.X - Player.hitbox.X < 0 && !CollisionChecker.IsIntersectsWalls(hitbox.X + speed, hitbox.Y, hitbox))
                MoveX(speed);
            if (hitbox.Y - Player.hitbox.Y > 0 && !CollisionChecker.IsIntersectsWalls(hitbox.X, hitbox.Y - speed, hitbox))
                MoveY(-speed);
            if (hitbox.Y - Player.hitbox.Y < 0 && !CollisionChecker.IsIntersectsWalls(hitbox.X, hitbox.Y + speed, hitbox))
                MoveY(speed);
            if (hitbox.IntersectsWith(Player.hitbox) && !Player.isInvincible)
            {
                Player.OnDamage();
            }
        }
    }

    public class SpiderEnemy : Enemy
    {
        public int moveTiming;
        public int movecd = 5;
        public SpiderEnemy(Rectangle hitbox)
        {
            this.hitbox = hitbox;
            health = 2;
            speed = 10;
        }
        public override void Move()
        {
            var rnd = new Random();
            var dx = rnd.Next(3).CompareTo(1) * speed;
            var dy = rnd.Next(3).CompareTo(1) * speed;
            if (Player.CurrentRoom.roomTimer - moveTiming > movecd)
            {
                if (!CollisionChecker.IsIntersectsWalls(hitbox.X + dx, hitbox.Y, hitbox))
                    hitbox.X += dx;
                if (!CollisionChecker.IsIntersectsWalls(hitbox.X, hitbox.Y + dy, hitbox))
                    hitbox.Y += dy;
                moveTiming = Player.CurrentRoom.roomTimer;
            }
            if (hitbox.IntersectsWith(Player.hitbox) && !Player.isInvincible)
            {
                Player.OnDamage();
            }
        }
    }
}
