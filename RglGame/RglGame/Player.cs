using System.Drawing;

namespace RglGame
{
    public static class Player
    {
        public static Point position = new Point(0, 0);
        public static (int, int) location = (0, 0);
        public static int speed = 15;
        public static int health = 3;
        public static int skillCD = 0;
        public static int height = 40;
        public static int width = 40;

        public static void MoveUp()
        {
                position.Y -= speed;
        }
        public static void MoveDown()
        {
                position.Y += speed;
        }
        public static void MoveLeft()
        {
                position.X -= speed;
        }
        public static void MoveRight()
        {
            position.X += speed;
        }
    };
}
