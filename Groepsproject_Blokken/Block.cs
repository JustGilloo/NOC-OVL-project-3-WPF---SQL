using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groepsproject_Blokken
{
    internal abstract class Block
    {
        private int _rotationState;
        private Position _offset;
        protected abstract Position[][] arrTiles { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int ID { get; }

        public Block()
        {
            _offset = new Position(StartOffSet.Row, StartOffSet.Column);
        }

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position position in arrTiles[_rotationState])
            {
                yield return new Position(position.Row + _offset.Row, position.Column + _offset.Column);
            }
        }
        public void RotateClockWise()
        {
            _rotationState = (_rotationState + 1) % arrTiles.Length;
        }
        public void RotateCounterClockwise()
        {
            if (_rotationState == 0)
            {
                _rotationState = arrTiles.Length - 1;
            }
            else
            {
                _rotationState--;
            }
        }
        public void Move(int rows, int columns)
        {
            _offset.Row += rows;
            _offset.Column += columns;
        }
        public void Reset()
        {
            _rotationState = 0;
            _offset.Row = StartOffSet.Row;
            _offset.Column = StartOffSet.Column;
        }
    }
}
