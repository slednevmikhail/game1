using System.Drawing;

namespace RglGame
{
    public static class Player
    {
        
        public static Rectangle hitbox = new Rectangle(hitbox.X, hitbox.Y, 40, 40);
        public static int speed = 12;
        public static int health = 3;
        public static int skillCD = 0;
        public static Room CurrentRoom;
        public static void ChangeRoom(Room goal)
        {
            hitbox.X = (goal.coord - CurrentRoom.coord) % 10 * CurrentRoom.bounds.Width / 3;
            hitbox.Y = (goal.coord - CurrentRoom.coord) / 10 * CurrentRoom.bounds.Height /3; 
            CurrentRoom = goal;
        }
        public static void MoveUp()
        {
            if (IsPossibleToMove(hitbox.X + hitbox.Width/2, hitbox.Top - speed))
                hitbox.Y -= speed;
        }
        public static void MoveDown()
        {
            if (IsPossibleToMove(hitbox.X + hitbox.Width/2, hitbox.Bottom + speed))
                hitbox.Y += speed;
        }
        public static void MoveLeft()
        {
            if (IsPossibleToMove(hitbox.Left - speed, hitbox.Y + hitbox.Height/2))
                hitbox.X -= speed;
        }
        public static void MoveRight()
        {
            if (IsPossibleToMove(hitbox.Right + speed, hitbox.Y + hitbox.Height/2))
                hitbox.X += speed;
        }
        public static bool IsPossibleToMove(int x, int y)
        {
            CollisionChecker.IsInsideDoor(x,y);
            return CollisionChecker.IsInsideRoom(x,y) && !CollisionChecker.IsInsideWall(x,y);
        }
    };
}
