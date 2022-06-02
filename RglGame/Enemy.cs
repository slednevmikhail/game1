using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading;

namespace RglGame
{
    public class Enemy
    {
        public bool IsMarked;
        public int Health;
        public int Speed;
        public int SaveSpeed;
        public int SaveHealth;
        public bool isInvincible = false;
        public Rectangle Hitbox = new Rectangle();
        public static void ControlAI()
        {
            if (!TimeFreezeAbility.IsActive)
            {
                for (int i = 0; i < Player.CurrentRoom.Enemies.Count; i++)
                {
                    Player.CurrentRoom.Enemies[i].Move();
                    Player.CurrentRoom.Enemies[i].Attack();
                    if (Player.CurrentRoom.Enemies[i].Health <= 0)
                    {
                        Player.CurrentRoom.Enemies.RemoveAt(i);
                        i--;
                    }
                    if (Player.CurrentRoom.IsBoss)
                    {
                        AttackZones.ControlAttackZones();
                    }
                }
            }
        }
        public virtual void Move()
        {
            
        }
        public virtual void FollowTarget(Rectangle target)
        {
            if (Hitbox.X - target.X > 0 && !CollisionChecker.IsIntersectsWalls(Hitbox.X - Speed, Hitbox.Y, Hitbox))
                MoveX(-Speed);
            if (Hitbox.X - target.X < 0 && !CollisionChecker.IsIntersectsWalls(Hitbox.X + Speed, Hitbox.Y, Hitbox))
                MoveX(Speed);
            if (Hitbox.Y - target.Y > 0 && !CollisionChecker.IsIntersectsWalls(Hitbox.X, Hitbox.Y - Speed, Hitbox))
                MoveY(-Speed);
            if (Hitbox.Y - target.Y < 0 && !CollisionChecker.IsIntersectsWalls(Hitbox.X, Hitbox.Y + Speed, Hitbox))
                MoveY(Speed);
        }
        public void MoveX(int speed)
        {
            Hitbox.X += speed;
        }
        public void MoveY(int speed)
        {
            Hitbox.Y += speed;
        }
        public virtual void GetDamage(bool movementMode, Bullet bullet)
        {
            if(!isInvincible)
            {
                if (movementMode)
                    Hitbox.X += 10 * bullet.Speed.CompareTo(0);
                else
                    Hitbox.Y += 10 * bullet.Speed.CompareTo(0);
                Health--;
            }
        }
        public virtual void Attack()
        {

        }
    }
    public class CloseEnemy:Enemy
    {
        public CloseEnemy(Rectangle hitbox)
        {
            Hitbox = hitbox;
            Health = 4;
            Speed = 3;
        }
        public override void Attack()
        {
            if (Hitbox.IntersectsWith(Player.Hitbox))
            {
                Player.GetDamage();
            }
        }
        public override void Move()
        {
            FollowTarget(Player.Hitbox);
        }
    }

    public class CloneEnemy : Enemy
    {
        private Rectangle goal = new Rectangle();
        private int dX;
        private int dY;
        private int duplicateChance;
        public CloneEnemy(Rectangle hitbox, int duplicateChance = 1)
        {
            this.Hitbox = hitbox;
            Health = 2;
            this.duplicateChance = duplicateChance;
            goal= hitbox;
            GetRandomDirection();
            Speed = 6;
        }
        public override void Move()
        {
            if (Hitbox.IntersectsWith(goal))
            {
                GetRandomDirection();
                Duplicate();
            }
            FollowTarget(goal);
        }
        public void Duplicate()
        {
            var rnd = new Random();
            var rand = rnd.Next(duplicateChance);
            if (rand == 0)
            {
                Player.CurrentRoom.Enemies.Add(new CloneEnemy(Hitbox, duplicateChance + 1) { Health = 1 });
                duplicateChance++;
            }
        }
        public void GetRandomDirection()
        {
            var rnd = new Random();
            var rand = rnd.Next(2);
            if (rand == 0)
            {
                dX = 1;
                dY = -1;
            }
            else
            {
                dX = -1;
                dY = 1;
            }
            goal.X = dX * goal.X;
            goal.Y = dY * goal.Y;
        }
        public override void Attack()
        {
            if (Hitbox.IntersectsWith(Player.Hitbox))
            {
                Player.GetDamage();
            }
        }
    }

