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
using TOBam.Framework;
using TOBam.Entities;

namespace TOBam.Framework
{
    public class Entity
    {
        public Entity(Vector2 pos, Vector2 vel, Vector2 accel)
        {
            Position = pos;
            Velocity = vel;
            Acceleration = accel;
        }
        public Entity(Vector2 pos, Vector2 vel) : this(pos, vel, new Vector2()) { }
        public Entity(Vector2 pos) : this(pos, new Vector2()) { }
        public Entity() : this(new Vector2()) { }
        public Level Level;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Size;
        public float Rotation = 0f;
        public Vector2 Friction = new Vector2(1f, 1f);

        public bool CheckCollision(Entity entity)
        {
            if (Left < entity.Right)
                if (Top < entity.Bottom)
                    if (Bottom > entity.Top)
                        if (Right > entity.Left)
                            return true;
            return false;
        }
        public virtual bool Update(GameTime time)
        {
            Velocity.X += Acceleration.X;// *(float)time.ElapsedGameTime.Ticks;
            Velocity.Y += Acceleration.Y;// *(float)time.ElapsedGameTime.Ticks;
            Velocity.X *= Friction.X;
            Velocity.Y *= Friction.Y;
            //System.Diagnostics.Debug.WriteLine(Acceleration.X.ToString());
            Position.X += Velocity.X;// *(float)time.ElapsedGameTime.Ticks;
            Position.Y += Velocity.Y;// *(float)time.ElapsedGameTime.Ticks;
            return true;
        }
        public float Width
        {
            get
            {
                return Size.X;
            }
            set
            {
                Size.X = value;
            }
        }
        public float Height
        {
            get
            {
                return Size.Y;
            }
            set
            {
                Size.Y = value;
            }
        }
        public float Right
        {
            get
            {
                return Position.X + Size.X;
            }
            set
            {
                Position.X = value - Size.X;
            }
        }
        public float Left
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }
        public float Top
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }
        public float Bottom
        {
            get
            {
                return Position.Y + Size.Y;
            }
            set
            {
                Position.Y = value - Size.Y;
            }
        }
        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X + Size.X / 2f, Position.Y + Size.Y / 2f);
            }
            set
            {
                Position.X = value.X - Size.X / 2f;
                Position.Y = value.Y - Size.Y / 2f;
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public virtual Entity Clone()
        {
            Entity enemy = new Entity();
            enemy.Acceleration = Acceleration;
            enemy.Friction = Friction;
            enemy.Position = Position;
            enemy.Rotation = Rotation;
            enemy.Size = Size;
            enemy.Velocity = Velocity;
            return enemy;
        }
    }
}
