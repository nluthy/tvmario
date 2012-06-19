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

        private int _type;

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }



        public Cell(ContentManager content, String strMap, Vector2 topleft, Vector2 size, int type)
        {
            TopLeft = topleft;
            Size = size;
            Texture2D[] textures = new Texture2D[1];
            textures[0] = content.Load<Texture2D>(strMap);
            Sprites = new List<My2DSprite>();
            My2DSprite temp = new My2DSprite(textures, TopLeft);
            temp.Size = this.Size;
            Sprites.Add(temp);
            SpritesCount = 1;
            _type = type;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {

            if (Size.X != 0)
            {
                Sprites[0].Position = TopLeft * Scale;
                Vector2 temp = Sprites[0].Size;
                temp.X *= Scale;
                temp.Y *= Scale;
                Sprites[0].Size = temp;
            }
            else
                Sprites[0].Position = TopLeft;
            base.Draw(spriteBatch, gameTime, color);
        }

    }
}
