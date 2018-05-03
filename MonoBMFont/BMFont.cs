using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBMFont {
    /// <summary> Represents a sprite font based on BMFont tool: http://www.angelcode.com/products/bmfont/. </summary>
    /// <remarks> Supports only single texture fonts. </remarks>
    public sealed class BMFont {
        /// <summary> File extension used by BMFont tool. </summary>
        public const string Extension = ".fnt";

        private Dictionary<char, Dictionary<char, int>> _kerningMap;
        private int _lineSpacing;

        /// <summary> Initializes a new instance of the <see cref="BMFont" /> class. </summary>
        /// <param name="texture">Font texture.</param>
        /// <param name="fontDesc">BMFont description.</param>
        /// <param name="spacing">Text spacing.</param>
        /// <param name="defaultCharacter">CSharacter optionally used for resolving unknown characters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="texture"/> is <see langword="null"/>
        /// -or- <paramref name="fontDesc"/> is <see langword="null"/></exception>
        public BMFont(Texture2D texture, FontData fontDesc, float spacing = 0,
            char? defaultCharacter = ' ') {
            if (texture == null) {
                throw new ArgumentNullException(nameof(texture));
            }

            if (fontDesc == null) {
                throw new ArgumentNullException(nameof(fontDesc));
            }

            Init(texture, fontDesc, spacing, defaultCharacter);
        }

        /// <summary> Gets the characters supported by the font and their font data. </summary>
        public ReadOnlyDictionary<char, FontGlyph> Characters { get; private set; }

        /// <summary> Gets the BMFont description. </summary>
        public FontData Data { get; private set; }

        /// <summary> Gets or sets the default character. </summary>
        public char? DefaultCharacter { get; set; }

        /// <summary>
        ///     Gets or sets the vertical distance (in pixels) between the base lines of two consecutive
        ///     lines of text. Line spacing includes the blank space between lines as well as the height of the characters.
        /// </summary>
        public int LineSpacing { get { return _lineSpacing; } set { _lineSpacing = Math.Abs(value); } }

        /// <summary>Gets or sets the spacing of the font characters. </summary>
        public float Spacing { get; set; }

        /// <summary> Gets the texture map for the font. </summary>
        public Texture2D Texture { get; private set; }

        /// <summary> Gets the kerning amount between two font characters. </summary>
        /// <param name="first">The first characters.</param>
        /// <param name="second">The second characters.</param>
        /// <returns>Kerning amount in pixels.</returns>
        public int GetKerning(char first, char second) {
            var kerning = 0;
            Dictionary<char, int> firstKerningPairs;
            if (_kerningMap.TryGetValue(first, out firstKerningPairs)) {
                firstKerningPairs.TryGetValue(second, out kerning);
            }

            return kerning;
        }

        /// <summary> Returns the width and height of a string. </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The width and height, in pixels, of text, when it is rendered. </returns>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null"/></exception>
        public Vector2 MeasureString(string text) {
            if (text == null) {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length == 0) {
                return Vector2.Zero;
            }

            return ProcessChars(text, null, null);
        }

        /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null"/></exception>
        internal Vector2 ProcessChars(string text, EachChar eachChar, Action newLineBegins) {
            if (text == null) {
                throw new ArgumentNullException(nameof(text));
            }

            var currentX = 0;
            var currentY = 0;
            var maxLineWidth = 0;
            var maxCharHeight = LineSpacing;
            char? previous = null;

            foreach (var c in text) {
                if (c == '\r') {
                    continue;
                }
                if (c == '\n') {
                    currentX -= (int)Spacing;
                    maxLineWidth = Math.Max(maxLineWidth, currentX);
                    currentX = 0;
                    currentY += maxCharHeight;
                    maxCharHeight = LineSpacing;
                    newLineBegins?.Invoke();
                    continue;
                }

                var actual = c;
                var charData = Characters.GetValue(actual);
                if (charData == null) {
                    if (!DefaultCharacter.HasValue) {
                        throw new ArgumentException("Text contains characters that cannot be resolved by this font.",
                            nameof(text));
                    }

                    charData = Characters.GetValue(DefaultCharacter.Value);
                    actual = DefaultCharacter.Value;
                }

                var kerning = 0;
                if (previous.HasValue) {
                    kerning = GetKerning(previous.Value, c);
                }

                var position = new Vector2(currentX + charData.XOffset + kerning, currentY + charData.YOffset);
                eachChar?.Invoke(actual, position, charData, previous);

                previous = actual;
                currentX += charData.XAdvance + kerning + (int)Spacing;
                if (charData.Height > maxCharHeight) {
                    maxCharHeight = charData.Height;
                }
            }

            currentY += maxCharHeight;
            return new Vector2(Math.Max(maxLineWidth, currentX), currentY);
        }

        private void Init(Texture2D texture, FontData fontDesc, float spacing, char? defaultCharacter) {
            if (texture == null) {
                throw new ArgumentNullException(nameof(texture));
            }

            if (fontDesc == null) {
                throw new ArgumentNullException(nameof(fontDesc));
            }

            var characterMap = new Dictionary<char, FontGlyph>(fontDesc.Chars.Count);
            foreach (var fontCharacter in fontDesc.Chars) {
                var c = (char)fontCharacter.Id;
                characterMap.Add(c, fontCharacter);
            }

            _kerningMap = fontDesc.GenerateKerningMap();

            Characters = new ReadOnlyDictionary<char, FontGlyph>(characterMap);
            Data = fontDesc;
            DefaultCharacter = defaultCharacter;
            if (DefaultCharacter.HasValue && !characterMap.ContainsKey(DefaultCharacter.Value)) {
                throw new ArgumentException("Given default character does not exist in the font.",
                    nameof(defaultCharacter));
            }

            Spacing = spacing;
            Texture = texture;
            LineSpacing = fontDesc.Common.LineHeight;
        }

        internal delegate void EachChar(char actual, Vector2 drawPosition, FontGlyph data, char? previous);
    }
}