    public class SpiderEnemy : Enemy
    {
        public int moveTiming;
        public int moveCd = 200;
        public int movingTime = 50;
        public SpiderEnemy(Rectangle hitbox)
        {
            this.Hitbox = hitbox;
            Health = 2;
            Speed = 9;
        }
        public override void Move()
        {
            var rnd = new Random();
            var dx = rnd.Next(3).CompareTo(1) * Speed;
            var dy = rnd.Next(3).CompareTo(1) * Speed;
            if (Player.CurrentRoom.RoomTimer - moveTiming > moveCd || Player.CurrentRoom.RoomTimer - moveTiming < movingTime)
            {
                if (!CollisionChecker.IsIntersectsWalls(Hitbox.X + dx, Hitbox.Y, Hitbox) && CollisionChecker.IsInsideRoom(Hitbox.X + dx, Hitbox.Y, Hitbox))
                    Hitbox.X += dx;
                if (!CollisionChecker.IsIntersectsWalls(Hitbox.X, Hitbox.Y + dy, Hitbox) && CollisionChecker.IsInsideRoom(Hitbox.X, Hitbox.Y + dy, Hitbox))
                    Hitbox.Y += dy;
                moveTiming = Player.CurrentRoom.RoomTimer;
            }
            if (Hitbox.IntersectsWith(Player.Hitbox))
            {
                Player.GetDamage();
            }
        }
    }

