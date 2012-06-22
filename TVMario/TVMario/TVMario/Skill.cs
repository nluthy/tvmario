﻿using System;
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
    public class Skill : VisibleGameEntity
    {
        private int _skillType;

        public int skillType
        {
            get { return _skillType; }
            set { _skillType = value; }
        }

        private int _maxLength;

        public int maxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
        private int _length;

        public int length
        {
            get { return _length; }
            set { _length = value; }
        }

        public void Init(ContentManager content, string strData)
        {
            XmlTextReader xml = new XmlTextReader(strData);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode skill = doc.GetElementsByTagName("skill")[0];
            string strPreTextures = skill["pretextures"].InnerText;
            string strSizeX = skill["size"]["x"].InnerText;
            string strSizeY = skill["size"]["y"].InnerText;
            float sizeX = float.Parse(strSizeX);
            float sizeY = float.Parse(strSizeY);
            int textureCount = Int32.Parse(skill["texturecount"].InnerText);
            Vector2 size = new Vector2(sizeX, sizeY);
            string strMaxLenght = skill["maxlength"].InnerText;
            int maxLength = Int32.Parse(strMaxLenght);
            xml.Close();
            this.Init(content, strPreTextures, textureCount, size, maxLength);
        }

        public void Init(ContentManager content, String strPreTextures,
            int textureCount, Vector2 size, int maxLength)
        {
            this.maxLength = maxLength;
            SpritesCount = 1;
            TopLeft = Vector2.Zero;
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

        public void Update(GameTime gameTime, bool isRight)
        {
            NextFrame();
            if (length < maxLength)
            {
                length++;
                Move(isRight);
            }
            else
            {
                length = 0;
                TopLeft = Vector2.Zero;
            }
            //base.Update(gameTime);
        }

        private void NextFrame()
        {
            if (this.Sprites[0].CurrentTexture < this.Sprites[0].TexturesCount - 1)
                this.Sprites[0].CurrentTexture++;
            else
                this.Sprites[0].CurrentTexture = 0;
        }

        public void Move(bool isRight)
        {
            Vector2 cur = TopLeft;
            if (isRight)
            {
                cur.X++;
            }
            else
            {
                cur.X--;
            }
            TopLeft = cur;
        }


    }
}
