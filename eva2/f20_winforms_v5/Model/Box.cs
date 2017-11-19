namespace BoxGame
{
    public class Box : IBox
	{
		Line m_Top;
		Line m_Bottom;
		Line m_Left;
		Line m_Right;

		ICornerMapper m_Mapper;
		Player m_Player;

		public Box(Corner ul, Corner ur, Corner lr, Corner ll, Player p, ICornerMapper cm)
			: this(new Line(ul, ur), new Line(ur, lr), new Line(ll, lr), new Line(ll, ul), p, cm)
		{
		}

		public Box(Line top, Line right, Line bottom, Line left, Player p, ICornerMapper cm)
		{
			m_Top = top;
			m_Bottom = bottom;
			m_Left = left;
			m_Right = right;
			m_Player = p;
			m_Mapper = cm;
		}

        public ILine GetTop()
        {
            return m_Top;
        }

        public ILine GetBottom()
        {
            return m_Bottom;
        }

        public ILine GetLeft()
        {
            return m_Left;
        }

        public ILine GetRight()
        {
            return m_Right;
        }

        public ICornerMapper Mapper => m_Mapper;

        public IPlayer Player => m_Player;

    }
}
