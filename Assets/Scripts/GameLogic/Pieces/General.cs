using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Pieces
{
    public class General : Piece
    {
        public override PieceType Type => PieceType.General;

        public General(PieceColor color) : base(color) { }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.North, Direction.South, Direction.East, Direction.West
        };

        public override Piece Copy()
        {
            return new General(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, directions)
                .Where(to => Board.IsInNinePalaces(to, Color))
                .Select(to => new Move(from, to))
                .Concat(MoveAsGeneralFaceToFace(from, board));
        }

        private IEnumerable<Move> MoveAsGeneralFaceToFace(Position from, Board board)
        {
            Direction direction = Direction.GetForwardDirection(Color);

            for (Position to = from + direction; Board.IsInside(to); to += direction)
            {
                if (!board.IsEmpty(to))
                {
                    Piece piece = board[to];

                    if (piece.Color != Color && piece.Type == PieceType.General)
                    {
                        yield return new Move(from, to);
                    }

                    break;
                }
            }
        }
    }
}