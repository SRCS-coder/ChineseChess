namespace GameLogic
{
    public class Move
    {
        public Position FromPosition { get; }
        public Position ToPosition { get; }

        public Move(Position from, Position to)
        {
            FromPosition = from;
            ToPosition = to;
        }

        public void Execute(Board board)
        {
            Piece piece = board[FromPosition];
            board[ToPosition] = piece;
            board[FromPosition] = null;
        }

        public bool IsLegal(Board board)
        {
            PieceColor color = board[FromPosition].Color;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(color);
        }
    }
}
