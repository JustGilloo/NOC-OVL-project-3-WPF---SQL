using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groepsproject_Blokken
{
    internal class BlockQueue
    {
        private readonly Block[] arrBlocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };
        private readonly Random random = new Random();

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        public Block NextBlock { get; private set; }

        public Block RandomBlock()
        {
            return arrBlocks[random.Next(arrBlocks.Length)];
        }
        public Block GetAndUpdate()
        {
            Block block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            } while (block.ID == NextBlock.ID);
            return block;
        }
    }
}
