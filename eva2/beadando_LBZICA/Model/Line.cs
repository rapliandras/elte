using System;


namespace BoxGame.Model
{
    public class Line : ILine
	{
		Corner m_Start;
		Corner m_End;

		public Line(Corner c1, Corner c2)
		{
			if (c1.IsAdjacentTo(c2))
			{
				if (c1.CompareTo(c2) < 0)
				{
					m_Start = c1;
					m_End = c2;
				}
				else
				{
					m_Start = c2;
					m_End = c1;
				}
			}
			else
			{
				throw new ArgumentException("Corners not adjacent");
			}
		}

        public ICorner GetStart()
        {
            return m_Start;
        }

        public ICorner GetEnd()
        {
            return m_End;
        }

        public bool Vertical => m_Start.GetLocation().X == m_End.GetLocation().X;

        int IComparable<ILine>.CompareTo(ILine other)
		{
            int start = GetStart().CompareTo(other.GetStart());
            int end = GetEnd().CompareTo(other.GetEnd());

            if (start < 0 || end < 0)
            {
                return -1;
            }

            if (start > 0 || end > 0)
            {
                return 1;
            }

            return 0;
        }
    }
}
