using System.Drawing;
using System.Linq;
using System.Collections.Generic;
namespace RglGame
{
    public static class CollisionChecker
    {
        public static bool IsInsideRoom(int x, int y, Rectangle hitbox)
        {
            return Player.CurrentRoom.bounds.Left <= x && Player.CurrentRoom.bounds.Right >= x + hitbox.Width
                && Player.CurrentRoom.bounds.Top <= y && Player.CurrentRoom.bounds.Bottom >= y + hitbox.Height;
        }
        public static bool IsIntersectsWalls(int x, int y, Rectangle hitbox, Room room = null)
        {
            if (room == null)
            {
                room = Player.CurrentRoom;
            }
            foreach (var wall in room.Walls)
                if (wall.Left <= x + hitbox.Width && wall.Right >= x && wall.Top <= y + hitbox.Height && wall.Bottom >= y)
                    return true;
            return false;
        }
            public static void ContactDoor(Rectangle hitbox)
        {
            foreach (var door in Player.CurrentRoom.Doors)
                if (door.Item1.IntersectsWith(hitbox) && Player.CurrentRoom.isCleared)
                        Player.ChangeRoom(door.Item2);
        }
        //public static bool IsIntersectsEnemies(int x, int y, Rectangle hitbox)
        //{
        //    foreach (var e in Player.CurrentRoom.Enemies)
        //    {
        //        if (e.hitbox.Left <= x + hitbox.Width
        //            && e.hitbox.Right >= x
        //            && e.hitbox.Top <= y + hitbox.Height && e.hitbox.Bottom >= y)
        //            return true;
        //    }
        //    return false;
        //}
    };
}
