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

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using TOBam.Framework;
using TOBam.Screens;
#endregion

namespace TOBam
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TOBamGame : Microsoft.Xna.Framework.Game
    {

        AudioEngine audio;
        public static SoundBank soundBank;
        WaveBank waveBank;

        public static Random Random = new Random();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TOBamGame game = new TOBamGame())
            {
                game.IsMouseVisible = false;
                game.IsFixedTimeStep = true;
                game.Run();
            }
        }

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            audio = new AudioEngine("Content/Audio/TOBam.xgs");
            waveBank = new WaveBank(audio, "Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audio, "Content/Audio/Sound Bank.xsb");
            TOBamGame.soundBank.PlayCue("required_ttc");
            base.LoadGraphicsContent(loadAllContent);
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            base.UnloadGraphicsContent(unloadAllContent);
        }

        public TOBamGame()
        {
            graphics = new GraphicsDeviceManager(this);
#if DEBUG
#else
            graphics.IsFullScreen = true;
#endif
            graphics.PreferredBackBufferWidth = ScreenManager.Width;
            graphics.PreferredBackBufferHeight = ScreenManager.Height;
            
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            screenManager.AddScreen(new MenuBackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }
    }
}
