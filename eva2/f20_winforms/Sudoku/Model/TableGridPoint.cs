namespace ELTE.Forms.Sudoku.Model
{
    public class TableGridPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public TableGridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}