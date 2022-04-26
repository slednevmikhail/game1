using System.Drawing;

namespace RglGame
{
    public static class Player
    {
        public static Point position = new Point(0, 0);
        public static Size size = new Size(40, 40);
        public static int speed = 15;
        public static int health = 3;
        public static int skillCD = 0;
        public static int height = 40;
        public static int width = 40;
        public static Room CurrentRoom;
        public static void ChangeRoom(Room goal)
        {
            position.X = (goal.coord - CurrentRoom.coord) % 10 * CurrentRoom.bounds.Width / 3;
            position.Y = (goal.coord - CurrentRoom.coord) / 10 * CurrentRoom.bounds.Height /3; 
            CurrentRoom = goal;
        }
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
