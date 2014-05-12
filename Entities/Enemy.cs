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
using TOBam.Framework;
using TOBam.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TOBam.Entities
{
    public class Enemy : Ship
    {
        public Enemy(Texture2D texture)
        {
            this.Texture = texture;
        }
        public void Deploy()
        {
            Size.X = Texture.Width;
            Size.Y = Texture.Height;
            Position.X = TOBamGame.Random.Next(0, ScreenManager.Width - (int)Size.X);
            Position.Y = -Size.Y;
        }
        //public void Draw(SpriteBatch sprites)
        //{
        //    sprites.Draw(Texture, Position, Color.White);
        //}

        public override bool Update(GameTime time)
        {
            if (Top > ScreenManager.Height)
                return false;
            Bullet bullet = Shoot(time, new Vector2(Velocity.X, BulletSpeed));
            if (bullet != null)
            {
                Level.Bullets.Add(bullet);
            }
            return base.Update(time);
        }

        public new Enemy Clone()
        {
            Enemy enemy = new Enemy(Texture);
            enemy.Acceleration = Acceleration;
            enemy.Friction = Friction;
            enemy.Position = Position;
            enemy.Rotation = Rotation;
            enemy.Size = Size;
            enemy.Velocity = Velocity;
            enemy.Health = Health;
            enemy.BulletDamage = BulletDamage;
            enemy.BulletSpeed = BulletSpeed;
            enemy.BulletType = BulletType;
            enemy.Defense = Defense;
            enemy.Level = Level;
            enemy.ReloadTime = ReloadTime;
            return enemy;
        }
    }
}
