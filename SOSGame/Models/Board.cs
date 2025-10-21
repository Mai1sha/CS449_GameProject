namespace SOSGame.Models
{
    public class Board
    {
        private readonly int _size;
        private readonly CellValue[,] _cells;

        public int Size => _size;

        public Board(int size)
        {
            if (size < 3)
                throw new ArgumentException("Board size must be at least 3", nameof(size));

            _size = size;
            _cells = new CellValue[size, size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    _cells[row, col] = CellValue.Empty;
                }
            }
        }

        public CellValue GetCell(int row, int col)
        {
            ValidateCoordinates(row, col);
            return _cells[row, col];
        }

        public bool PlaceMove(int row, int col, CellValue value)
        {
            ValidateCoordinates(row, col);

            if (value == CellValue.Empty)
                throw new ArgumentException("Cannot place empty cell", nameof(value));

            if (_cells[row, col] != CellValue.Empty)
                return false;

            _cells[row, col] = value;
            return true;
        }

        public bool IsCellEmpty(int row, int col)
        {
            ValidateCoordinates(row, col);
            return _cells[row, col] == CellValue.Empty;
        }

        public bool IsFull()
        {
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    if (_cells[row, col] == CellValue.Empty)
                        return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            InitializeBoard();
        }

        private void ValidateCoordinates(int row, int col)
        {
            if (row < 0 || row >= _size || col < 0 || col >= _size)
                throw new ArgumentOutOfRangeException($"Coordinates ({row}, {col}) out of bounds");
        }
    }
}
