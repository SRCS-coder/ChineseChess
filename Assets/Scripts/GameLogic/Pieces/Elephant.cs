using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Pieces
{
    public class Elephant : Piece
    {
        public override PieceType Type => PieceType.Elephant;

        public Elephant(PieceColor color) : base(color) { }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.NorthWest * 2, Direction.NorthEast * 2,
            Direction.SouthWest * 2, Direction.SouthEast * 2
        };

        public override Piece Copy()
        {
            return new Elephant(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, directions)
                .Where(to => board.IsEmpty(Position.Middle(from, to)))
                .Where(to => Board.IsInColorRange(to, Color))
                .Select(to => new Move(from, to));
        }
    }
}
