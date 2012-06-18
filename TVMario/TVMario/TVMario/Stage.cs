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
            KeyboardState kbs = Keyboard.GetState();
            human.Update(gameTime);
            map.Update(gameTime);
            if (!HumanIsOnTheGround(human) && !human.isJumping)
            {
                HumanFall();
            }
            if (kbs.IsKeyDown(Keys.Right))
            {
                if (human.isRight)
                    HumanRun();
                else
                    human.isRight = true;
            }
            if (kbs.IsKeyDown(Keys.Left))
            {
                if (!human.isRight)
                    HumanBack();
                else
                    human.isRight = false;
            }
            if (kbs.IsKeyDown(Keys.Space))
            {
                if (HumanIsOnTheGround(human))
                {
                    human.isJumping = true;
                    HumanJump();
                }
            }

        }

        private bool HumanIsOnTheGround(Human hm)
        {
            if (hm.TopLeft.Y <= 631)
            {
                int y = (int)hm.TopLeft.Y + 40;
                int x1 = (int)hm.TopLeft.X + 25;
                int x2 = (int)hm.TopLeft.X + 30;
                x1 -= (int)map.TopLeft.X;
                x2 -= (int)map.TopLeft.X;
                for (int x = x1 - 24; x <= x2; x++)
                {
                    int row = y / map.CELL_HEIGHT;
                    int col = x / map.CELL_WIDTH;
                    int cell = map.iCells[row, col];
                    if (cell != 0 && cell != 8 && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return true;
                }
            }
            return false;
        }

        private bool HumanCanRun(Human hm)
        {
            if (hm.TopLeft.Y <= 631)
            {
                int x = (int)hm.TopLeft.X + 35;
                x -= (int)map.TopLeft.X;
                int y1 = (int)hm.TopLeft.Y+1;
                int y2 = (int)hm.TopLeft.Y + 39;
                for (int y = y1; y <= y2; y++)
                {
                    int row = y / map.CELL_HEIGHT;
                    int col = x / map.CELL_WIDTH;
                    int cell = map.iCells[row, col];
                    if (cell != 0 && cell != 8 && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return false;
                }
            }
            return true;
        }

        private bool HumanCanBack(Human hm)
        {
            if (hm.TopLeft.Y <= 631)
            {
                int x = (int)hm.TopLeft.X-1;
                x -= (int)map.TopLeft.X;
                int y1 = (int)hm.TopLeft.Y+1;
                int y2 = (int)hm.TopLeft.Y + 39;
                for (int y = y1; y <= y2; y++)
                {
                    int row = y / map.CELL_HEIGHT;
                    int col = x / map.CELL_WIDTH;
                    int cell = map.iCells[row, col];
                    if (cell != 0 && cell != 8 && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return false;
                }
            }
            return true;
        }


        public void HumanFall()
        {
            Human hm = human;
            for (int i = 0; i < GlobalSetting.GRAVITY; i++)
            {
                hm.Fall(1);
                if (HumanIsOnTheGround(hm))
                {
                    human.Fall(i);
                    return;
                }
            }
            human.Fall(GlobalSetting.GRAVITY);
        }

        public void HumanRun()
        {
            if (HumanCanRun(human))
            {
                map.MoveLeft();
                if (human.TopLeft.X < 492)
                {
                    human.Run(1);
                }
            }
        }

        public void HumanBack()
        {
            if (HumanCanBack(human))
            {
                map.MoveRight();
            }
        }

        private bool HumanCanJump(Human hm)
        {
            if (hm.TopLeft.Y <= 631)
            {
                int y = (int)hm.TopLeft.Y - 1;
                int x1 = (int)hm.TopLeft.X + 25;
                int x2 = (int)hm.TopLeft.X + 30;
                x1 -= (int)map.TopLeft.X;
                x2 -= (int)map.TopLeft.X;
                for (int x = x1 - 24; x <= x2; x++)
                {
                    int row = y / map.CELL_HEIGHT;
                    int col = x / map.CELL_WIDTH;
                    int cell = map.iCells[row, col];
                    if (cell != 0 && cell != 8 && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return false;
                }
            }
           
            return true;
        }

        public void HumanJump()
        {
            Human hm = human;
            for (int i = 0; i < GlobalSetting.JUMP; i++)
            {
                hm.Jump(1);
                if (!HumanCanJump(hm))
                {
                    human.Jump(i);
                    human.isJumping = false;
                    return;
                }
            }
            human.Jump(GlobalSetting.JUMP);
            human.isJumping = false;
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
