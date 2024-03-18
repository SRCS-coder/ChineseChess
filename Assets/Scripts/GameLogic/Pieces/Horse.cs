using System.Collections.Generic;

namespace GameLogic.Pieces
{
    public class Horse : Piece
    {
        public override PieceType Type => PieceType.Horse;

        public Horse(PieceColor color) : base(color) { }

        private static readonly Direction[,] directions = new Direction[,]
        {
            { Direction.North, Direction.NorthEast, Direction.NorthWest },
            { Direction.South, Direction.SouthEast, Direction.SouthWest },
            { Direction.East, Direction.NorthEast, Direction.SouthEast },
            { Direction.West, Direction.NorthWest, Direction.SouthWest }
        };

        public override Piece Copy()
        {
            return new Horse(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            for (int i = 0; i < 4; i++)
            {
                Position legPosition = from + directions[i, 0];

                if (!Board.IsInside(legPosition))
                {
                    continue;
                }

                if (board.IsEmpty(legPosition))
                {
                    for (int j = 1; j <= 2; j++)
                    {
                        Position to = legPosition + directions[i, j];

                        if (!Board.IsInside(to) || !board.IsEmpty(to) && board[to].Color == Color)
                        {
                            continue;
                        }

                        yield return new Move(from, to);
                    }
                }
            }
        }
    }
}
