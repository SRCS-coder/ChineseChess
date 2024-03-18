using System.Collections.Generic;

namespace GameLogic.Pieces
{
    public class Cannon : Piece
    {
        public override PieceType Type => PieceType.Cannon;

        public Cannon(PieceColor color) : base(color) { }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.North, Direction.South, Direction.East, Direction.West
        };

        public override Piece Copy()
        {
            return new Cannon(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Direction direction in directions)
            {
                Position batteryPosition = null;

                for (Position to = from + direction; Board.IsInside(to); to += direction)
                {
                    if (board.IsEmpty(to))
                    {
                        yield return new Move(from, to);

                        continue;
                    }

                    batteryPosition = to;

                    break;
                }

                if (batteryPosition == null)
                {
                    continue;
                }

                for (Position to = batteryPosition + direction; Board.IsInside(to); to += direction)
                {
                    if (board.IsEmpty(to))
                    {
                        continue;
                    }

                    Piece piece = board[to];

                    if (piece.Color != Color)
                    {
                        yield return new Move(from, to);
                    }

                    break;
                }
            }
        }
    }
}
