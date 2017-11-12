using System;

namespace ELTE.Forms.Sudoku.Model
{
    public class Line
    {
        public TableGridPoint StartPoint { get; set; }
        public TableGridPoint EndPoint { get; set; }

        public Line(TableGridPoint startPoint, TableGridPoint endPoint)
        {
            this.StartPoint = startPoint ?? throw new ArgumentNullException(nameof(startPoint));
            this.EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
        }
    }
}