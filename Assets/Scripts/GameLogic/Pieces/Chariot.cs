using System.Collections.Generic;

namespace GameLogic.Pieces
{
    public class Chariot : Piece
    {
        public override PieceType Type => PieceType.Chariot;

        public Chariot(PieceColor color) : base(color) { }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.North, Direction.South, Direction.East, Direction.West
        };

        public override Piece Copy()
        {
            return new Chariot(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Direction direction in directions)
            {
                for (Position to = from + direction; Board.IsInside(to); to += direction)
                {
                    if (board.IsEmpty(to))
                    {
                        yield return new Move(from, to);

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
