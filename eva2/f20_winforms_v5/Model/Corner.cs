using System;
using System.Drawing;


namespace BoxGame
{
    public class Corner : ICorner
	{
		Point m_Location;
		ICornerMapper m_Mapper;
		Color m_Color;

		public Corner(Point location, ICornerMapper mapper, Color color)
		{
			m_Location = location;
			m_Mapper = mapper;
			m_Color = color;
		}

        public Color Color => m_Color;

        public Point GetLocation()
        {
            return m_Location;
        }

        public bool Equals(ICorner other) => m_Location == other.GetLocation();

        public int CompareTo(ICorner other)
		{
			// Order matters here

			if (m_Location.Y < other.GetLocation().Y)
			{
				return -1;
			}

			if (m_Location.Y > other.GetLocation().Y)
			{
				return 1;
			}

			if (m_Location.X < other.GetLocation().X)
			{
				return -1;
			}

			if (m_Location.X > other.GetLocation().X)
			{
				return 1;
			}

			return 0;
		}


        internal Point GraphicsPoint => m_Mapper.GetGraphicsPoint(this);

        internal bool IsAdjacentTo(ICorner c)
		{
			bool bOk = false;

			if (!Equals(c))
			{
				if (GetLocation().X == c.GetLocation().X)
				{
					bOk = (Math.Abs(GetLocation().Y - c.GetLocation().Y) == 1);
				}
				else if (GetLocation().Y == c.GetLocation().Y)
				{
					bOk = (Math.Abs(GetLocation().X - c.GetLocation().X) == 1);
				}
			}

			return bOk;
		}
	}
}
