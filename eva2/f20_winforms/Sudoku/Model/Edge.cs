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

        public bool ConnectsTo(Edge AnotherEdge)
        {
            foreach (Node N1 in this.Nodes)
            {
                foreach(Node N2 in AnotherEdge.Nodes)
                {
                    if(N1.X == N2.X ^ N1.Y == N2.Y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
    }
}