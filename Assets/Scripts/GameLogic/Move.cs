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

        public MoveHistory Execute(Board board)
        {
            Piece piece = board[FromPosition];
            Piece eatenPiece = board[ToPosition];

            board[ToPosition] = piece;
            board[FromPosition] = null;

            return new MoveHistory(this, eatenPiece);
        }

        public void Cancel(Board board, Piece eatenPiece)
        {
            Piece piece = board[ToPosition];

            board[FromPosition] = piece;
            board[ToPosition] = eatenPiece;
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
