using System;
using System.Collections.Generic;

namespace GameLogic
{
    public class Counting
    {
        private readonly Dictionary<PieceType, int> redCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();

        public int TotalCount { get; private set; }

        public Counting()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                redCount[type] = 0;
                blackCount[type] = 0;
            }
        }

        public void Increment(PieceColor color, PieceType type)
        {
            if (color == PieceColor.Red)
            {
                redCount[type]++;
            }
            else if (color == PieceColor.Black)
            {
                blackCount[type]++;
            }

            TotalCount++;
        }

        public int Red(PieceType type)
        {
            return redCount[type];
        }

        public int Black(PieceType type)
        {
            return blackCount[type];
        }
    }
}
