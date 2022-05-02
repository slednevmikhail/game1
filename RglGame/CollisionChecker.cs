using System.Drawing;
using System.Linq;
using System.Collections.Generic;
namespace RglGame
{
    public static class CollisionChecker
    {
        public static bool IsInsideRoom(int x, int y)
        {
            return Player.CurrentRoom.bounds.Left <= x && Player.CurrentRoom.bounds.Right >= x 
                && Player.CurrentRoom.bounds.Top <= y && Player.CurrentRoom.bounds.Bottom >= y;
        }
        public static bool IsInsideWall(int x, int y)
        {
            foreach (var wall in Player.CurrentRoom.Walls)
                if (wall.Left <= x && wall.Right >= x && wall.Top <= y && wall.Bottom >= y)
                    return true;
            return false;
        }
        public static void IsInsideDoor(int x, int y)
        {
            foreach (var door in Player.CurrentRoom.Doors)
                if (door.Item1.Left <= x && door.Item1.Right >= x && door.Item1.Top <= y && door.Item1.Bottom >= y)
                        Player.ChangeRoom(door.Item2);
        }
    };
}
