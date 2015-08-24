# HOWTO

		// Font loading, assuming 'assetName' is something like "Fonts\\Calibri_22_r.fnt":
        string fontTextureName = FontData.GetTextureNameForFont(assetName);
        var fontTexture = content.Load<Texture2D>(fontTextureName);
		var fontFilePath = Path.Combine(contentRootDirectory, assetName);

        using (var stream = TitleContainer.OpenStream(fontFilePath)) {
            var fontDesc = FontData.Load(stream);
            var font = new BMFont(fontTexture, fontDesc);
        }

		// Draw with extension method on SpriteBatch:
		spriteBatch.DrawString(bmFont, "Hello, world!", new Vector2(15, 50), Color.White);