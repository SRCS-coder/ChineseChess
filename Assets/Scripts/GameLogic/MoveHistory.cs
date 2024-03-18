namespace GameLogic
{
    public class MoveHistory
    {
        public Move Move { get; }
        public Piece EatenPiece { get; }

        public MoveHistory(Move move, Piece eatenPiece)
        {
            Move = move;
            EatenPiece = eatenPiece;
        }
    }
}
