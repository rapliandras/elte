namespace ELTE.Forms.Sudoku.Model
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Direction GetDirectionFromAdjacentPoint(Node OtherPoint)
        {
            if(this.X + 1 == OtherPoint.X)
            {
                return Direction.Left;
            }

            if (this.X - 1 == OtherPoint.X)
            {
                return Direction.Right;
            }

            if (this.Y + 1 == OtherPoint.Y)
            {
                return Direction.Down;
            }

            if (this.X - 1 == OtherPoint.X)
            {
                return Direction.Up;
            }

            return Direction.Down;
            //throw new System.Exception("The points are not adjacent.");
        }
    }
}