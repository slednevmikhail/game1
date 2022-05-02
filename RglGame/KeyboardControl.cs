namespace RglGame
{
    public static class KeyboardControl
    {
        public static bool IsAdown;
        public static bool IsWdown;
        public static bool IsDdown;
        public static bool IsSdown;

        public static void GetMoveDirection()
        {
            if (IsAdown)
                Player.MoveLeft();
            if (IsWdown)
                Player.MoveUp();
            if (IsDdown)
                Player.MoveRight();
            if (IsSdown)
                Player.MoveDown();
        }
    }
}
