using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TVMario
{
    public class MyMap : VisibleGameEntity
    {
        int nRow;
        int nColumn;
        int tileWidth;
        int tileHeight;
        int height = GraphicsDeviceManager.DefaultBackBufferHeight;
        Rectangle[] rec = new Rectangle[16 * 24 + 1];
        int[,] dataMap = new int[35, 159];


        public MyMap(int size, int row, int column, int tWidth, int tHeight,string strFile)
        {
            nRow = row;
            nColumn = column;
            tileWidth = tWidth;
            tileHeight = tHeight;
            _spritesCount = size;
            //_sprites = new My2DSprite[_spritesCount];
            _sprites = new List<My2DSprite>();
            GlobalSetting._deltaX = 0;
            GlobalSetting._deltaY = -(int)size / 2 * height;

            int k = 0;
            rec[k++] = new Rectangle(0, 0, 0, 0);
            for (int i = 0; i < 24; i++)
                for (int j = 0; j < 16; j++)
                {
                    rec[k++] = new Rectangle(j * tileWidth, i * tileHeight, tileWidth, tileHeight);
                }

            StreamReader sr = new StreamReader(strFile);

            for (int i = 0; i < nRow; i++)
            {
                string s = sr.ReadLine();
                int startIndex = 0;
                int endIndex;
                for (int j = 0; j < nColumn; j++)
                {
                    endIndex = s.IndexOf(' ', startIndex + 1);
                    dataMap[i, j] = int.Parse(s.Substring(startIndex, endIndex - startIndex));
                    startIndex = endIndex;
                }
            }

        }


        public override bool Init(ContentManager Content, int n, string strResource)
        {
            try
            {
                for (int i = 0; i < _spritesCount; i++)
                {
                    Texture2D[] map = new Texture2D[1];
                    map[0] = Content.Load<Texture2D>("Images/Map/" + strResource);
                    Vector2 a = new Vector2(0, i * height);
                    //this._sprites[i] = new My2DSprite(map, a);
                    this._sprites.Add(new My2DSprite(map,a));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                if (GlobalSetting._deltaX < 0)
                    GlobalSetting._deltaX += 10;
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                if (GlobalSetting._deltaX > (GraphicsDeviceManager.DefaultBackBufferWidth - nColumn * tileWidth) + 10)
                    GlobalSetting._deltaX -= 10;
            }

            if (ks.IsKeyDown(Keys.Escape))
            {
                
            }

            //if (ks.IsKeyDown(Keys.Up))
            //{
            //    if (GlobalSetting._deltaY < 0)
            //        GlobalSetting._deltaY += 10;


            //}
            //else if (ks.IsKeyDown(Keys.Down))
            //{
            //    if (GlobalSetting._deltaY > GraphicsDeviceManager.DefaultBackBufferHeight - 35 * 20 + 10)
            //        GlobalSetting._deltaY -= 10;
            //}
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nColumn; j++)
                {
                    Vector2 a = new Vector2(j * tileWidth+1, i * tileHeight+1);
                    _sprites[0].Position = a;
                    _sprites[0].Draw(gameTime, spriteBatch, rec[dataMap[i, j]]);
                }
            }
        }

        
    }
}
