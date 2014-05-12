#region License
/*
 * TOBam - Forward scrolling arcade shooter based in Toronto
 * Copyright (C) 2007 Rob Loach
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 1, or (at your option)
 * any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion License

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TOBam.Framework;
using TOBam.Screens;

namespace TOBam.Entities
{
    public class Player : Ship
    {
        public Player(PlayerIndex index)
        {
            this.Index = index;
            Friction = new Vector2(0.95f, 0.95f);
            //ReloadTime = 50f;
        }
        public int NextPowerupScore = 2000;
        public PlayerIndex Index = PlayerIndex.One;
        public int Lives = -1;
        public float Speed = 1f;
        public int Score = 0;

        public Keys keyLeft = Keys.Zoom;
        public Keys keyRight = Keys.Zoom;
        public Keys keyUp = Keys.Zoom;
        public Keys keyDown = Keys.Zoom;
        public Keys keyShoot = Keys.Zoom;
        public Keys keyStart = Keys.Zoom;

        public void HandleInput(GameTime gameTime)
        {
            GamePadState gamepadState = GamePad.GetState(Index);
            KeyboardState keyboard = Keyboard.GetState();

            if (Lives > 0)
            {
                bool shoot = false;
                if (gamepadState.IsConnected)
                {
                    Acceleration.X = gamepadState.ThumbSticks.Left.X / 2f;
                    Acceleration.Y = -gamepadState.ThumbSticks.Left.Y / 4f;
                    shoot = gamepadState.Triggers.Right > 0f;
                }
                else
                {
                    Acceleration.X = keyboard.IsKeyDown(keyLeft) ? -0.5f : (keyboard.IsKeyDown(keyRight) ? 0.5f : 0);
                    Acceleration.Y = keyboard.IsKeyDown(keyDown) ? 0.25f : (keyboard.IsKeyDown(keyUp) ? -0.25f : 0);
                    shoot = keyboard.IsKeyDown(keyShoot);
                }
                Rotation = Velocity.X / 15f;
                if (shoot)
                {
                    float rot = Rotation - (float)Math.PI / 2f;// (float)(Rotation * 180 / Math.PI);
                    float x = (float)(Math.Cos(rot) * BulletSpeed);
                    float y = (float)(Math.Sin(rot) * BulletSpeed);
                    Bullet bullet = Shoot(gameTime);
                    if (bullet != null)
                    {
                        bullet.Top = Position.Y;
                        Level.PlayerBullets.Add(bullet);
                    }
                }
            }
            else if(Lives == -1)
            {
                if (gamepadState.Buttons.Start == ButtonState.Pressed || keyboard.IsKeyDown(keyStart))
                {
                    Lives = 3;
                    Respawn();
                }
            }
        }
        public override void Draw(SpriteBatch sprites)
        {
            if (Lives > 0)
            {
                base.Draw(sprites);
            }
        }
        public override bool Update(GameTime time)
        {
            if (Health <= 0f && Lives > 0)
            {
                Lives--;
                Respawn();

                bool anyoneAlive = false;
                foreach (Player player in Level.Screen.players)
                {
                    if (player.Lives > 0)
                    {
                        anyoneAlive = true;
                        break;
                    }
                }
                if (!anyoneAlive)
                {
                    Level.Screen.EndGame();
                }
            }

            if (Left < 0) Left = 0;
            if (Right > ScreenManager.Width) Right = ScreenManager.Width;

            if (Top < 0) Top = 0;
            if (Bottom > ScreenManager.Height) Bottom = ScreenManager.Height;

            if (Velocity.X > 6f * Speed) Velocity.X = 6f * Speed;
            if (Velocity.X < -6f * Speed) Velocity.X = -6f * Speed;
            if (Velocity.Y > 6f * Speed) Velocity.Y = 6f * Speed;
            if (Velocity.Y < -6f * Speed) Velocity.Y = -6f * Speed;
            return base.Update(time);
        }
        public void Respawn()
        {
            Position = new Vector2(160 * ((int)Index + 1), 500);
            Velocity = new Vector2();
            Acceleration = new Vector2();
            Health = 1f;
            Speed = 1f;
            Defense = 1f ;
            BulletDamage = 0.5f;
            BulletType = "Bullet1";
            BulletSpeed = 7f;
        }
        
    }
}
