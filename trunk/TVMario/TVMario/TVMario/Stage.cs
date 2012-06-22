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
        private List<Skill> _skills;
        private Game1 _game;
        bool _gameOver = false;




        public Stage(ContentManager content, string strData, Game1 main)
        {
            _monsters = new List<Monster>();
            _skills = new List<Skill>();
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
            XmlNodeList skillNodeList = stage["skills"].ChildNodes;
            foreach (XmlNode node in skillNodeList)
            {
                string strSkill = node["data"].InnerText;
                Skill skill = new Skill();
                skill.Init(content, strSkill);
                this._skills.Add(skill);
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
                        if (_human.CollisionWithMonster(m) && !m.isDie)
                        {
                            _human._isDie = true;
                        }
                        m.Update(gameTime);
                        if (!m.isDie)
                        {
                            if (MonsterCanMove(m))
                            {
                                m.Move();
                            }
                            else
                            {
                                m.ChangeDirection();
                            }
                            if (!MonsterIsOnTheGround(m))
                            {

                            }
                        }
                    }
                }

                for (int i = 0; i < _map.nRows; i++)
                {
                    for (int j = 0; j < _map.nColumns; j++)
                    {
                        Cell cell = _map.Cells[i, j];
                        if (cell.Type == GlobalSetting.INDEX_TEXTURE_COIN)
                        {
                            if (_human.CollisionWithCell(cell))
                            {
                                _game.coinSound.Play();
                                _map.Cells[i, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_TRANSPARENT], cell.TopLeft, cell.Size, GlobalSetting.INDEX_TEXTURE_TRANSPARENT, 0);
                                _human.nCoin++;
                            }
                        }
                        else
                        {
                            if (cell.Type == GlobalSetting.INDEX_TEXTURE_FLOWER)
                            {
                                if (_human.CollisionWithCell(cell))
                                {
                                    // _game.coinSound.Play();
                                    _map.Cells[i, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_TRANSPARENT], cell.TopLeft, cell.Size, GlobalSetting.INDEX_TEXTURE_TRANSPARENT, 0);
                                    _human.skillList.Add(3);
                                }
                            }
                            else
                            {
                                if (cell.Type == GlobalSetting.INDEX_TEXTURE_STAR)
                                {
                                    if (_human.CollisionWithCell(cell))
                                    {
                                        // _game.coinSound.Play();
                                        _map.Cells[i, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_TRANSPARENT], cell.TopLeft, cell.Size, GlobalSetting.INDEX_TEXTURE_TRANSPARENT, 0);
                                        _human.skillList.Add(2);
                                    }
                                }
                                else
                                {
                                    if (cell.Type == GlobalSetting.INDEX_TEXTURE_QUESTION)
                                    {
                                        if (cell.CollisionWwithHuman(_human))
                                        {
                                            _game.coinSound.Play();
                                            _map.Cells[i, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_LAND], cell.TopLeft, cell.Size, GlobalSetting.INDEX_TEXTURE_LAND, 0);
                                            _human.nCoin++;
                                        }
                                    }
                                    else
                                    {
                                        if (cell.SkillType != 0)
                                        {
                                            if (cell.CollisionWwithHuman(_human))
                                            {
                                                if (cell.SkillType == 3)
                                                {
                                                    _map.Cells[i - 1, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_FLOWER], _map.Cells[i - 1, j].TopLeft, _map.Cells[i - 1, j].Size, GlobalSetting.INDEX_TEXTURE_FLOWER, 0);
                                                }
                                                else
                                                {
                                                    if (cell.SkillType == 2)
                                                    {
                                                        _map.Cells[i - 1, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_STAR], _map.Cells[i - 1, j].TopLeft, _map.Cells[i - 1, j].Size, GlobalSetting.INDEX_TEXTURE_STAR, 0);
                                                    }
                                                }
                                                _map.Cells[i, j] = new Cell(_game.Content, _map.strCells[GlobalSetting.INDEX_TEXTURE_LAND], cell.TopLeft, cell.Size, GlobalSetting.INDEX_TEXTURE_LAND, 0);
                                                _map.Cells[i, j].SkillType = 0;

                                            }
                                        }
                                    }
                                }

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

                if (kbs.IsKeyDown(Keys.A) && _human.HasSkill(1))
                {
                    _game.cutSound.Play();
                    foreach (Skill sk in _skills)
                    {
                        if (sk.skillType == 1)
                        {
                            Vector2 cur = _human.TopLeft;
                            if (_human._isRight)
                            {
                                cur.X += 30;
                            }
                            else
                            {
                                cur.X -= 30;
                            }
                            sk.TopLeft = cur;
                            sk.show = true;
                        }
                    }
                }
                else
                {
                    if (kbs.IsKeyDown(Keys.W) && _human.HasSkill(2))
                    {
                        _game.fireBallSound.Play();
                        foreach (Skill sk in _skills)
                        {
                            if (sk.skillType == 2)
                            {
                                Vector2 cur = _human.TopLeft;
                                if (_human._isRight)
                                {
                                    cur.X += 30;
                                }
                                else
                                {
                                    cur.X -= 30;
                                }
                                sk.TopLeft = cur;
                                sk.show = true;
                            }
                        }
                    }
                    else
                    {
                        if (kbs.IsKeyDown(Keys.D) && _human.HasSkill(3))
                        {
                            _game.boltSound.Play();
                            foreach (Skill sk in _skills)
                            {
                                if (sk.skillType == 3)
                                {
                                    Vector2 cur = _human.TopLeft;
                                    if (_human._isRight)
                                    {
                                        cur.X += 30;
                                    }
                                    else
                                    {
                                        cur.X -= 30;
                                    }
                                    sk.TopLeft = cur;
                                    sk.show = true;
                                }
                            }
                        }
                    }
                }

                if (_skills.Count > 0)
                {
                    foreach (Skill sk in _skills)
                    {
                        if (sk.TopLeft != Vector2.Zero)
                        {
                            sk.Update(gameTime, _human._isRight);

                            foreach (Monster m in _monsters)
                            {
                                if (sk.CollisionWithMonster(m))
                                {
                                    if (m.blood > 0)
                                    {
                                        if ((m.blood - sk.damage) >= 0)
                                        {
                                            m.blood -= sk.damage;
                                        }
                                        else
                                        {
                                            m.blood = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }

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
                    int cell = _map.Cells[row, col].Type;
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
                    int cell = _map.Cells[row, col].Type;
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
                    int cell = _map.Cells[row, col].Type;
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
                int y = (int)hm.TopLeft.Y;
                int x1 = (int)hm.TopLeft.X + 25;
                int x2 = (int)hm.TopLeft.X + 30;
                x1 -= (int)_map.TopLeft.X;
                x2 -= (int)_map.TopLeft.X;
                for (int x = x1 - 24; x <= x2; x++)
                {
                    int row = y / _map.CELL_HEIGHT;
                    int col = x / _map.CELL_WIDTH;
                    int cell = _map.Cells[row, col].Type;
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
                    if (!m.isDie)
                        m.Draw(spriteBatch, gameTime, Color.White);
                }
            }
            if (_skills.Count > 0)
            {
                foreach (Skill sk in _skills)
                {
                    if (sk.TopLeft != Vector2.Zero)
                    {
                        if (_human._isRight)
                            sk.Draw(spriteBatch, gameTime, Color.White);
                        else
                            sk.DrawFlipHorizontal(spriteBatch, gameTime, Color.White);
                    }
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
                        int cell = _map.Cells[row, col].Type;
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
                        int cell = _map.Cells[row, col].Type;
                        if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                            return false;
                    }
                    else return false;
                }
                return true;
            }
        }

        private bool MonsterIsOnTheGround(Monster mt)
        {
            int y = (int)mt.TopLeft.Y + 32;
            int x1 = (int)mt.TopLeft.X;
            int x2 = (int)mt.TopLeft.X + 24;
            for (int x = x1 - 24; x <= x2; x++)
            {
                int row = y / _map.CELL_HEIGHT;
                int col = x / _map.CELL_WIDTH;
                if (row >= 0 && col >= 0 && row < _map.nRows && col < _map.nColumns)
                {
                    int cell = _map.Cells[row, col].Type;
                    if (cell != GlobalSetting.INDEX_TEXTURE_TRANSPARENT && cell != GlobalSetting.INDEX_TEXTURE_COIN && cell != 9 && cell != 10 && cell != 12 && cell != 13)
                        return true;
                }
            }
            return false;
        }

        public void MonsterFall(ref Monster mt)
        {
            Monster mon = mt;
            for (int i = 0; i < GlobalSetting.GRAVITY; i++)
            {
                mon.Fall(1);
                if (MonsterIsOnTheGround(mon))
                {
                    mt.Fall(i);
                    return;
                }
            }
            mt.Fall(GlobalSetting.GRAVITY);
        }

        public void Restart()
        {

        }

    }
}
