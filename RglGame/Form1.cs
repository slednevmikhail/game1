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
            var bulletPen = new Pen(Color.Red);
            var pen = new Pen(Color.Black);
            ClientSize = new Size(1280, 720);
            var centerX = 1280 / 2;
            var centerY = 720 / 2;
            var time = 0;
            var timer = new Timer
            {
                Interval = 20,
            };
            timer.Tick += (sender, args) =>
            {
                time++;
                Player.CurrentRoom.roomTimer = time;
                KeyboardControl.GetKeyPressed();
                Enemy.ControlAI();
                Player.ControlState(time);
                Bullet.ControlBullets(Player.CurrentRoom.roomBullets);
                Invalidate();
            };
            timer.Start();
            
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            Paint += (sender, args) =>
            {
                args.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                args.Graphics.TranslateTransform(centerX,centerY);
                args.Graphics.DrawRectangle(pen, Player.hitbox);
                DrawRoom(args, pen, Player.CurrentRoom);
                DrawBullets(args, bulletPen, Player.CurrentRoom);
                args.Graphics.ResetTransform();
                args.Graphics.DrawString(Player.isInvincible.ToString(), new Font("Arial", 16), Brushes.Black, new Point(0, 250));
                args.Graphics.DrawString(Player.health.ToString(), new Font("Arial", 16), Brushes.Black, new Point(0, 300));
                args.Graphics.DrawString(Player.CurrentRoom.roomTimer.ToString(), new Font("Arial", 16), Brushes.Black, new Point(0, 350));
                
            };
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) KeyboardControl.IsWdown = false;
            if (e.KeyCode == Keys.A) KeyboardControl.IsAdown = false;
            if (e.KeyCode == Keys.D) KeyboardControl.IsDdown = false;
            if (e.KeyCode == Keys.S) KeyboardControl.IsSdown = false;
            if (e.KeyCode == Keys.Right) KeyboardControl.IsRightDown = false;
            if (e.KeyCode == Keys.Left) KeyboardControl.IsLeftDown = false;
            if (e.KeyCode == Keys.Up) KeyboardControl.IsUpDown = false;
            if (e.KeyCode == Keys.Down) KeyboardControl.IsBotDown = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) KeyboardControl.IsWdown = true;
            if (e.KeyCode == Keys.A) KeyboardControl.IsAdown = true;
            if (e.KeyCode == Keys.D) KeyboardControl.IsDdown = true;
            if (e.KeyCode == Keys.S) KeyboardControl.IsSdown = true;
            if (e.KeyCode == Keys.Right) KeyboardControl.IsRightDown = true;
            if (e.KeyCode == Keys.Left) KeyboardControl.IsLeftDown = true;
            if (e.KeyCode == Keys.Up) KeyboardControl.IsUpDown = true;
            if (e.KeyCode == Keys.Down) KeyboardControl.IsBotDown = true;
        }
        public void DrawRoom(PaintEventArgs args, Pen pen, Room currentRoom)
        {
            DrawWalls(args, currentRoom);
            DrawDoors(args, currentRoom);
            args.Graphics.DrawRectangle(pen, currentRoom.bounds);
            DrawEnemies(args, currentRoom);
        }
        public void DrawBullets(PaintEventArgs args, Pen pen, Room currentRoom)
        {
            foreach (var b in currentRoom.roomBullets)
            {
                args.Graphics.DrawRectangle(pen, b.hitbox);
            }
        }
        public void DrawDoors(PaintEventArgs args, Room currentRoom)
        {
            if (currentRoom.isCleared)
                args.Graphics.FillRectangles(Brushes.Gray, currentRoom.DoorRects);
            else args.Graphics.DrawRectangles(Pens.Gray, currentRoom.DoorRects);
        }
        public void DrawWalls(PaintEventArgs args, Room currentRoom)
        {
            foreach (var w in currentRoom.Walls)
            {
                args.Graphics.FillRectangle(Brushes.LightBlue, w);
                args.Graphics.DrawRectangle(Pens.Black, w);
            }
        }
        public void DrawEnemies(PaintEventArgs args, Room currentRoom)
        {
            foreach (var e in currentRoom.Enemies)
            {
                args.Graphics.DrawRectangle(Pens.Black, e.hitbox);
                args.Graphics.DrawString(e.health.ToString(), new Font("Arial", 16), Brushes.Black, e.hitbox.X, e.hitbox.Y);
            }
        }
    }
}
