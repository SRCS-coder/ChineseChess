using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Pieces
{
    public class Soldier : Piece
    {
        public override PieceType Type => PieceType.Soldier;

        public Soldier(PieceColor color) : base(color) { }

        public override Piece Copy()
        {
            return new Soldier(Color);
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, AvailableDirections(from))
                .Select(to => new Move(from, to));
        }

        private IEnumerable<Direction> AvailableDirections(Position from)
        {
            yield return Direction.GetForwardDirection(Color);

            if (!Board.IsInColorRange(from, Color))
            {
                yield return Direction.East;
                yield return Direction.West;
            }
        }
    }
}
