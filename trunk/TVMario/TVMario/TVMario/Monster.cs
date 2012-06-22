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
using System.Xml;

namespace TVMario
{
    public class Monster : Character
    {
        private bool _isDie;

        public bool isDie
        {
            get { return _isDie; }
            set { _isDie = value; }
        }

        private bool _isRight;

        public bool isRight
        {
            get { return _isRight; }
            set { _isRight = value; }
        }

        public Monster()
        {
            isDie = false;
            isRight = false;
        }

        public void Init(ContentManager content, Vector2 topLeft, string strData)
        {
            XmlTextReader xml = new XmlTextReader(strData);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode monster = doc.GetElementsByTagName("monster")[0];
            string strPreTextures = monster["pretextures"].InnerText;
            string strSizeX = monster["size"]["x"].InnerText;
            string strSizeY = monster["size"]["y"].InnerText;
            float sizeX = float.Parse(strSizeX);
            float sizeY = float.Parse(strSizeY);
            int textureCount = Int32.Parse(monster["texturecount"].InnerText);
            Vector2 size = new Vector2(sizeX, sizeY);
            xml.Close();
            this.Init(content, strPreTextures, textureCount, topLeft, size);
        }

        int count = 0;
        public override void Update(GameTime gameTime)
        {
            count++;
            if (count % 8 == 0)
            {
                NextFrame();
            }

            if (!isDie)
            {
                Move();
            }
            //base.Update(gameTime);
        }

        public void Move()
        {
            if (isRight)
            {
                Vector2 cur = TopLeft;
                cur.X += GlobalSetting.MONSTER_STEP;
                TopLeft = cur;
            }
            else
            {
                Vector2 cur = TopLeft;
                cur.X -= GlobalSetting.MONSTER_STEP;
                TopLeft = cur;
            }
        }

        private void NextFrame()
        {
            if (Sprites[0].CurrentTexture == Sprites[0].TexturesCount - 2)
            {
                Sprites[0].CurrentTexture = 0;
            }
            else
            {
                if (Sprites[0].CurrentTexture < Sprites[0].TexturesCount - 2)
                {
                    Sprites[0].CurrentTexture++;
                }
            }
        }

        public void ChangeDirection()
        {
            if (isRight)
                isRight = false;
            else isRight = true;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            if (!isRight)
                base.Draw(spriteBatch, gameTime, color);
            else
            {
                foreach (My2DSprite sp in Sprites)
                {
                    sp.DrawFlipHorizontal(spriteBatch, gameTime, color);
                }
            }
        }
    }
}
