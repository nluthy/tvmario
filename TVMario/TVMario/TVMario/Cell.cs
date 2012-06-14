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
    public class Cell : VisibleGameEntity
    {


        float _scale = 1.0f;

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }



        public Cell(ContentManager content, String strMap, Vector2 topleft, Vector2 size)
        {
            TopLeft = topleft;
            Size = size;
            Texture2D[] textures = new Texture2D[1];
            textures[0] = content.Load<Texture2D>(strMap);
            _sprites = new List<My2DSprite>();
            My2DSprite temp = new My2DSprite(textures, TopLeft);
            temp.Size = this.Size;
            _sprites.Add(temp);
            SpritesCount = 1;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {

            if (Size.X != 0)
            {
                _sprites[0].Position = -TopLeft * Scale;
                Vector2 temp = _sprites[0].Size;
                temp.X *= Scale;
                temp.Y *= Scale;
                _sprites[0].Size = temp;
            }
            else
                _sprites[0].Position = -TopLeft;
            base.Draw(gameTime, spriteBatch, color);
        }

    }
}
