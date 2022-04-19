using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RglGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            DoubleBuffered = true;
            var pen = new Pen(Color.Black);

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
                Invalidate();
            };
            timer.Start();
            Paint += (sender, args) =>
            {
                for (int i = 0; i < time; i++)
                {
                    cameraCenter.X = centerX - Player.position.X;
                    cameraCenter.Y = centerY - Player.position.Y;
                    args.Graphics.TranslateTransform(centerX,centerY);
                    args.Graphics.FillEllipse(Brushes.Blue, Player.position.X, Player.position.Y, 50, 50);
                    DrawRoom(args , pen);
                    args.Graphics.ResetTransform();

                    args.Graphics.DrawString(Player.position.X.ToString() + " " + Player.position.Y.ToString(), new Font("Arial", 16), Brushes.Black, 0, 270);
                }
            };
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, true);
        }

        private void HandleKey(Keys e, bool down)
        {
            if (e == Keys.A) Player.MoveLeft();
            if (e == Keys.D) Player.MoveRight();
            if (e == Keys.W) Player.MoveUp();
            if (e == Keys.S) Player.MoveDown();
        }
        public void DrawDoors()
        {

        }
        public void DrawRoom(PaintEventArgs args, Pen pen)
        {
            args.Graphics.DrawRectangle(pen,-550,-310, 1100, 620);
        }
        //public void DrawWorld()
        //{
        //    foreach(var e in World.ExistingRooms)

        //}
    }
}
