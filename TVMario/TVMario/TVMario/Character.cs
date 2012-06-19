using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TVMario
{
    public class Character:VisibleGameEntity
    {
        public void Init(ContentManager content, String strPreTextures,
            int textureCount, Vector2 topLeft, Vector2 size)
        {
            SpritesCount = 1;
            TopLeft = topLeft;
            Size = size;
            String[] strTextures = new String[textureCount];
            for (int i = 0; i < textureCount; i++)
            {
                strTextures[i] = strPreTextures + (i + 1).ToString("00");
            }
            Texture2D[] textures = new Texture2D[textureCount];
            for (int i = 0; i < textureCount; i++)
            {
                textures[i] = content.Load<Texture2D>(strTextures[i]);
            }

            Sprites = new List<My2DSprite>();
            My2DSprite temp = new My2DSprite(textures, TopLeft);
            Sprites.Add(temp);
            SpritesCount = 1;
        }
    }
}
