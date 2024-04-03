namespace Groepsproject_Blokken
{
    internal class OBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
            {
            new Position[] { new Position(0, 0), new Position(0, 1), new Position(1, 0), new Position(1,1) }
            };
        public override int ID => 4;
        protected override Position StartOffSet => new Position(0, 4);
        protected override Position[][] arrTiles => tiles;
    }
}
