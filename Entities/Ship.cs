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
using TOBam.Entities;
using TOBam.Framework;
using TOBam.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOBam.Entities
{
    public class Ship : Entity
    {
        public float Health = 1f;
        public float Defense = 1f;
        public float BulletDamage = 0.5f;
        public string BulletType = "Bullet1";
        public float BulletSpeed = 7f;
        public double ReloadTime = 250;
        public double LastShot = 0;
        public Texture2D Texture;
        
        public Ship()
        {
        }
        public Ship(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void Draw(SpriteBatch sprites)
        {
            sprites.Draw(Texture, new Vector2(Position.X + Width / 2f, Position.Y + Height / 2f), null, new Color(255, (byte)(Health * 255f), (byte)(Health * 255f), 255), Rotation, new Vector2(Width / 2f, Height / 2f), 1f, SpriteEffects.None, 0);    
        }

        public Bullet Shoot(GameTime gameTime)
        {
            float rot = Rotation - (float)Math.PI / 2f;// (float)(Rotation * 180 / Math.PI);
            float x = (float)(Math.Cos(rot) * BulletSpeed);
            float y = (float)(Math.Sin(rot) * BulletSpeed);
            return Shoot(gameTime, new Vector2(x, y));
        }

        public bool GetHit(float damage)
        {
            this.Health -= damage * this.Defense;
            if (this.Health <= 0f)
            {
                return true;
            } return false;
        }

        public Bullet Shoot(GameTime time, Vector2 velocity)
        {
            System.Diagnostics.Debug.WriteLine(LastShot.ToString() + " " + ReloadTime.ToString() + " " + time.TotalGameTime.TotalMilliseconds);
            if (LastShot + ReloadTime < time.TotalGameTime.TotalMilliseconds)
            {
                LastShot = time.TotalGameTime.TotalMilliseconds;
                Bullet bullet = new Bullet(Level.AvailableBullets[BulletType]);
                bullet.Center = Center;
                bullet.Damage = BulletDamage;
                bullet.Velocity = velocity;
                bullet.Owner = this;
                bullet.Level = Level;
                return bullet;
            }
            return null;
        }
    }
}
