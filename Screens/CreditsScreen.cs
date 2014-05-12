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
using Microsoft.Xna.Framework;
using TOBam.Framework;
#endregion

namespace TOBam.Screens
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class CreditsScreen : MenuScreen
    {
        #region Initialization
        public CreditsScreen() :this("")
        {
        }

        string message = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditsScreen(string message)
        {
            MenuEntries.Add("Programming: Rob Loach");
            MenuEntries.Add("Music: Anthony Garin");
            MenuEntries.Add("Graphics: Ari Feldman");
            MenuEntries.Add("Back");

            // Flag that there is no need for the game to transition
            // off when the pause menu is on top of it.
            IsPopup = false;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user menu selections.
        /// </summary>
        protected override void OnSelectEntry(int entryIndex)
        {
            LoadMainMenuScreen(null, null);
        }


        /// <summary>
        /// When the user cancels the pause menu, resume the game.
        /// </summary>
        protected override void OnCancel()
        {
            ExitScreen();
        }


        /// <summary>
        /// Loading screen callback for activating the main menu screen,
        /// used when quitting from the game.
        /// </summary>
        void LoadMainMenuScreen(object sender, EventArgs e)
        {
            ExitScreen();
            //ScreenManager.RemoveScreens();
            //ScreenManager.AddScreen(new MenuBackgroundScreen());
            //ScreenManager.AddScreen(new MainMenuScreen());
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the pause menu screen. This darkens down the gameplay screen
        /// that is underneath us, and then chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, message, new Vector2(100,100
            base.Draw(gameTime);
        }


        #endregion
    }
}

