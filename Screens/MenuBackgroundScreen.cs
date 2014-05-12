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
using Microsoft.Xna.Framework.Content;
using TOBam.Framework;
using Microsoft.Xna.Framework.Audio;

namespace TOBam.Screens
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class MenuBackgroundScreen : Screen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;
        SpriteFont titleFont;
        Texture2D titleImage;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuBackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the ScreenManager, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services);

                backgroundTexture = content.Load<Texture2D>("Content/Graphics/Level1");
                titleFont = content.Load<SpriteFont>("Content/Fonts/SmallFont");
                titleImage = content.Load<Texture2D>("Content/Graphics/Title");
            }
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
                content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            byte fade = TransitionAlpha;

            ScreenManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen,
                                           new Color(fade, fade, fade));
            //ScreenManager.SpriteBatch.DrawString(titleFont, "TOBam!", new Vector2(100, 100), new Color(fade, fade, fade));

            string message = "Copyright Rob Loach 2007 - TOJam";
            Vector2 size = titleFont.MeasureString(message);
            base.ScreenManager.SpriteBatch.DrawString(titleFont, message, new Microsoft.Xna.Framework.Vector2(ScreenManager.GraphicsDevice.Viewport.Width - size.X - 20, ScreenManager.GraphicsDevice.Viewport.Height - size.Y), Color.White);

            ScreenManager.SpriteBatch.Draw(titleImage, new Vector2(410, 110), null, Color.Black, 0, new Vector2(titleImage.Width / 2f, titleImage.Height / 2f), 2f, SpriteEffects.None, 0f);
            ScreenManager.SpriteBatch.Draw(titleImage, new Vector2(400, 100), null, Color.LightCoral, 0, new Vector2(titleImage.Width / 2f, titleImage.Height / 2f), 2f, SpriteEffects.None, 0f);


            ScreenManager.SpriteBatch.End();
        }


        #endregion
    }
}
