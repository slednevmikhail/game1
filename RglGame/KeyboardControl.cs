using System.Linq;
namespace RglGame
{
    public static class KeyboardControl
    {
        public static bool IsAdown;
        public static bool IsWdown;
        public static bool IsDdown;
        public static bool IsSdown;

        public static bool IsRightDown;
        public static bool IsLeftDown;
        public static bool IsUpDown;
        public static bool IsBotDown;
        public static void GetKeyPressed()
        {
            if (IsAdown)
                PlayerMovement.MoveLeft();
            if (IsWdown)
                PlayerMovement.MoveUp();
            if (IsDdown)
                PlayerMovement.MoveRight();
            if (IsSdown)
                PlayerMovement.MoveDown();
            if (IsRightDown)
            {
                Player.Shoot(true, 18);
            }
            else if (IsLeftDown)
            {
                Player.Shoot(true, -18);
            }
            else if (IsUpDown)
            {
                Player.Shoot(false, -18);
            }
            else if(IsBotDown)
            {
                Player.Shoot(false, 18);
            }
        }
    }
}
