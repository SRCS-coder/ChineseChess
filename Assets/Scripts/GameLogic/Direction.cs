namespace GameLogic
{
    public class Direction
    {
        public readonly static Direction North = new(-1, 0);
        public readonly static Direction South = new(1, 0);
        public readonly static Direction East = new(0, 1);
        public readonly static Direction West = new(0, -1);

        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;

        public int RowDelta { get; }
        public int ColumnDelta { get; }

        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        public static Direction operator +(Direction direction1, Direction direction2)
        {
            return new Direction(direction1.RowDelta + direction2.RowDelta,
                direction1.ColumnDelta + direction2.ColumnDelta);
        }

        public static Direction operator *(Direction direction, int scale)
        {
            return new Direction(direction.RowDelta * scale, direction.ColumnDelta * scale);
        }

        public static Direction GetForwardDirection(PieceColor color)
        {
            return color switch
            {
                PieceColor.Black => South,
                PieceColor.Red => North,
                _ => null
            };
        }
    }
}