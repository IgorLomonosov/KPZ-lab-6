﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisLibrary.Blocks;

namespace TetrisLibrary
{
    public class BlockQueue
    {
        private readonly IFigure[] blocks = new IFigure[]
        {
            new IBlock(), new JBlock(), new LBlock(), new OBlock(), new SBlock(), new TBlock(), new ZBlock()
        };
        private readonly Random random = new Random();
        public IFigure NextBlock { get; private set; }
        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }
        private IFigure RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }
        public IFigure GetAndUpdate()
        {
            IFigure block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
