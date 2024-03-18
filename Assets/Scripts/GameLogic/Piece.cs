using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }

        public PieceColor Color { get; }

        public Piece(PieceColor color)
        {
            Color = color;
        }

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected IEnumerable<Position> MovePositionsInDirections(Position from, Board board, IEnumerable<Direction> directions)
        {
            foreach (Direction direction in directions)
            {
                Position to = from + direction;

                if (!Board.IsInside(to) || !board.IsEmpty(to) && board[to].Color == Color)
                {
                    continue;
                }
                
                yield return to;
            }
        }

        public bool CanCaptureOpponentGeneral(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];

                return piece != null && piece.Type == PieceType.General;
            });
        }
    }
}