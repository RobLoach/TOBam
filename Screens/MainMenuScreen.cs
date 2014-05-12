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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TOBam.Screens
{
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
        {
            MenuEntries.Add("Play Game");
            MenuEntries.Add("Options");
            MenuEntries.Add("Credits");
            MenuEntries.Add("Exit");
            
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
                    // Play the game.
                    //LoadingScreen.Load(ScreenManager, LoadGameplayScreen, true);
                    ScreenManager.AddScreen(new GameplayScreen());
                    break;

                case 1:
                    // Go to the options screen.
                    ScreenManager.AddScreen(new OptionsMenuScreen());
                    break;

                case 2:
                    ScreenManager.AddScreen(new CreditsScreen());
                    break;

                case 3:
                    // Exit the sample.
                    OnCancel();
                    break;
            }
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            //ScreenManager.Game.Exit();
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen messageBox = new MessageBoxScreen(message);

            messageBox.Accepted += ExitMessageBoxAccepted;

            ScreenManager.AddScreen(messageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        /// <summary>
        /// Loading screen callback for activating the gameplay screen.
        /// </summary>
        void LoadGameplayScreen(object sender, EventArgs e)
        {
            //ScreenManager.AddScreen(new GameplayScreen());
        }


        #endregion
    }
}
