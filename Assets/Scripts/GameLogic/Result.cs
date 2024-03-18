namespace GameLogic
{
    public class Result
    {
        public PieceColor Winner { get; }
        public EndReason Reason { get; }

        public Result(PieceColor winner, EndReason reason)
        {
            Winner = winner;
            Reason = reason;
        }

        public static Result Win(PieceColor winner, EndReason reason)
        {
            return new Result(winner, reason);
        }

        public static Result Draw(EndReason reason)
        {
            return new Result(PieceColor.None, reason);
        }
    }
}
