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
using System.IO;


namespace TVMario
{
    public class MapWithCells : VisibleGameEntity
    {
        public int CELL_WIDTH = 24;
        public int CELL_HEIGHT = 24;

        float _scale = 1.0f;

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        int _nRows;

        public int nRows
        {
            get { return _nRows; }
            set { _nRows = value; }
        }
        int _nColumns;

        public int nColumns
        {
            get { return _nColumns; }
            set { _nColumns = value; }
        }

        private Cell[,] _cells;

        public Cell[,] Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        private int[,] _iCells;

        public int[,] iCells
        {
            get { return _iCells; }
            set { _iCells = value; }
        }

        public MapWithCells(ContentManager content, string strData)
        {
            String[] strCells = new String[21];
            for (int i = 0; i < strCells.Length; i++)
            {
                strCells[i] = "Images\\Maps\\Tile" + i.ToString("00");
            }
            StreamReader reader = new StreamReader(strData);
            string strLine = reader.ReadLine();
            string[] strArray = strLine.Split(' ');
            int x = Int32.Parse(strArray[0]);
            int y = Int32.Parse(strArray[1]);
            strLine = reader.ReadLine();
            strArray = strLine.Split(' ');
            int nRows = Int32.Parse(strArray[0]);
            int nColumns = Int32.Parse(strArray[1]);
            int[,] cells = new int[nRows, nColumns];
            for (int i = 0; i < nRows; i++)
            {
                strLine = reader.ReadLine();
                strArray = strLine.Split(' ');
                for (int j = 0; j < nColumns; j++)
                {
                    cells[i, j] = Int32.Parse(strArray[j]);
                }
            }
            Init(content, strCells, new Vector2(x, y), new Vector2(nColumns * CELL_WIDTH, nRows * CELL_HEIGHT), nRows, nColumns, cells);
        }

        public void Init(ContentManager content, String[] strCells, Vector2 topleft, Vector2 size, int rows, int columns, int[,] cells)
        {
            TopLeft = topleft;
            Size = size;

            nRows = rows;
            nColumns = columns;

            iCells = cells;

            Cells = new Cell[nRows, nColumns];
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nColumns; j++)
                {
                    Cells[i, j] = new Cell(content, strCells[cells[i, j]], new Vector2(j * CELL_WIDTH + TopLeft.X, i * CELL_HEIGHT + TopLeft.Y), new Vector2(CELL_WIDTH, CELL_HEIGHT));
                }
            _sprites = new List<My2DSprite>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void MoveLeft()
        {
            Vector2 ufo = Cells[0, 0].TopLeft;
            if (ufo.X > (-1 * (Size.X - Game1.graphics.GraphicsDevice.Viewport.Width)))
            {
                moveMap(-1, 0);
            }
        }

        public void MoveRight()
        {
            Vector2 ufo = Cells[0, 0].TopLeft;
            if (ufo.X < 0)
            {
                moveMap(1, 0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {

            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nColumns; j++)
                {
                    Cells[i, j].Draw(spriteBatch, gameTime, color);
                }

        }

        public void moveMap(int x, int y)
        {
            Vector2 curTopLeft = TopLeft;
            curTopLeft.X += x;
            curTopLeft.Y += y;
            TopLeft = curTopLeft;
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nColumns; j++)
                {
                    Vector2 temp = Cells[i, j].TopLeft;
                    temp.X += x;
                    temp.Y += y;
                    Cells[i, j].TopLeft = temp;
                }
        }
    }
}
