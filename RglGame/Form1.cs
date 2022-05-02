using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace RglGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            DoubleBuffered = true;
            var pen = new Pen(Color.Black);
            var KeyList = new List<KeyEventArgs>();
            ClientSize = new Size(1280, 720);
            var centerX = 1280 / 2;
            var centerY = 720 / 2;
            Point cameraCenter = new Point(centerX, centerY);
            var time = 0;
            var timer = new Timer
            {
                Interval = 40,
            };
            timer.Tick += (sender, args) =>
            {
                time++;
                KeyboardControl.GetMoveDirection();
                Invalidate();
            };
            timer.Start();
            
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            Paint += (sender, args) =>
            {
                for (int i = 0; i < time; i++)
                {
                    cameraCenter.X = centerX - Player.hitbox.X;
                    cameraCenter.Y = centerY - Player.hitbox.Y;
                    args.Graphics.TranslateTransform(centerX,centerY);
                    args.Graphics.DrawRectangle(pen, Player.hitbox);
                    DrawRoom(args , pen, Player.CurrentRoom);
                    args.Graphics.ResetTransform();
                    args.Graphics.DrawString(Player.hitbox.X.ToString() + " " + Player.hitbox.Y.ToString(), new Font("Arial", 16), Brushes.Black, 0, 270);
                    
                }
            };
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) KeyboardControl.IsWdown = false;
            if (e.KeyCode == Keys.A) KeyboardControl.IsAdown = false;
            if (e.KeyCode == Keys.D) KeyboardControl.IsDdown = false;
            if (e.KeyCode == Keys.S) KeyboardControl.IsSdown = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) KeyboardControl.IsWdown = true;
            if (e.KeyCode == Keys.A) KeyboardControl.IsAdown = true;
            if (e.KeyCode == Keys.D) KeyboardControl.IsDdown = true;
            if (e.KeyCode == Keys.S) KeyboardControl.IsSdown = true;
        }
        public void DrawRoom(PaintEventArgs args, Pen pen, Room currentRoom)
        {
            args.Graphics.DrawRectangle(pen, currentRoom.bounds);
            DrawDoors(args, pen, currentRoom);
            foreach (var block in currentRoom.Walls)
                args.Graphics.DrawRectangle(pen, block);
        }
        public void DrawDoors(PaintEventArgs args, Pen pen, Room currentRoom)
        {
            foreach (var door in currentRoom.Doors)
                args.Graphics.DrawRectangle(pen, door.Item1);
        }
    }
}
