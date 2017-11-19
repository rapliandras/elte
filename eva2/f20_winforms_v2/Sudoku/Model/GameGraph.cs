using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.Forms.Sudoku.Model
{
    public class GameGraph : List<Edge>
    {

        public  IEnumerable<Node> DepthFirstTraversal( GameGraph graph, Node start)
        {
            var visited = new HashSet<Node>();
            var stack = new Stack<Node>();

            stack.Push(start);

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current))
                {
                    continue;
                }

                yield return current;

                //Console.WriteLine(visited.Count());
                var neighbours = graph.GetNeighbours(current).Where(n => !visited.Contains(n));
                
                foreach (var neighbour in neighbours.Reverse())
                {
                    stack.Push(neighbour);
                }
            }
        }

        public List<Node> GetNeighbours(Node N)
        {
            List<Node> Result = new List<Node>();

            foreach(Edge E in this)
            {
                foreach(Node GraphNode in E.Nodes)
                {
                    if (GraphNode.NeighboursWith(N))
                    {
                        Result.Add(GraphNode);
                    }
                }
            }

            return Result;
        }
    }
}
