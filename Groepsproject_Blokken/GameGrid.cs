using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Groepsproject_Blokken
{
    internal class GameGrid
    {
        MediaPlayer lineClearedSound = new MediaPlayer();
        private readonly int[,] _grid;
        public int Rows { get; }
        public int Columns { get; }
        public int this[int row, int column]
        {
            get { return _grid[row, column]; }
            set { _grid[row, column] = value; }
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _grid = new int[Rows, Columns];
        }

        public bool IsInside(int row, int column)
        {
            bool isInside = false;
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                isInside = true;
            }
            return isInside;
        }
        public bool IsEmpty(int row, int column)
        {
            bool isEmpty = false;
            if (IsInside(row, column) && _grid[row, column] == 0)
            {
                isEmpty = true;
            }
            return isEmpty;
        }
        public bool IsRowFull(int row)
        {
            bool isFull = true;
            for (int column = 0; column < Columns; column++)
            {
                if (_grid[row, column] == 0)
                {
                    isFull = false;
                }
            }
            return isFull;
        }
        public bool IsRowEmpty(int row)
        {
            bool isEmpty = true;
            for (int column = 0; column < Columns; column++)
            {
                if (_grid[row, column] != 0)
                {
                    isEmpty = false;
                }
            }
            return isEmpty;
        }
        public void ClearRow(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                _grid[row, column] = 0;
            }
        }
        private void MoveRowDown(int row, int numberOfRows)
        {
            for (int column = 0; column < Columns; column++)
            {
                _grid[row + numberOfRows, column] = _grid[row, column];
                _grid[row, column] = 0;
            }
        }
        public int ClearFullRows()
        {
            string soundeffectFilePath = "../../Assets/Sounds/Blokken LineCleared.wav";
            int cleared = 0;
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    if (!string.IsNullOrEmpty(soundeffectFilePath))
                    {
                        lineClearedSound.Open(new Uri(soundeffectFilePath, UriKind.Relative));
                        lineClearedSound.Volume = 0.5;
                        lineClearedSound.Play();
                    }
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(row, cleared);
                }
            }
            return cleared;
        }
    }
}
