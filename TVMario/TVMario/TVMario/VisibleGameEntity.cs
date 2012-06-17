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
    public abstract class VisibleGameEntity : GameEntity
    {
        protected List<My2DSprite> _sprites;




        protected int _spritesCount;

        public int SpritesCount
        {
            get { return _spritesCount; }
            set { _spritesCount = value; }
        }

        protected Vector2 _topLeft;

        public Vector2 TopLeft
        {
            get { return _topLeft; }
            set
            {
                _topLeft = value;
                foreach (My2DSprite sprite in _sprites)
                {
                    sprite.Position = _topLeft;
                }
            }
        }
        protected Vector2 _size;

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }


        public VisibleGameEntity()
        {
            _sprites = new List<My2DSprite>();
            SpritesCount = _sprites.Count;
        }





        virtual public void Update(GameTime gameTime)
        {
            foreach (My2DSprite sprite in _sprites)
            {
                sprite.Update(gameTime);
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            foreach (My2DSprite sprite in _sprites)
            {
                sprite.Draw(spriteBatch, gameTime, color);
            }
        }

        virtual public bool IsClicked(Vector2 point)
        {
            if (point.X >= TopLeft.X && point.X <= TopLeft.X + Size.X
                && point.Y >= TopLeft.Y && point.Y <= TopLeft.Y + Size.Y)
            {
                return true;
            }
            return false;

        }

    }
}
