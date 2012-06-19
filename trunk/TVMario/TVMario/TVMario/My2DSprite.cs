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
    public class My2DSprite
    {
        Vector2 _size;

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }
        Texture2D[] _textures;

        public Texture2D[] Textures
        {
            get { return _textures; }
            set { _textures = value; }
        }
        int _texturesCount;

        public int TexturesCount
        {
            get { return _texturesCount; }
            set { _texturesCount = value; }
        }
        int _currentTexture;

        public int CurrentTexture
        {
            get { return _currentTexture; }
            set { _currentTexture = value; }
        }
        Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }





        public float _normalDelay = 16.0f;

        public My2DSprite(Texture2D[] textures, Vector2 position)
        {
            Textures = textures;
            TexturesCount = Textures.Length;
            CurrentTexture = 0;
            Position = position;
            Width = Textures[0].Width;
            Height = Textures[0].Height;
        }

        public void Update(GameTime gameTime)
        {
            int delta = (int)(gameTime.TotalGameTime.Milliseconds / _normalDelay);
            CurrentTexture = delta % TexturesCount;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            Rectangle rect;
            if(Size.X == 0)
            rect= new Rectangle((int)Position.X, (int)Position.Y, Textures[0].Width, Textures[0].Height);
            else
                rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); ;
            spriteBatch.Draw(Textures[CurrentTexture], rect, color);
        }

        public void DrawFlipHorizontal(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            Rectangle rect;
            if (Size.X == 0)
                rect = new Rectangle((int)Position.X, (int)Position.Y, Textures[0].Width, Textures[0].Height);
            else
                rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); ;
            //spriteBatch.Draw(Textures[CurrentTexture], rect, color);
            spriteBatch.Draw(Textures[CurrentTexture], rect, null, color, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
        }

        //public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle rectangle)
        //{
        //    spriteBatch.Draw(_textures[_currentTexture], new Vector2(_position.X, _position.Y), rectangle, Color.White);
        //}


        public Rectangle GetBound()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Textures[0].Width, Textures[0].Height);
        }

        public bool CheckCollision(My2DSprite sprite)
        {
            return GetBound().Intersects(sprite.GetBound());
        }
        public bool CheckCollision(Rectangle rect)
        {
            return GetBound().Intersects(rect);
        }
    }
}
