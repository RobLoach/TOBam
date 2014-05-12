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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using TOBam.Entities;

namespace TOBam.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class GameplayScreen : Screen
    {
        #region Fields

        ContentManager content;
        SpriteFont smallFont;

        public Player[] players = new Player[4];
        List<Texture2D> levelTextures = new List<Texture2D>();
        Dictionary<string, Texture2D> enemies = new Dictionary<string, Texture2D>();
        public Dictionary<string, Texture2D> BulletTextures = new Dictionary<string, Texture2D>();
        public Dictionary<string, Texture2D> PowerupTextures = new Dictionary<string, Texture2D>();
        public static Level Level;
        

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(2);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            TOBamGame.soundBank.PlayCue("flue");
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services);

                smallFont = content.Load<SpriteFont>("Content/Fonts/SmallFont");

                for (int i = 1; true; i++)
                {
                    try
                    {
                        levelTextures.Add(content.Load<Texture2D>("Content/Graphics/Level" + i.ToString()));
                    }
                    catch
                    {
                        break;
                    }
                }

                BulletTextures.Add("Bullet1", content.Load<Texture2D>("Content/Graphics/Bullet1"));
                BulletTextures.Add("Bullet2", content.Load<Texture2D>("Content/Graphics/Bullet2"));
                BulletTextures.Add("Bullet3", content.Load<Texture2D>("Content/Graphics/Bullet3"));
                BulletTextures.Add("Goat", content.Load<Texture2D>("Content/Graphics/required_goatonapole_good"));

                PowerupTextures.Add("PowerupA", content.Load<Texture2D>("Content/Graphics/PowerupA"));
                PowerupTextures.Add("PowerupB", content.Load<Texture2D>("Content/Graphics/PowerupB"));
                PowerupTextures.Add("PowerupE", content.Load<Texture2D>("Content/Graphics/PowerupE"));

                enemies.Add("SkiShooter", content.Load<Texture2D>("Content/Graphics/Enemy-SkiShooter"));
                enemies.Add("SmallPig", content.Load<Texture2D>("Content/Graphics/Enemy-SmallPig"));
                enemies.Add("BigGreen", content.Load<Texture2D>("Content/Graphics/Enemy-BigGreen"));
                enemies.Add("BigBi", content.Load<Texture2D>("Content/Graphics/Enemy-BigBi"));

                for (int i = 0; i < 4; i++)
                {
                    players[i] = new Player((PlayerIndex)i);
                    players[i].Texture = content.Load<Texture2D>("Content/Graphics/Player" + (i + 1).ToString());
                    players[i].Width = players[i].Texture.Width;
                    players[i].Height = players[i].Texture.Height;
                    players[i].Respawn();
                }

                players[0].keyStart = Keys.Enter;
                players[0].keyLeft = Keys.Left;
                players[0].keyRight = Keys.Right;
                players[0].keyUp = Keys.Up;
                players[0].keyDown = Keys.Down;
                players[0].keyShoot = Keys.Space;

                players[1].keyStart = Keys.Tab;
                players[1].keyLeft = Keys.A;
                players[1].keyRight = Keys.D;
                players[1].keyUp = Keys.W;
                players[1].keyDown = Keys.S;
                players[1].keyShoot = Keys.Tab;

                players[2].keyStart = Keys.NumPad0;
                players[2].keyLeft = Keys.NumPad4;
                players[2].keyRight = Keys.NumPad6;
                players[2].keyUp = Keys.NumPad8;
                players[2].keyDown = Keys.NumPad5;
                players[2].keyShoot = Keys.NumPad0;

                players[3].keyStart = Keys.U;
                players[3].keyLeft = Keys.J;
                players[3].keyRight = Keys.L;
                players[3].keyUp = Keys.I;
                players[3].keyDown = Keys.K;
                players[3].keyShoot = Keys.U;

                LoadLevel(1);
                foreach (Player player in players)
                    player.Level = Level;
                
                players[0].Lives = 3;
            }
        }

        public bool LoadLevel(int levelNum)
        {
            if (levelNum > levelTextures.Count)
                return false;
            else
            {
                Level = new Level(levelTextures[levelNum - 1], levelNum,enemies, BulletTextures, this);
                Level.Bottom = ScreenManager.GraphicsDevice.Viewport.Height;
                foreach (Player player in players)
                    player.Level = Level;
                foreach (Player player in players)
                    player.Respawn();
                return true;
            }
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
                content.Unload();
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            
            if (IsActive){

                for (int i = 0; i < 4; i++)
                {
                    if (players[i].Lives > 0)
                    {

                        players[i].Update(gameTime);
                    }
                }
                Level.Update(gameTime);
                if (Level.Top >= 0)
                {
                    if (LoadLevel(Level.Number + 1) == false)
                    {
                        EndGame();
                    }
                }
            }
        }
        public void EndGame()
        {
            int score = 0;
            int highscore = 0;
            foreach (Player player in players)
                score += player.Score;
#if XBOX360
#else
            StorageDevice device = StorageDevice.ShowStorageDeviceGuide();
            StorageContainer container = device.OpenContainer("TOBam");
            string filename = Path.Combine(container.Path, "highscore.txt");
            if (!File.Exists(filename))
                // Notify the user there is no save           
                highscore = 0;
            else
            {
                try
                {
                    highscore = int.Parse(File.ReadAllText(Path.Combine(container.Path, "highscore.txt")));
                }
                catch
                {
                    highscore = 0;
                }
            }

            //MessageBoxScreen win = new MessageBoxScreen("You Won!  Play Again?");
            //win.Accepted += new EventHandler<EventArgs>(win_Accepted);
            //win.Cancelled += new EventHandler<EventArgs>(win_Cancelled);

            if (score > highscore)
            {
                highscore = score;
                File.WriteAllText(Path.Combine(container.Path, "highscore.txt"), score.ToString());
            }
#endif

            PauseMenuScreen pause = new PauseMenuScreen("Score: " + score + "      Highscore: " + highscore.ToString());
            ScreenManager.AddScreen(pause);
        }

        void win_Cancelled(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreens();
            ScreenManager.AddScreen(new MenuBackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        void win_Accepted(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreens();
            ScreenManager.AddScreen(new GameplayScreen());
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input, GameTime gameTime)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.PauseGame)
            {
                // If they pressed pause, bring up the pause menu screen.
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            else
            {
                foreach (Player player in players)
                    player.HandleInput(gameTime);
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            ScreenManager.SpriteBatch.Begin();
            Level.Draw(ScreenManager.SpriteBatch);
            for (int i = 0; i < 4; i++)
            {
                players[i].Draw(ScreenManager.SpriteBatch);
                if (players[i].Lives == -1)
                {
                    ScreenManager.SpriteBatch.Draw(players[i].Texture, new Rectangle(160 * (i + 1), 5, (int)players[i].Width / 2, (int)players[i].Height / 2), Color.White);
                    ScreenManager.SpriteBatch.DrawString(smallFont, "Press Start!", new Vector2(160 * (i + 1) + (int)players[i].Width / 2, 5), Color.White);
                }
                else if (players[i].Lives > 0)
                {
                    ScreenManager.SpriteBatch.DrawString(smallFont, players[i].Score.ToString(), new Vector2(160 * (i + 1), 30), Color.White);
                    for (int nLives = 0; nLives < players[i].Lives; nLives++)
                        ScreenManager.SpriteBatch.Draw(players[i].Texture, new Rectangle(160 * (i + 1) + (int)(nLives * players[i].Width / 2), 5, (int)players[i].Width / 2, (int)players[i].Height / 2), Color.White);
                }
            }

            if (!IsActive)
                ScreenManager.SpriteBatch.Draw(ScreenManager.blankTexture, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), new Color(0, 0, 0, 150));


            ScreenManager.SpriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
