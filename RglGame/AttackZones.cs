using System.Drawing;
using System.Collections.Generic;

namespace RglGame
{
    public class AttackZones
    {
        //public static List<AttackZones> SaveAttackZones;
        public int CreationTime;
        public Rectangle Area;
        public Rectangle SaveArea;
        public bool IsActive
        {
            get
            {
                return (Player.CurrentRoom.RoomTimer - CreationTime > 100);
            }
        }
        public AttackZones(Rectangle area)
        {
            Area = area;
            CreationTime = Player.CurrentRoom.RoomTimer;
        }
        public void UpdateAttackZone()
        {
            if (IsActive && Area.IntersectsWith(Player.Hitbox) && !Player.IsInvincible)
            {
                Player.GetDamage();
            }
        }
        public static void ControlAttackZones()
        {
            if (!TimeFreezeAbility.IsActive)
            {
                foreach (var zone in Player.CurrentRoom.AttackZones)
                {
                    zone.UpdateAttackZone();
                }
            }
        }
        public void SaveInfo()
        {
            SaveArea = Area;
        }
        public void BackUpInfo()
        {
            Area = SaveArea;
        }
    }
}
