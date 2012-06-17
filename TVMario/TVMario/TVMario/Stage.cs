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
    public class Stage : InvisibleGameEntity
    {
        private Texture2D _background;
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }
        MyMap map;
        Human human;
        List<Monster> monsters;

        public void Init(ContentManager content, string strBackground, string strHuman, string strMap, string strMonsters)
        {
            //Background = content.Load<Texture2D>(strBackground);
            human = new Human();
            human.Init(content, strHuman);
            
        }

        public void Update(GameTime gameTime)
        {
            human.Update(gameTime);
            //map.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            human.Draw(gameTime, spriteBatch, Color.White);
        }

    }
}
