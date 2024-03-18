using GameLogic.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public class Board
    {
        public const int RowCount = 10;
        public const int ColumnCount = 9;

        private readonly Piece[,] pieces = new Piece[RowCount, ColumnCount];

        public Piece this[int row, int column]
        {
            get { return pieces[row, column]; }
            set { pieces[row, column] = value; }
        }

        public Piece this[Position position]
        {
            get { return pieces[position.Row, position.Column]; }
            set { pieces[position.Row, position.Column] = value; }
        }

        public bool IsEmpty(Position position)
        {
            return this[position] == null;
        }

        public static bool IsInside(Position position)
        {
            return position.Row >= 0 && position.Row < RowCount &&
                position.Column >= 0 && position.Column < ColumnCount;
        }

        public static bool IsInColorRange(Position position, PieceColor color)
        {
            if (!IsInside(position))
            {
                return false;
            }

            return color switch
            {
                PieceColor.Black => position.Row <= 4,
                PieceColor.Red => position.Row >= 5,
                _ => false
            };
        }

        public static bool IsInNinePalaces(Position position, PieceColor color)
        {
            if (!IsInside(position) || !(position.Column >= 3 && position.Column <= 5))
            {
                return false;
            }

            return color switch
            {
                PieceColor.Black => position.Row >= 0 && position.Row <= 2,
                PieceColor.Red => position.Row >= 7 && position.Row <= 9,
                _ => false
            };
        }

        public static Board Initial()
        {
            Board board = new();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Chariot(PieceColor.Black);
            this[0, 1] = new Horse(PieceColor.Black);
            this[0, 2] = new Elephant(PieceColor.Black);
            this[0, 3] = new Advisor(PieceColor.Black);
            this[0, 4] = new General(PieceColor.Black);
            this[0, 5] = new Advisor(PieceColor.Black);
            this[0, 6] = new Elephant(PieceColor.Black);
            this[0, 7] = new Horse(PieceColor.Black);
            this[0, 8] = new Chariot(PieceColor.Black);

            this[2, 1] = new Cannon(PieceColor.Black);
            this[2, 7] = new Cannon(PieceColor.Black);
            this[3, 0] = new Soldier(PieceColor.Black);
            this[3, 2] = new Soldier(PieceColor.Black);
            this[3, 4] = new Soldier(PieceColor.Black);
            this[3, 6] = new Soldier(PieceColor.Black);
            this[3, 8] = new Soldier(PieceColor.Black);

            this[9, 0] = new Chariot(PieceColor.Red);
            this[9, 1] = new Horse(PieceColor.Red);
            this[9, 2] = new Elephant(PieceColor.Red);
            this[9, 3] = new Advisor(PieceColor.Red);
            this[9, 4] = new General(PieceColor.Red);
            this[9, 5] = new Advisor(PieceColor.Red);
            this[9, 6] = new Elephant(PieceColor.Red);
            this[9, 7] = new Horse(PieceColor.Red);
            this[9, 8] = new Chariot(PieceColor.Red);

            this[7, 1] = new Cannon(PieceColor.Red);
            this[7, 7] = new Cannon(PieceColor.Red);
            this[6, 0] = new Soldier(PieceColor.Red);
            this[6, 2] = new Soldier(PieceColor.Red);
            this[6, 4] = new Soldier(PieceColor.Red);
            this[6, 6] = new Soldier(PieceColor.Red);
            this[6, 8] = new Soldier(PieceColor.Red);
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    Position position = new(row, column);

                    if (!IsEmpty(position))
                    {
                        yield return position;
                    }
                }
            }
        }

        public IEnumerable<Piece> AllPieces()
        {
            return PiecePositions().Select(position => this[position]);
        }

        public Position GetPiecePosition(Piece piece)
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    if (this[row, column] == piece)
                    {
                        return new Position(row, column);
                    }
                }
            }

            return null;
        }

        public IEnumerable<Position> PiecePositionsFor(PieceColor color)
        {
            return PiecePositions().Where(position => this[position].Color == color);
        }

        public bool IsInCheck(PieceColor color)
        {
            return PiecePositionsFor(color.Opponent()).Any(position =>
            {
                Piece piece = this[position];

                return piece.CanCaptureOpponentGeneral(position, this);
            });
        }

        public Board Copy()
        {
            Board copy = new();

            foreach (Position position in PiecePositions())
            {
                copy[position] = this[position].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new();

            foreach (Position position in PiecePositions())
            {
                Piece piece = this[position];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsGeneralVGeneral(counting);
        }

        private static bool IsGeneralVGeneral(Counting counting)
        {
            return counting.TotalCount == 2;
        }
    }
}