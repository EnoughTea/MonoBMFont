using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBMFont {
    /// <summary> Extension methods for <see cref="SpriteBatch"/>. </summary>
    public static class SpriteBatchExtensions {
        /// <summary> Adds a string to a batch of sprites for rendering using the specified font, text, position,
        /// and color. </summary>
        /// <param name="spriteBatch">A sprite batch to use.</param>
        /// <param name="bmFont">A font for diplaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <exception cref="System.ArgumentNullException">text</exception>
        public static void DrawString(this SpriteBatch spriteBatch, BMFont bmFont, string text,
            Vector2 position, Color color) {
            if (bmFont == null) { throw new ArgumentNullException(nameof(bmFont)); }
            if (text == null) { throw new ArgumentNullException(nameof(text)); }

            DrawString(spriteBatch, bmFont, text, position, color, 0.0f, Vector2.Zero, new Vector2(1.0f),
                SpriteEffects.None, 0.0f);
        }

        /// <summary> Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteBatch">A sprite batch to use.</param>
        /// <param name="bmFont">A font for diplaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.
        /// </param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1
        /// represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <exception cref="System.ArgumentNullException">text</exception>
        public static void DrawString(this SpriteBatch spriteBatch, BMFont bmFont, string text,
            Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects,
            float layerDepth) {
            if (bmFont == null) { throw new ArgumentNullException(nameof(bmFont)); }
            if (text == null) { throw new ArgumentNullException(nameof(text)); }

            DrawString(spriteBatch, bmFont, text, position, color, rotation, origin, new Vector2(scale), effects,
                layerDepth);
        }

        /// <summary> Adds a string to a batch of sprites for rendering using the specified font, text, position,
        /// color, rotation, origin, scale, effects and layer. </summary>
        /// <param name="spriteBatch">A sprite batch to use.</param>
        /// <param name="bmFont">A font for diplaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.
        /// </param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1
        /// represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        public static void DrawString(this SpriteBatch spriteBatch, BMFont bmFont, string text,
            Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects,
            float layerDepth) {
            if (bmFont == null) { throw new ArgumentNullException(nameof(bmFont)); }
            if (text == null) { throw new ArgumentNullException(nameof(text)); }

            Matrix temp;
            Matrix transform;
            Matrix.CreateScale(scale.X, scale.Y, 1, out temp);
            Matrix.CreateRotationZ(rotation, out transform);
            Matrix.Multiply(ref temp, ref transform, out transform);

            bmFont.ProcessChars(text, (actual, drawPos, data, previous) => {
                var sourceRectangle = new Rectangle(data.X, data.Y, data.Width, data.Height);
                Vector2.Transform(ref drawPos, ref transform, out drawPos);
                var destRectangle = new Rectangle((int)(position.X + drawPos.X), (int)(position.Y + drawPos.Y),
                    (int)(data.Width * scale.X), (int)(data.Height * scale.Y));

                spriteBatch.Draw(bmFont.Texture, destRectangle, sourceRectangle, color, rotation, origin,
                   effects, layerDepth);
            }, null);
        }
    }
}
