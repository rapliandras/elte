using System.Drawing;
using BoxGame.Persistence;


namespace BoxGame
{
    public abstract class Player :IPlayer
	{
		string m_Name;
		string m_Initials;
		Color m_Color;
		int m_Score;

		protected Player(string name, string initials, Color color)
		{
			m_Name = name;
			m_Initials = initials;
			m_Color = color;
		}

        public string GetDisplayName()
        {
            return m_Name;
        }

        public string GetInitials()
        {
            return m_Initials;
        }

        public Color GetColor()
        {
            return m_Color;
        }

        public int GetScore()
        {
            return m_Score;
        }

        public IMove GetNextMove(IGameBoard board)
		{
			return GetNextMove_Impl(board);
		}

		protected abstract IMove GetNextMove_Impl(IGameBoard board);

		public int AddScore(int score)
		{
			return (m_Score += score);
		}

		public int AddScore(MoveScore score)
		{
			if(score == MoveScore.One)
			{
				m_Score++;
			}
			else if(score == MoveScore.Two)
			{
				m_Score += 2;
			}

			return m_Score;
		}
	}
}
