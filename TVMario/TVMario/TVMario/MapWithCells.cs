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
    public class MapWithCells : VisibleGameEntity
    {
        public int CELL_WIDTH = 0;
        public int CELL_HEIGHT = 0;

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

        protected Cell[,] Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        public MapWithCells(ContentManager content, String[] strCells, Vector2 topleft, Vector2 size, int rows, int columns, int[,] cells)
        {
            TopLeft = topleft;
            Size = size;

            nRows = rows;
            nColumns = columns;

            Cells = new Cell[nRows, nColumns];
            for (int i = 0; i < nRows; i++)
                for (int j = 0; i < nColumns; j++)
                {
                    Cells[i, j] = new Cell(content, strCells[cells[i,j]], new Vector2(j * CELL_WIDTH, i * CELL_HEIGHT), new Vector2(CELL_WIDTH, CELL_HEIGHT));
                }
            _sprites = new List<My2DSprite>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {

            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nColumns; j++)
                {
                    Cells[i, j].Draw(gameTime, spriteBatch, color);
                }
            base.Draw(gameTime, spriteBatch, color);
        }
    }
}
