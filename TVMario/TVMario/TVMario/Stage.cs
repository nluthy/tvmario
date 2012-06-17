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
        MapWithCells map;
        Human human;
        List<Monster> monsters;

        public void Init(ContentManager content, string strBackground, string strHuman, string strMap, string strMonsters)
        {
            //Background = content.Load<Texture2D>(strBackground);
            human = new Human();
            human.Init(content, strHuman);

            //Test
            String[] strCells = new String[21];
            for (int i = 0; i < strCells.Length; i++)
            {
                strCells[i] = "Images\\Maps\\Tile" + i.ToString("00");
            }
            int nRows = 30;
            int nColumns = 30;
            int[,] cells = new int[nRows, nColumns];
            Random rand = new Random();
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nColumns; j++)
                {
                    cells[i, j] = rand.Next() % 21;
                }
            map = new MapWithCells(content, strCells, new Vector2(0, 0), new Vector2(nColumns * 24, nRows * 24), nRows, nColumns, cells);
            
        }

        public void Update(GameTime gameTime)
        {
            human.Update(gameTime);
            map.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            human.Draw(gameTime, spriteBatch, Color.White);
            map.Draw(gameTime, spriteBatch, Color.White);
        }

    }
}
