using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.Forms.Sudoku.Model
{
    // Undirected edge
    public class Edge
    {
        public HashSet<Node> Nodes { get; set; }
        public Player AssignedPlayer;

        public Orientation EdgeOrientation { get; set; }

        public Edge(Node startPoint, Node endPoint)
        {
            this.Nodes = new HashSet<Node>();
            this.Nodes.Add(startPoint);
            this.Nodes.Add(endPoint);

            Direction[] Horizontal = { Direction.Left, Direction.Right };

            this.EdgeOrientation = Horizontal.Contains(startPoint.GetDirectionFromAdjacentPoint(endPoint)) ? Orientation.Horizontal : Orientation.Vertical; 
        }
        
    }
}