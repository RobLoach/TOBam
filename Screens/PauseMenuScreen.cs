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
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization
        public PauseMenuScreen() :this("")
        {
        }

        string message = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(string message)
        {
            this.message = message;
            if(this.message != string.Empty)
                MenuEntries.Add(message);
            else
                MenuEntries.Add("Resume");
            MenuEntries.Add("Main Menu");

            // Flag that there is no need for the game to transition
            // off when the pause menu is on top of it.
            IsPopup = true;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user menu selections.
        /// </summary>
        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0:
                    // Resume the game.
                    if (MenuEntries[0] == "Resume")
                        ExitScreen();
                    else
                        ScreenManager.RemoveScreens();
                        ScreenManager.AddScreen(new GameplayScreen());
                    break;

                case 1:
                    if (MenuEntries[0] == "Resume")
                    {
                        // Quit the game, after a confirmation message box.
                        const string message = "Are you sure you want to quit?";

                        MessageBoxScreen messageBox = new MessageBoxScreen(message);

                        messageBox.Accepted += LoadMainMenuScreen;

                        ScreenManager.AddScreen(messageBox);
                    }
                    else
                        LoadMainMenuScreen(null, null);
                    break;
            }
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
            ScreenManager.RemoveScreens();
            ScreenManager.AddScreen(new MenuBackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
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
