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
    public class Stage : InvisibleGameEntity
    {
        private Texture2D _background;
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }
        MapWithCells map;
        Human human;
        List<Monster> monsters;

        public Stage(ContentManager content, string strData)
        {
            string strBackground, strHuman, strMap, strMonsters;
            XmlTextReader xml = new XmlTextReader(strData);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode stage = doc.GetElementsByTagName("stage")[0];
            strBackground = stage["background"].InnerText;
            strHuman = stage["human"].InnerText;
            strMap = stage["map"].InnerText;
            strMonsters = stage["monsters"].InnerText;
            xml.Close();
            Init(content, strBackground, strHuman, strMap, strMonsters);
        }

        public void Init(ContentManager content, string strBackground, string strHuman, string strMap, string strMonsters)
        {
            Background = content.Load<Texture2D>(strBackground);
            human = new Human();
            human.Init(content, strHuman);
            map = new MapWithCells(content, strMap);
            
        }

        public void Update(GameTime gameTime)
        {
            human.Update(gameTime);
            map.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Background != null)
                spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            human.Draw(spriteBatch, gameTime, Color.White);
            map.Draw(spriteBatch, gameTime, Color.White);
        }

    }
}
