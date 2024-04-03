﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groepsproject_Blokken
{
    internal class TBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
            {
            new Position[] { new Position(0, 1), new Position(1, 0), new Position(1, 1), new Position(1,2) },
            new Position[] { new Position(0, 1), new Position(1, 1), new Position(1, 2), new Position(2,1) },
            new Position[] { new Position(1, 0), new Position(1, 1), new Position(1, 2), new Position(2,1) },
            new Position[] { new Position(0, 1), new Position(1, 0), new Position(1, 1), new Position(2,1) },
            };
        public override int ID => 6;
        protected override Position StartOffSet => new Position(0, 3);
        protected override Position[][] arrTiles => tiles;
    }
}
