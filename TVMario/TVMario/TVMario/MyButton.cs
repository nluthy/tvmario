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
    public class MyButton : VisibleGameEntity
    {
        bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }
        String _text;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        SpriteFont _font;

        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }


        Color _color;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public MyButton(SpriteFont font, String text, ContentManager content, String[] strTextures,
            int textureCount, Vector2 topLeft, Vector2 size)
        {

            InitMyButton(font, text, content, strTextures, textureCount, ref topLeft, ref size);
        }

        private void InitMyButton(SpriteFont font, String text, ContentManager content, String[] strTextures, int textureCount, ref Vector2 topLeft, ref Vector2 size)
        {
            TopLeft = topLeft;
            Size = size;
            Text = text;
            Font = font;

            Texture2D[] textures = new Texture2D[textureCount];
            for (int i = 0; i < textureCount; i++)
            {
                textures[i] = content.Load<Texture2D>(strTextures[i]);
            }

            _sprites = new List<My2DSprite>();
            My2DSprite temp = new My2DSprite(textures, TopLeft);
            _sprites.Add(temp);
            SpritesCount = 1;
            _color = Color.White;
            IsSelected = false;
        }

        public MyButton(SpriteFont font, String text, ContentManager content, String strPreTextures,
            int textureCount, Vector2 topLeft, Vector2 size)
        {
            String[] strTextures = new String[textureCount];
            for (int i = 0; i < textureCount; i++)
            {
                strTextures[i] = strPreTextures + (i + 1).ToString("00");
            }

            InitMyButton(font, text, content, strTextures, textureCount, ref topLeft, ref size);
        }

     



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {

            base.Draw(gameTime, spriteBatch, color);
            Vector2 _textPos = TopLeft;
            if (Size.X == 0)
            {
                Vector2 temp = new Vector2();
                temp.X = _sprites[0].Width;
                temp.Y = _sprites[0].Height;
                Size = temp;
            }
            _textPos.X += Size.X / 2;
            _textPos.Y += Size.Y / 2;
            Vector2 _textOrigin = Font.MeasureString(Text) / 2;
            spriteBatch.DrawString(Font, Text, _textPos, Color.Black, 0f, _textOrigin, 1.1f, SpriteEffects.None, 1f);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            base.Draw(gameTime, spriteBatch, _color);
            Vector2 _textPos = TopLeft;
            if (Size.X == 0)
            {
                Vector2 temp = new Vector2();
                temp.X = _sprites[0].Width;
                temp.Y = _sprites[0].Height;
                Size = temp;
            }
            _textPos.X += Size.X / 2;
            _textPos.Y += Size.Y / 2;
            Vector2 _textOrigin = Font.MeasureString(Text) / 2;
            spriteBatch.DrawString(Font, Text, _textPos, Color.Black, 0f, _textOrigin, 1.1f, SpriteEffects.None, 1f);
        }
    }
}
