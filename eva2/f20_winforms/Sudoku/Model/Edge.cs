using System;
using System.Linq;

namespace ELTE.Forms.Sudoku.Model
{
    public class Edge
    {
        public TableGridPoint StartPoint { get; set; }
        public TableGridPoint EndPoint { get; set; }
        public Orientation EdgeOrientation { get; set; }

        public Edge(TableGridPoint startPoint, TableGridPoint endPoint)
        {
            this.StartPoint = startPoint ?? throw new ArgumentNullException(nameof(startPoint));
            this.EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));

            Direction[] Horizontal = { Direction.Left, Direction.Right };
            this.EdgeOrientation = Horizontal.Contains(this.StartPoint.GetDirectionFromAdjacentPoint(this.EndPoint)) ? Orientation.Horizontal : Orientation.Vertical; 
        }
        
    }
}