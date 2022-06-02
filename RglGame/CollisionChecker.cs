using System.Drawing;
using System.Linq;
using System.Collections.Generic;
namespace RglGame
{
    public static class CollisionChecker
    {
        public static bool IsInsideRoom(int x, int y, Rectangle hitbox)
        {
            return Player.CurrentRoom.Bounds.Left <= x && Player.CurrentRoom.Bounds.Right >= x + hitbox.Width
                && Player.CurrentRoom.Bounds.Top <= y && Player.CurrentRoom.Bounds.Bottom >= y + hitbox.Height;
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
                {
                    if (!door.Item2.IsBoss || World.ClearedRooms.Count == World.Rooms.Count - 1)
                    {
                        Player.ChangeRoom(door.Item2);
                        World.ClearedRooms.Add(Player.CurrentRoom.coord);
                    }
                }
        }
    };
}