    public class BossEnemy: Enemy
    {
        public int attackCooldown = 20;
        public int attackTiming = 0;
        private bool isShooting;
        public int moveCd;
        public int moveTiming;
        public int AnimationDuration = 150;
        public Rectangle preChargePos = new Rectangle(0, 0, 10, 10);
        public int zonesCooldown;
        public Rectangle bullethitbox;
        public Rectangle goal;
        public Rectangle AttackZone;
        private int bulletSpeed = 13;
        private bool[] PhaseFlags = new bool[4] {false, false, false, false };
        private Random attackZoneSeed;
        private Room currentRoom
        {
            get
            {
                return Player.CurrentRoom;
            }
        }
        private bool isDashing;
        private int DashStartTiming;
        private Rectangle phaseThreeTarget;
        public bool isDead;
        public BossEnemy()
        {
            Hitbox = new Rectangle(-100, -100, 200, 200);
            Health = 50;
            Speed = 6;
            goal = new Rectangle(Player.CurrentRoom.Bounds.Left, Player.CurrentRoom.Bounds.Top, 1, 1);
            attackZoneSeed = new Random(2);
        }
        public override void Move()
        {
            if (PhaseFlags[0])
            {
                FollowTarget(goal);
                if (Hitbox.IntersectsWith(goal))
                {
                    ChangeSide();
                }
            }
            if (!PhaseFlags[0] && !PhaseFlags[1] && !PhaseFlags[2] && !PhaseFlags[3])
            {
                isInvincible = true;
                var startPos = new Rectangle(-100, Player.CurrentRoom.Bounds.Top, 1, 1);
                FollowTarget(startPos);
                if (Hitbox.IntersectsWith(startPos))
                {
                    isInvincible = false;
                    PhaseFlags[0] = true;
                    isShooting = true;
                    Speed = 6;
                }
            }
        }
        public void ChangeSide()
        {
            if (goal.X == currentRoom.Bounds.Left)
            {
                goal.X = currentRoom.Bounds.Right;
            }
            else goal.X = currentRoom.Bounds.Left;
        }
        public override void Attack()
        {
            if (Hitbox.IntersectsWith(Player.Hitbox))
            {
                Player.GetDamage();
            }
            if (Player.CurrentRoom.RoomTimer - attackTiming > attackCooldown && isShooting)
            {
                attackTiming = Player.CurrentRoom.RoomTimer;
                var bullethitbox = new Rectangle(Hitbox.X + Hitbox.Width / 2, Hitbox.Bottom, 90, 90);
                Player.CurrentRoom.RoomBullets.Add(new Bullet(bullethitbox, false, bulletSpeed, false));
            }
            if (Health < 40 && Health > 30 && !PhaseFlags[1])
            {
                PhaseFlags[1] = true;
                AttackPhaseOne();
            }
            if (Health <= 30 && Health > 22)
            {
                isShooting = false;
                PhaseFlags[0] = false;
                if (!PhaseFlags[2])
                {
                    Speed += 30;
                }
                AttackPhaseTwo();                
            }
            if (Health <= 22 && Health > 1)
            {
                if (!PhaseFlags[2])
                {
                    Player.CurrentRoom.Enemies.RemoveRange(1, Player.CurrentRoom.Enemies.Count - 1);
                    PhaseFlags[2] = true;
                }
                Player.CurrentRoom.AttackZones = new List<AttackZones>();
                Speed = 21;
                AttackPhaseThree();
            }
            if (Health <= 15)
            {
                if (!PhaseFlags[3])
                {
                    Player.CurrentRoom.Enemies.Add(new CloseEnemy(new Rectangle(300, 300, 40, 40)) { Health = 9999 });
                    Player.CurrentRoom.Enemies.Add(new CloseEnemy(new Rectangle(-300, -300, 40, 40)) { Health = 9999 });
                    PhaseFlags[3] = true;
                }
            }
            if (Health == 1)
            {
                if (PhaseFlags[3])
                {
                    Player.CurrentRoom.Enemies.RemoveRange(1, Player.CurrentRoom.Enemies.Count - 1);
                    PhaseFlags[3] = false;
                    isInvincible = true;
                    Speed = 2;
                } 
                FollowTarget(new Rectangle(-100, -100, 1, 1));
                if (Hitbox.X >= -101 && Hitbox.X <= -99 && Hitbox.Y >= -101 && Hitbox.Y <= -99 && !isDead)
                {
                    Hitbox.X = -120;
                    isDead = true;
                    moveTiming = Player.CurrentRoom.RoomTimer;
                }
                if (isDead)
                {
                    KillBoss();
                }
            }
        }
        public override void GetDamage(bool movementMode, Bullet bullet)
        {
            if (!isInvincible)
            {
                Health--;
            }
        }
        public void AttackPhaseOne()
        {
            Player.CurrentRoom.SpawnEnemies(true);
        }
        public void AttackPhaseTwo()
        {
            if (Hitbox.X < -100 || Hitbox.X > -90)
            {
                Speed = 25;
                FollowTarget(new Rectangle(-100, Hitbox.Y, 1, 1));
            }
            if (currentRoom.AttackZones.Count == 0)
            {
                var next = attackZoneSeed.Next(2);
                var x = currentRoom.Bounds.X + currentRoom.Bounds.Width / 2 * next;
                currentRoom.AttackZones.Add(new AttackZones(
                    new Rectangle(x,
                    currentRoom.Bounds.Top,
                    currentRoom.Bounds.Width / 2, Player.CurrentRoom.Bounds.Height)));
                attackTiming = currentRoom.RoomTimer;
                currentRoom.Enemies.Add(new CloseEnemy(new Rectangle(
                    Player.CurrentRoom.Bounds.Right - (currentRoom.Bounds.Width / 2) * next - 340,
                    Player.CurrentRoom.Bounds.Y + 50, 30, 30)));
                currentRoom.Enemies.Add(new CloseEnemy(new Rectangle(
                    Player.CurrentRoom.Bounds.Right - (currentRoom.Bounds.Width / 2) * next - 310,
                    Player.CurrentRoom.Bounds.Y + 50, 30, 30)));
            }
            else if (currentRoom.RoomTimer - attackTiming > 150)
            {
                currentRoom.AttackZones.RemoveAt(0);
            }
        }
        public void AttackPhaseThree()
        {
            GetTarget();
            if (isDashing)
            {
                DashToPlayer(phaseThreeTarget);
            }
        }
        public void GetTarget()
        {
            if (Player.CurrentRoom.RoomTimer - DashStartTiming > 35 && !isDashing)
            {
                DashStartTiming = Player.CurrentRoom.RoomTimer;
                phaseThreeTarget = Player.Hitbox;
                isDashing = true;
            }
        }
        public void DashToPlayer(Rectangle target)
        {
            FollowTarget(target);
            if (Hitbox.IntersectsWith(target))
            {
                isDashing = false;
            }
        }
        public void KillBoss()
        {
            Speed = 15;
            if (PlayDeathAnimation())
            {
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Top, Hitbox.Left, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Top, Hitbox.Right, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Bottom, Hitbox.Left, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Bottom, Hitbox.Right, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Top, Hitbox.Left, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Top, Hitbox.Right, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Bottom, Hitbox.Left, 30, 30)));
                Player.CurrentRoom.Enemies.Add(new SpiderEnemy(new Rectangle(Hitbox.Bottom, Hitbox.Right, 30, 30)));
                Health = 0;
            }
        }
        public bool PlayDeathAnimation()
        {
            var rnd = new Random();
            var dx = rnd.Next(3).CompareTo(1) * Speed;
            var dy = rnd.Next(3).CompareTo(1) * Speed;
            if (Player.CurrentRoom.RoomTimer - moveTiming < AnimationDuration)
            {
                if (!CollisionChecker.IsIntersectsWalls(Hitbox.X + dx, Hitbox.Y, Hitbox) && CollisionChecker.IsInsideRoom(Hitbox.X + dx, Hitbox.Y, Hitbox))
                    Hitbox.X += dx;
                if (!CollisionChecker.IsIntersectsWalls(Hitbox.X, Hitbox.Y + dy, Hitbox) && CollisionChecker.IsInsideRoom(Hitbox.X, Hitbox.Y + dy, Hitbox))
                    Hitbox.Y += dy;
                return false;
            }
            return true;
        }
    }
}
