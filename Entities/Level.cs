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
using TOBam.Screens;

namespace TOBam.Entities
{
    public class Level : Entity
    {
        public int Number = -1;
        public Texture2D Texture;
        public List<Powerup> Powerups = new List<Powerup>();
        public List<Enemy> Enemies = new List<Enemy>();
        public List<Bullet> Bullets = new List<Bullet>();
        public List<Bullet> PlayerBullets = new List<Bullet>();
        public Dictionary<string, Texture2D> AvailableEnemies;
        public Dictionary<string, Texture2D> AvailableBullets;
        public GameplayScreen Screen;
        float LevelDisplayTime = 1f;
        public Level(Texture2D levelBackground, int number, Dictionary<string, Texture2D> enemies, Dictionary<string, Texture2D> bullets, GameplayScreen screen)
        {
            Screen = screen;
            AvailableBullets = bullets;
            AvailableEnemies = enemies;
            Number = number;
            Texture = levelBackground;
            Size = new Vector2(levelBackground.Width, levelBackground.Height);
            Velocity.Y = 0.4f;
            TOBamGame.soundBank.PlayCue("required_ttc");
            TOBamGame.soundBank.PlayCue("flue");
            //Position.Y = 0;
        }
        public override bool Update(GameTime time)
        {
            if (TOBamGame.Random.NextDouble() < (float)Number / 50f)
            {
                Enemy enemy = null;
                switch (TOBamGame.Random.Next(1, Number+2))
                {
                    case 1:
                        enemy = new Enemy(AvailableEnemies["SmallPig"]);
                        enemy.Level = this;
                        enemy.Deploy();
                        enemy.Velocity.Y = 4.8f;
                        enemy.BulletSpeed = 10f;
                        enemy.BulletType = "Bullet1";
                        enemy.BulletDamage = 0.1f;
                        enemy.ReloadTime = 3200f;
                        enemy.Defense = 100f;
                        enemy.Velocity.X = (float)TOBamGame.Random.NextDouble() * 1.4f - 0.7f;
                        Enemies.Add(enemy);
                        break;
                    case 2:
                        enemy = new Enemy(AvailableEnemies["SkiShooter"]);
                        enemy.Level = this;
                        enemy.Deploy();
                        enemy.Velocity.Y = 1.6f;
                        enemy.BulletSpeed = 4.5f;
                        enemy.BulletType = "Bullet2";
                        enemy.BulletDamage = 0.3f;
                        enemy.ReloadTime = 1500f;
                        enemy.Defense = 1f;
                        enemy.Velocity.X = (float)TOBamGame.Random.NextDouble() * 1f - 0.5f;
                        Enemies.Add(enemy);
                        break;
                    case 3:
                        enemy = new Enemy(AvailableEnemies["BigGreen"]);
                        enemy.Level = this;
                        enemy.Deploy();
                        enemy.Velocity.Y = 2f;
                        enemy.BulletSpeed = 3.7f;
                        enemy.BulletType = "Goat";
                        enemy.BulletDamage = 0.7f;
                        enemy.ReloadTime = 1000f;
                        enemy.Defense = 1.6f;
                        enemy.Velocity.X = (float)TOBamGame.Random.NextDouble() * 1f - 0.5f;
                        Enemies.Add(enemy);
                        break;
                    case 4:
                        enemy = new Enemy(AvailableEnemies["BigBi"]);
                        enemy.Level = this;
                        enemy.Deploy();
                        enemy.Velocity.Y = 0.8f;
                        enemy.BulletSpeed = 3.5f;
                        enemy.BulletType = "Bullet3";
                        enemy.BulletDamage = 0.6f;
                        enemy.ReloadTime = 1000f;
                        enemy.Defense = 0.6f;
                        enemy.Velocity.X = (float)TOBamGame.Random.NextDouble() * 0.6f - 0.3f;
                        Enemies.Add(enemy);
                        break;

                }
            }
            
                if (LevelDisplayTime > 0f)
                    LevelDisplayTime -= 0.005f;

            //enemies
            List<Bullet> removedBullets = new List<Bullet>();
            List<Bullet> removedPlayerBullets = new List<Bullet>();
            List<Enemy> removedEnemies = new List<Enemy>();
            List<Powerup> removedPowerups = new List<Powerup>();

            foreach (Powerup powerup in Powerups)
            {
                if (powerup.Update(time) == false)
                {
                    removedPowerups.Add(powerup);
                    continue;
                }

                foreach (Player player in Screen.players)
                {
                    if (player.Lives <= 0) continue;
                    if (powerup.CheckCollision(player))
                    {
                        switch (powerup.Power)
                        {
                            case PowerupType.FullHealth:
                                player.Health = 1f;
                                break;
                            case PowerupType.Bomb:
                                Enemies.Clear();
                                break;
                            case PowerupType.Attack:
                                player.BulletDamage = 1f;
                                player.BulletType = "Bullet2";
                                break;
                        }
                        removedPowerups.Add(powerup);
                        break;
                    }
                }
            }

            foreach (Bullet bullet in PlayerBullets)
            {
                if (bullet.Update(time) == false)
                {
                    removedPlayerBullets.Add(bullet);
                    continue;
                }
            }
            foreach (Enemy enemy in Enemies)
            {
                if (enemy.Update(time) == false)
                {
                    removedEnemies.Add(enemy);
                    continue;
                }

                foreach (Bullet bullet in PlayerBullets)
                {
                    if(bullet.CheckCollision(enemy))
                    {
                        removedPlayerBullets.Add(bullet);
                        Player player = (Player)bullet.Owner;
                        player.Score += (int)(bullet.Damage * 100);
                        if(player.Score > player.NextPowerupScore)
                        {
                            player.NextPowerupScore += 2000;
                            switch((PowerupType)TOBamGame.Random.Next(0,3))
                            {
                                case PowerupType.FullHealth:
                                    Powerups.Add(new Powerup(Screen.PowerupTextures["PowerupA"], PowerupType.FullHealth));
                                    break;
                                case PowerupType.Bomb:
                                    Powerups.Add(new Powerup(Screen.PowerupTextures["PowerupB"], PowerupType.Bomb));
                                    break;
                                default:
                                    Powerups.Add(new Powerup(Screen.PowerupTextures["PowerupE"], PowerupType.Attack));
                                    break;
                            }
                        }
                        if (enemy.GetHit(bullet.Damage))
                        {
                            removedEnemies.Add(enemy);
                            ((Player)bullet.Owner).Score += (int)(bullet.Damage * 500);
                            continue;
                        }
                    }
                }

                foreach (Player player in Screen.players)
                {
                    if (player.Lives <= 0) continue;
                    if (enemy.CheckCollision(player))
                    {
                        player.GetHit(0.08f);
                        if (enemy.GetHit(0.1f))
                        {
                            removedEnemies.Add(enemy);
                            continue;
                        }
                    }
                }
            }

            //bullets
            foreach (Bullet bullet in Bullets)
            {
                if (bullet.Update(time) == false)
                {
                    removedBullets.Add(bullet);
                    continue;
                }

                foreach (Player player in Screen.players)
                {
                    if (player.Lives <= 0) continue;
                    if (bullet.CheckCollision(player))
                    {
                        removedBullets.Add(bullet);
                        player.GetHit(bullet.Damage);
                        break;
                    }
                }
            }
            foreach (Enemy enemy in removedEnemies)
                Enemies.Remove(enemy);
            foreach (Bullet bullet in removedBullets)
                Bullets.Remove(bullet);
            foreach (Bullet bullet in removedPlayerBullets)
                PlayerBullets.Remove(bullet);
            foreach (Powerup powerup in removedPowerups)
                Powerups.Remove(powerup);

            return base.Update(time);
        }
        public void Draw(SpriteBatch sprites)
        {
            //sprites.Draw(Texture, new Vector2(0, 0),
            //    new Rectangle(0, (int)Position.Y + Texture.Height - Screen.ScreenManager.GraphicsDevice.Viewport.Height, Screen.ScreenManager.GraphicsDevice.Viewport.Width, Screen.ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            sprites.Draw(Texture, Rectangle, Color.White);
            foreach (Bullet bullet in Bullets)
                bullet.Draw(sprites);
            foreach (Bullet bullet in PlayerBullets)
                bullet.Draw(sprites);
            foreach (Powerup powerup in Powerups)
                powerup.Draw(sprites);
            foreach (Enemy enemy in Enemies)
                enemy.Draw(sprites);

            if (LevelDisplayTime > 0f)
            {
                string text = "Level " + Number.ToString();
                Vector2 textSize = Screen.ScreenManager.Font.MeasureString(text);
                Screen.ScreenManager.SpriteBatch.DrawString(Screen.ScreenManager.Font, text, new Vector2(Screen.ScreenManager.GraphicsDevice.Viewport.Width / 2f - textSize.X / 2f + 10, Screen.ScreenManager.GraphicsDevice.Viewport.Height / 2f + 10), new Color(0, 0, 0, (byte)(LevelDisplayTime * 255f)));
                Screen.ScreenManager.SpriteBatch.DrawString(Screen.ScreenManager.Font, text, new Vector2(Screen.ScreenManager.GraphicsDevice.Viewport.Width / 2f - textSize.X / 2f, Screen.ScreenManager.GraphicsDevice.Viewport.Height / 2f), new Color(255, 255, 255, (byte)(LevelDisplayTime * 255f)));
            }
        }
    }
}
