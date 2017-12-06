using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace BoxGame.Model
{
	public class Move : IMove
	{
		Line m_Line;
		Player m_Player;

		public Move(Line line)
			: this(line, null)
		{
		}

		public Move(Line line, Player player)
		{
			m_Line = line;
			m_Player = player;
		}

        public ILine GetLine()
        {
            return m_Line;
        }

        public IPlayer GetPlayer()
        {
            return m_Player;
        }

        public void SetPlayer(Player p)
		{
			m_Player = p;
		}


        public int CompareTo(IMove other)
        {
            return GetLine().CompareTo(other.GetLine());
        }
	}
}
