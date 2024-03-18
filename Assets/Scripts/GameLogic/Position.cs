namespace GameLogic
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static Position operator +(Position position, Direction direction)
        {
            return new Position(position.Row + direction.RowDelta,
                position.Column + direction.ColumnDelta);
        }

        //public static bool IsSame(Position position1, Position position2)
        //{
        //    return position1.Row == position2.Row && position1.Column == position2.Column;
        //}

        public static Position Middle(Position position1, Position position2)
        {
            return new Position((position1.Row + position2.Row) / 2,
                (position1.Column + position2.Column) / 2);
        }
    }
}