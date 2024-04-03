using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groepsproject_Blokken
{
    internal class GameState
    {
        private Block _currentBlock;

        public Block CurrentBlock
        {
            get => _currentBlock;
            private set
            {
                _currentBlock = value;
                _currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    CurrentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        CurrentBlock.Move(-1, 0);
                    }
                }
            }
        }
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }
        public bool GameOver { get; private set; }
        public bool Pause { get; set; }
        public bool BlockIsPlaced { get; set; }
        public int Score { get; set; }
        public int ScorePlayerOne { get; set; }
        public int ScorePlayerTwo { get; set; }
        public bool RowIsCleared { get; set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
            Pause = false;
        }

        public bool BlockFits()
        {
            bool fits = true;
            foreach (Position position in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(position.Row, position.Column))
                {
                    fits = false;
                }
            }
            return fits;
        }
        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }
            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmpBlock = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmpBlock;
            }
            CanHold = false;
        }
        public void RotateBlockClockWise()
        {
            CurrentBlock.RotateClockWise();
            if (!BlockFits())
            {
                CurrentBlock.RotateCounterClockwise();
            }
        }
        public void RotateBlockCounterClockwise()
        {
            CurrentBlock.RotateCounterClockwise();
            if (!BlockFits())
            {
                CurrentBlock.RotateClockWise();
            }
        }
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }
        public bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }
        public void PlaceBlock()
        {
            foreach (Position position in CurrentBlock.TilePositions())
            {
                GameGrid[position.Row, position.Column] = CurrentBlock.ID;
            }

            int rowsCleared = GameGrid.ClearFullRows();
            if (rowsCleared > 0)
            {
                Score += rowsCleared * 100;
                RowIsCleared = true;
            }

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                BlockIsPlaced = true;
                CanHold = true;
            }
        }
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
        public int TileDropDistance(Position position)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(position.Row + drop + 1, position.Column))
            {
                drop++;
            }
            return drop;
        }
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;
            foreach (Position position in CurrentBlock.TilePositions())
            {
                drop = Math.Min(drop, TileDropDistance(position));
            }
            return drop;
        }
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
