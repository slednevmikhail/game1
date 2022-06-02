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
                    Player.CurrentRoom.RoomTimer = time;
                    KeyboardControl.GetKeyPressed();
                    Enemy.ControlAI();
                    Player.ControlState(time);
                    Bullet.ControlBullets(Player.CurrentRoom.RoomBullets);
                    Invalidate();
            };
            timer.Start();

            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            Paint += (sender, args) =>
            {
                args.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                args.Graphics.TranslateTransform(centerX, centerY);
                DrawScene(args, World.GameStarted);
                args.Graphics.ResetTransform();
                args.Graphics.DrawString("Здоровье: " + Player.health.ToString(), new Font("Arial", 16), Brushes.Red, new Point(0, 10));
                args.Graphics.DrawString("Способность:" + Player.HitCount.ToString() + "/15", new Font("Arial", 16), Brushes.Black, new Point(150, 10));
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
            if (e.KeyCode == Keys.Space) KeyboardControl.IsSpaceDown = false;
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
            if (e.KeyCode == Keys.Space) KeyboardControl.IsSpaceDown = true;
            if (e.KeyCode == Keys.Enter) KeyboardControl.IsEnterDown = true;
            if (e.KeyCode == Keys.Escape) Application.Exit();
        }
        public void DrawRoom(PaintEventArgs args, Pen pen, Room currentRoom)
        {
            foreach (var e in currentRoom.EnemyBullets)
            {
                args.Graphics.DrawRectangle(pen, e);
            }
            DrawWalls(args, currentRoom);
            DrawAttackZone(args, currentRoom, currentRoom.RoomTimer);
            DrawDoors(args, currentRoom);
            args.Graphics.DrawRectangle(pen, currentRoom.Bounds);
            DrawEnemies(args, currentRoom);
        }
        public void DrawPlayer(PaintEventArgs args)
        {
            args.Graphics.DrawRectangle(Pens.Black, Player.Hitbox);
            DrawAbility(args);
        }
        public void DrawBullets(PaintEventArgs args, Pen pen, Room currentRoom)
        {
            foreach (var b in currentRoom.RoomBullets)
            {
                args.Graphics.DrawRectangle(pen, b.Hitbox);
            }
        }
        public void DrawDoors(PaintEventArgs args, Room currentRoom)
        {
            foreach (var door in currentRoom.Doors)
            {
                if (currentRoom.isCleared)
                {
                    if (door.Item2.IsBoss)
                    {
                        if (World.ClearedRooms.Count < 7)
                            args.Graphics.DrawRectangle(Pens.Red, door.Item1);
                        else
                            args.Graphics.FillRectangle(Brushes.Red, door.Item1);
                    }
                    else
                        args.Graphics.FillRectangle(Brushes.Gray, door.Item1);
                }
                else
                {
                    if (door.Item2.IsBoss)
                        args.Graphics.DrawRectangle(Pens.Red, door.Item1);
                    else args.Graphics.DrawRectangle(Pens.Gray, door.Item1);
                }
            }
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
                args.Graphics.DrawRectangle(Pens.Black, e.Hitbox);
                args.Graphics.DrawString(e.Health.ToString(), new Font("Arial", 16), Brushes.Black, e.Hitbox.X, e.Hitbox.Y);
            }
        }
        public void DrawAttackZone(PaintEventArgs args, Room currentRoom, int time)
        {
            foreach (var zone in currentRoom.AttackZones)
            {
                if (zone.IsActive)
                {
                    args.Graphics.FillRectangle(Brushes.Red, zone.Area);
                }
                else
                {
                    if (time % 15 == 0)
                    {
                        args.Graphics.DrawRectangle(Pens.Red, zone.Area);
                        args.Graphics.DrawLine(Pens.Red, zone.Area.Left, zone.Area.Top, zone.Area.Right, zone.Area.Bottom);
                        args.Graphics.DrawLine(Pens.Red, zone.Area.Right, zone.Area.Top, zone.Area.Left, zone.Area.Bottom);
                    }
                }
            }
        }
        public void DrawAbility(PaintEventArgs args)
        {
            if (TimeFreezeAbility.IsActive)
                args.Graphics.FillRectangle(Brushes.Black, Player.Hitbox);
            foreach (var e in Player.CurrentRoom.Enemies)
            {
                if (e.IsMarked)
                {
                    args.Graphics.FillRectangle(Brushes.Black, e.Hitbox);
                }
            }
            for (int i = 1; i < TimeFreezeAbility.MarkedEnemies.Count; i++)
            {
                var thisEnemy = TimeFreezeAbility.MarkedEnemies[i];
                var previousEnemy = TimeFreezeAbility.MarkedEnemies[i - 1];
                args.Graphics.DrawLine(Pens.Black,
                    thisEnemy.Hitbox.X + thisEnemy.Hitbox.Width / 2,
                    thisEnemy.Hitbox.Y + thisEnemy.Hitbox.Height / 2,
                    previousEnemy.Hitbox.X + previousEnemy.Hitbox.Width / 2,
                    previousEnemy.Hitbox.Y + previousEnemy.Hitbox.Height / 2);
            }
        }
        public void DrawScene(PaintEventArgs args, bool isGameStarted)
        {
            if (isGameStarted)
            {
                DrawRoom(args, Pens.Black, Player.CurrentRoom);
                DrawBullets(args, Pens.Red, Player.CurrentRoom);
                DrawPlayer(args);
            }
            else
            {
                args.Graphics.DrawRectangle(Pens.Black, Player.CurrentRoom.Bounds);
                DrawManual(args);
            }
        }
        public void DrawManual(PaintEventArgs args)
        {
            args.Graphics.DrawString("WASD - движение" , new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left , -250);
            args.Graphics.DrawString("Стрелочки - атака", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, -175);
            args.Graphics.DrawString("Ваш персонаж имеет способность остановки времени", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, -100);
            args.Graphics.DrawString("Во время ее активации он неуязвим и наносит урон врагам, проходя через них", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, -75);
            args.Graphics.DrawString("Она накапливается с попаданием по врагу, отнимается при получении урона", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, -50);
            args.Graphics.DrawString("Пробел - использовать способность, количество ударов для способности - справа от здоровья", new Font("Arial", 16), Brushes.Black, Player.CurrentRoom.Bounds.Left, 0);
            args.Graphics.DrawString("Здоровье персонажа - в левом верхнем углу", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, 50);
            args.Graphics.DrawString("Ваша цель - зачистить все комнаты от монстров и победить босса", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, 100);
            args.Graphics.DrawString("Его дверь помечена красным и не откроется, пока не будут зачищены все комнаты", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left,150);
            args.Graphics.DrawString("Нажмите Enter, чтобы начать", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, 200);
            args.Graphics.DrawString("Escape - выход", new Font("Arial", 20), Brushes.Black, Player.CurrentRoom.Bounds.Left, 250);
        }
    }
}
