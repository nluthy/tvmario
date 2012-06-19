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
    public class Human : Character
    {

        public bool _isRight = true;   //  Đang xoay về bên phải
        public bool _isJumping = false;   //  Đang nhảy
        public bool _isDie = false;       //  Chết chưa
        private int _nLife = 0;  // Số mạng
        private int _nCoin = 0;  // Số xu

        public int nCoin
        {
            get { return _nCoin; }
            set { _nCoin = value; }
        }

        public int nLife
        {
            get { return _nLife; }
            set { _nLife = value; }
        }

        public void Init(ContentManager content, string strXML)
        {
            XmlTextReader xml = new XmlTextReader(strXML);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode human = doc.GetElementsByTagName("human")[0];
            string strPreTextures = human["pretextures"].InnerText;
            string strTopLeftX = human["topleft"]["x"].InnerText;
            string strTopLeftY = human["topleft"]["y"].InnerText;
            string strSizeX = human["size"]["x"].InnerText;
            string strSizeY = human["size"]["y"].InnerText;
            float topLeftX = float.Parse(strTopLeftX);
            float topLeftY = float.Parse(strTopLeftY);
            float sizeX = float.Parse(strSizeX);
            float sizeY = float.Parse(strSizeY);
            int textureCount = Int32.Parse(human["texturecount"].InnerText);
            Vector2 topLeft = new Vector2(topLeftX, topLeftY);
            Vector2 size = new Vector2(sizeX, sizeY);
            string strLife = human["life"].InnerText;
            int iLife = Int32.Parse(strLife); 
            xml.Close();
            this.Init(content, strPreTextures, textureCount, topLeft, size);
            this._nLife = iLife;
        }

        public override void Update(GameTime gameTime)
        {

            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Right) || kbs.IsKeyDown(Keys.Left))
            {
                if (this.Sprites[0].CurrentTexture < this.Sprites[0].TexturesCount - 1)
                    this.Sprites[0].CurrentTexture++;
                else
                    this.Sprites[0].CurrentTexture = 0;
            }
            if (TopLeft.Y > 632)
            {
                _isDie = true;
            }

            if (_nCoin == GlobalSetting.COIN_TO_LIFE)
            {
                _nCoin = 0;
                _nLife++;
            }


        }

        public bool onTheGround()
        {
            return false;
        }

        public void Jump(int value)
        {
            Sprites[0].CurrentTexture = GlobalSetting.INDEX_TEXTURE_JUMP;
            Vector2 cur = TopLeft;
            cur.Y -= value;
            TopLeft = cur;
        }

        public void Run(int value)
        {
            Vector2 cur = TopLeft;
            cur.X += value;
            TopLeft = cur;
        }

        public void Fall(int value)
        {
            Vector2 cur = TopLeft;
            cur.Y += value;
            TopLeft = cur;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            if (_isRight)
                base.Draw(spriteBatch, gameTime, color);
            else
            {
                foreach (My2DSprite sprite in Sprites)
                {
                    sprite.DrawFlipHorizontal(spriteBatch, gameTime, color);
                }
            }
        }

        public bool Die()
        {
            if (_nLife > 0)
            {
                _nLife--;
                return false;
            }
            return true;
        }

        public bool CollisionWithCell(Cell cell)
        {
            return Sprites[0].CheckCollision(cell.Sprites[0]);
        }
    }
}
