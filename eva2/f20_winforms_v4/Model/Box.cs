﻿using System.Drawing;


namespace BoxGame
{
    class Box : IBox
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

		public ILine Top
		{
			get { return m_Top; }
		}

		public ILine Bottom
		{
			get { return m_Bottom; }
		}

		public ILine Left
		{
			get { return m_Left; }
		}

		public ILine Right
		{
			get { return m_Right; }
		}

        public ICornerMapper Mapper
        {
            get { return m_Mapper;  }
        }

        public IPlayer Player
        {
            get { return m_Player; }
        }


		public void Render(IGraphicProvider gfx)
		{
			Point ul = m_Mapper.GetGraphicsPoint(m_Top.Start);
			Point lr = m_Mapper.GetGraphicsPoint(m_Right.End);

			Rectangle bounds = new Rectangle(ul, new Size(lr.X - ul.X, lr.Y - ul.Y));
            
            if(m_Player.Initials == "P1")
            {
                gfx.DrawImage(Properties.Resources.red, lr.X - 48, lr.Y - 48);
            }
            else
            {
                gfx.DrawImage(Properties.Resources.blue, lr.X - 48, lr.Y - 48);
            }

        }

	}
}
