namespace GameLogic
{
    public static class PieceColorExtensions
    {
        public static PieceColor Opponent(this PieceColor color)
        {
            return color switch
            {
                PieceColor.Black => PieceColor.Red,
                PieceColor.Red => PieceColor.Black,
                _ => color
            };
        }
    }
}
