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
using TOBam.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
     
namespace TOBam.Entities
{
    public enum PowerupType
    {
        FullHealth, Bomb, Attack
    }

    public class Powerup : Entity
    {
        public Texture2D Texture;
        public PowerupType Power = PowerupType.FullHealth;
        public Powerup(Texture2D texture, PowerupType type)
        {
            this.Texture = texture;
            Position = new Vector2(TOBamGame.Random.Next(0, ScreenManager.Width), -texture.Height + 1);
            Velocity.Y = 1.2f;
            Size = new Vector2(Texture.Width, Texture.Height);
            Power = type;
        }
        public void Draw(SpriteBatch sprites)
        {
            sprites.Draw(Texture, Position, Color.White);
        }
        public override bool Update(GameTime time)
        {
            if (Top > ScreenManager.Height) return false;
            if (Bottom < 0) return false;
            if (Left > ScreenManager.Width) return false;
            if (Right < 0) return false;
            return base.Update(time);
        }
    }
}
