using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Pieces
{
    public class Advisor : Piece
    {
        public override PieceType Type => PieceType.Advisor;

        public Advisor(PieceColor color) : base(color) { }

        private static readonly Direction[] directions = new Direction[]
        {
            Direction.NorthWest, Direction.NorthEast,
            Direction.SouthWest, Direction.SouthEast
        };

        public override Piece Copy()
        {
            return new Advisor(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, directions)
                .Where(to => Board.IsInNinePalaces(to, Color))
                .Select(to => new Move(from, to));
        }
    }
}
