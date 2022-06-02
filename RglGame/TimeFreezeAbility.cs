using System.Collections.Generic;

namespace RglGame
{
    public static class TimeFreezeAbility
    {
        public static List<Enemy> MarkedEnemies = new List<Enemy>();
        public static bool IsActive;
        public static int ActivationTime;
        public static int Cooldown;
        public static int Duration = 100;
        public static int CurrentTime
        {
            get
            {
                return Player.CurrentRoom.RoomTimer;
            }
        }
        public static void Activate()
        {
            MarkedEnemies = new List<Enemy>();
            IsActive = true;
            Player.IsInvincible = true;
            ActivationTime = CurrentTime;
        }
        public static void UpdateState()
        {
            if (IsActive && CurrentTime - ActivationTime > Duration)
            {
                IsActive = false;
                MarkedEnemies = new List<Enemy>();
                foreach (var e in Player.CurrentRoom.Enemies)
                {
                    if (e.IsMarked)
                    {
                        e.Health -= 2;
                        e.IsMarked = false;
                    }
                }
            }
            if (IsActive && CurrentTime - ActivationTime < Duration)
            {
                foreach (var e in Player.CurrentRoom.Enemies)
                {
                    if (Player.Hitbox.IntersectsWith(e.Hitbox))
                    {
                        e.IsMarked = true;
                        MarkedEnemies.Add(e);
                    }
                }
            }
        }
    }


}
