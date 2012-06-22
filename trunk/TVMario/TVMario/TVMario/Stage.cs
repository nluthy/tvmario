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
        private Texture2D _imgCoin;
        private Texture2D _imgIcon;
        private MapWithCells _map;
        private Human _human;
        private List<Monster> _monsters;
        private Game1 _game;
        bool _gameOver = false;




        public Stage(ContentManager content, string strData, Game1 main)
        {
            _monsters = new List<Monster>();
            _game = main;
            string strBackground, strHuman, strMap;
            XmlTextReader xml = new XmlTextReader(strData);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode stage = doc.GetElementsByTagName("stage")[0];
            strBackground = stage["background"].InnerText;
            strHuman = stage["human"].InnerText;
            strMap = stage["map"].InnerText;
            XmlNodeList monsterNodeList = stage["monsters"].ChildNodes;
            foreach (XmlNode node in monsterNodeList)
            {
                string strTopLeftX = node["topleft"]["x"].InnerText;
                string strTopLeftY = node["topleft"]["y"].InnerText;
                float topLeftX = float.Parse(strTopLeftX);
                float topLeftY = float.Parse(strTopLeftY);
                Vector2 topLeft = new Vector2(topLeftX, topLeftY);
                string strMonsterData = node["data"].InnerText;
                Monster monster = new Monster();
                monster.Init(content, topLeft, strMonsterData);
                this._monsters.Add(monster);
            }
            xml.Close();
            Init(content, strBackground, strHuman, strMap);
        }

        public void Init(ContentManager content, string strBackground, string strHuman, string strMap)
        {
            _background = content.Load<Texture2D>(strBackground);
            _imgCoin = content.Load<Texture2D>("Images\\Maps\\Tile08");
            _imgIcon = content.Load<Texture2D>("Images\\Characters\\icon24");
            _human = new Human();
            _human.Init(content, strHuman);
            _map = new MapWithCells(content, strMap);

        }

        public void Update(GameTime gameTime)
        {
            if (_human._isDie)
            {
                // _game._dieSound.Play();// Nó play hoài, sửa lại
                _gameOver = _human.Die();
                if (!_gameOver)
                {
                    Restart();
                }
            }
            else
            {
                KeyboardState kbs = Keyboard.GetState();
                _human.Update(gameTime);
                _map.Update(gameTime);
                if (_monsters.Count > 0)
                {
                    foreach (Monster m in _monsters)
                    {
                        if (_human.CollisionWithMonster(m))
                        {
                            _human._isDie = true;
                        }
                        m.Update(gameTime);
                        if (MonsterCanMove(m))
                        {
                            m.Move();
                        }
                        else
                        {
                            m.ChangeDirection();
                        }
                    }
                }

                for (int i = 0; i < _map.nRows; i++)
                {
                    for (int j = 0; j < _map.nColumns; j++)
                    {
                        if (_map.Cells[i, j].Type == GlobalSetting.INDEX_TEXTURE_COIN)
                        {
                            if (_human.CollisionWithCell(_map.Cells[i, j]))
                            {
                                _game.coinSound.Play();
                                _map.Cells[i, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_TRANSPARENT], _map.Cells[i, j].TopLeft, _map.Cells[i, j].Size, GlobalSetting.INDEX_TEXTURE_TRANSPARENT);
                                _human.nCoin++;
                            }
                        }
                    }
                }


                if (kbs.IsKeyDown(Keys.Right))
                {
                    if (_human._isRight)
                        HumanRun();
                    else
                        _human._isRight = true;
                }
                if (kbs.IsKeyDown(Keys.Left))
                {
                    if (!_human._isRight)
                        HumanBack();
                    else
                        _human._isRight = false;
                }
                if (kbs.IsKeyDown(Keys.Space))
                {
                    if (HumanIsOnTheGround(_human))
                    {
                        _human._isJumping = true;
                        HumanJump();
                    }
                }
                if (_human._isJumping)
                {
                    if (_human.JumpHightNow < _human.jumpHight)
                    {
                        int hight = _human.jumpHight - _human.JumpHightNow;
                        if ((hight) >= GlobalSetting.JUMP_STEP)
                        {
                            _human.Jump(GlobalSetting.JUMP_STEP);
                            _human.JumpHightNow += GlobalSetting.JUMP_STEP;
                        }
                        else
                        {
                            _human.Jump(hight);
                            _human.JumpHightNow += hight;
                        }
                    }
                    else
                    {
                        _human._isJumping = false;
                        _human.jumpHight = 0;
                        _human.JumpHightNow = 0;
                    }
                }
                else
                {
                    if (!HumanIsOnTheGround(_human))
                    {
                        HumanFall();
                    }
                }
                if (kbs.IsKeyUp(Keys.Left) && kbs.IsKeyUp(Keys.Right) && HumanIsOnTheGround(_human))
                {
                    _human.Sprites[0].CurrentTexture = 0;
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
                x1 -= (int)_map.TopLeft.X;
                x2 -= (int)_map.TopLeft.X;
                for (int x = x1 - 24; x <= x2; x++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;
                    int cell = _map.iCells[row, col];
                    if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
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
                x -= (int)_map.TopLeft.X;
                int y1 = (int)hm.TopLeft.Y + 1;
                int y2 = (int)hm.TopLeft.Y + 39;
                for (int y = y1; y <= y2; y++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;
                    int cell = _map.iCells[row, col];
                    if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return false;
                }
            }
            return true;
        }

        private bool HumanCanBack(Human hm)
        {
            if (hm.TopLeft.Y <= 631)
            {
                int x = (int)hm.TopLeft.X - 1;
                x -= (int)_map.TopLeft.X;
                int y1 = (int)hm.TopLeft.Y + 1;
                int y2 = (int)hm.TopLeft.Y + 39;
                for (int y = y1; y <= y2; y++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;
                    int cell = _map.iCells[row, col];
                    if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return false;
                }
            }
            return true;
        }


        public void HumanFall()
        {
            Human hm = _human;
            for (int i = 0; i < GlobalSetting.GRAVITY; i++)
            {
                hm.Fall(1);
                if (HumanIsOnTheGround(hm))
                {
                    _human.Fall(i);
                    return;
                }
            }
            _human.Fall(GlobalSetting.GRAVITY);
        }

        public void HumanRun()
        {
            if (HumanCanRun(_human))
            {

                if (_human.TopLeft.X < 492)
                {
                    _human.Run(GlobalSetting.STEP_WIDTH);
                }
                else
                {
                    bool mapMove = _map.MoveLeft();
                    if (!mapMove)
                    {
                        if (_human.TopLeft.X < (1024 - 40))
                        {
                            _human.Run(GlobalSetting.STEP_WIDTH);
                        }
                    }
                }
            }
        }

        public void HumanBack()
        {
            if (HumanCanBack(_human))
            {
                if (_human.TopLeft.X > 492)
                {
                    _human.Run(-GlobalSetting.STEP_WIDTH);
                }
                else
                {
                    bool mapMove = _map.MoveRight();
                    if (!mapMove)
                    {
                        if (_human.TopLeft.X > 40)
                        {
                            _human.Run(-GlobalSetting.STEP_WIDTH);
                        }
                    }
                }

            }
        }

        private bool HumanCanJump(Human hm)
        {
            if (hm.TopLeft.Y <= 631)
            {
                int y = (int)hm.TopLeft.Y - 1;
                int x1 = (int)hm.TopLeft.X + 25;
                int x2 = (int)hm.TopLeft.X + 30;
                x1 -= (int)_map.TopLeft.X;
                x2 -= (int)_map.TopLeft.X;
                for (int x = x1 - 24; x <= x2; x++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;
                    int cell = _map.iCells[row, col];
                    if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return false;
                }
                return true;
            }

            return false;
        }

        public void HumanJump()
        {
            _game.jumpSound.Play();

            Human hm = new Human(_human);
            for (int i = 1; i <= GlobalSetting.JUMP; i++)
            {
                hm.Jump(1);
                if (!HumanCanJump(hm))
                {
                    _human.jumpHight = i;
                    return;
                }
            }
            _human.jumpHight = GlobalSetting.JUMP;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_background != null)
                spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            spriteBatch.Draw(_imgCoin, Vector2.Zero, Color.White);
            SpriteFont font = _game.Content.Load<SpriteFont>("Fonts\\Font01");
            spriteBatch.DrawString(font, "x" + _human.nCoin, new Vector2(28, 0), Color.White);
            spriteBatch.Draw(_imgIcon, new Vector2(0, 28), Color.White);
            spriteBatch.DrawString(font, "x" + _human.nLife, new Vector2(28, 28), Color.White);
            _human.Draw(spriteBatch, gameTime, Color.White);
            _map.Draw(spriteBatch, gameTime, Color.White);
            if (_monsters.Count > 0)
            {
                foreach (Monster m in _monsters)
                {
                    m.Draw(spriteBatch, gameTime, Color.White);
                }
            }
        }


        public bool MonsterCanMove(Monster mt)
        {
            if (mt.isRight)
            {
                int x = (int)mt.TopLeft.X + 23;
                int y1 = (int)mt.TopLeft.Y + 1;
                int y2 = (int)mt.TopLeft.Y + 31;
                for (int y = y1; y <= y2; y++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;

                    if (row >= 0 && col >= 0 && row < _map.nRows && col < _map.nColumns)
                    {
                        int cell = _map.iCells[row, col];
                        if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                            return false;
                    }
                    else
                        return false;
                }
                return true;

            }
            else
            {
                int x = (int)mt.TopLeft.X - 1;
                int y1 = (int)mt.TopLeft.Y + 1;
                int y2 = (int)mt.TopLeft.Y + 31;
                for (int y = y1; y <= y2; y++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;
                    if (row >= 0 && col >= 0 && row < _map.nRows && col < _map.nColumns)
                    {
                        int cell = _map.iCells[row, col];
                        if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                            return false;
                    }
                    else return false;
                }
                return true;
            }
        }

        public void Restart()
        {

        }

    }
}
