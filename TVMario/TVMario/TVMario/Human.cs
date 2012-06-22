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
        private int _nLife;  // Số mạng
        private int _nCoin;  // Số xu
        private int _jumpHight; // Độ cao có thể nhảy đc hiện tại
        private int _jumpHightNow;  // Độ cao đã nhảy

        public int JumpHightNow
        {
            get { return _jumpHightNow; }
            set { _jumpHightNow = value; }
        }

        public int jumpHight
        {
            get { return _jumpHight; }
            set { _jumpHight = value; }
        }

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

        private List<int> _skillList;

        public List<int> skillList
        {
            get { return _skillList; }
            set { _skillList = value; }
        }

        private Vector2 _topLeftBegin;

        public Vector2 topLeftBegin
        {
            get { return _topLeftBegin; }
            set { _topLeftBegin = value; }
        }

        public Human(Human hm)
        {
            this.TopLeft = hm.TopLeft;
            this.Sprites = hm.Sprites;
        }

        public Human()
        {
            _skillList = new List<int>();
            _skillList.Add(1);
            _nLife = 5;
            _nCoin = 0;
            _jumpHight = 0;
            _jumpHightNow = 0;
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
            xml.Close();
            this.Init(content, strPreTextures, textureCount, topLeft, size);
            topLeftBegin = TopLeft;
            
        }

        int count = 0;
        public override void Update(GameTime gameTime)
        {

            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Right) || kbs.IsKeyDown(Keys.Left))
            {
                count++;
                if (count % 2 == 0)
                {
                    NextFrame();
                }
            }
            if (TopLeft.Y > 632)
            {
                _isDie = true;
            }

            if (_nCoin >= GlobalSetting.COIN_TO_LIFE)
            {
                _nCoin -= GlobalSetting.COIN_TO_LIFE;
                _nLife++;
            }


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
            _isDie = false;
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

        public bool CollisionWithMonster(Monster mt)
        {
            return Sprites[0].CheckCollision(mt.Sprites[0]);
        }

        public void NextFrame()
        {
            if (this.Sprites[0].CurrentTexture < this.Sprites[0].TexturesCount - 1)
                this.Sprites[0].CurrentTexture++;
            else
                this.Sprites[0].CurrentTexture = 0;
        }

        public bool HasSkill(int type)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i] == type)
                    return true;
            }
            return false;
        }
    }
}
