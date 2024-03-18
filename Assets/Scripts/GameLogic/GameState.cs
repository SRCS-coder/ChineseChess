using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public class GameState
    {
        public Board Board { get; }
        public PieceColor CurrentColor { get; private set; }
        public Result Result { get; private set; }

        public GameState()
        {
            Board = Board.Initial();
            CurrentColor = PieceColor.Red;
            Result = null;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position position)
        {
            if (Board.IsEmpty(position) || Board[position].Color != CurrentColor)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[position];
            IEnumerable<Move> moveCandidates = piece.GetMoves(position, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public IEnumerable<Move> AllLegalMovesFor(PieceColor color)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(color).SelectMany(position =>
            {
                Piece piece = Board[position];
                return piece.GetMoves(position, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            move.Execute(Board);
            CurrentColor = CurrentColor.Opponent();
            CheckForGameOver();
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentColor).Any())
            {
                PieceColor winner = CurrentColor.Opponent();
                EndReason endReason = Board.IsInCheck(CurrentColor) ? EndReason.Checkmate : EndReason.Stalemate;
                Result = Result.Win(winner, endReason);
            }
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }
    }
}