using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using BoxGame.Persistence;


namespace BoxGame
{
	class Game : IGame
	{
		List<Player> m_Players;
		GameBoard m_GameBoard;

		int m_CurrentPlayer = 0;
		int m_RewardLines = 1;
		int m_MovesPerTurn = 1;

		bool m_FreeMoveOnScore = true;

        public bool GetFreeMoveOnScore() => m_FreeMoveOnScore;

        public void SetFreeMoveOnScore(bool value) => m_FreeMoveOnScore = value;

        public Game(IPlayer[] players, GameBoard board)
		{
			m_GameBoard = board;
			m_Players = new List<Player>(players.Length);
			foreach (Player p in players)
			{
				m_Players.Add(p);
			}
		}

        public GameBoard Board => m_GameBoard;

        private Player CurrentPlayer => m_Players[m_CurrentPlayer];

        private Player NextPlayer()
		{
			m_CurrentPlayer++;
			if (m_CurrentPlayer >= m_Players.Count)
			{
				m_CurrentPlayer = 0;
			}

			return CurrentPlayer;
		}

        public IPlayer[] GetPlayers() => m_Players.ToArray();

        public IGameBoard GetBoard() => m_GameBoard;

        public void Start(IGraphicProvider gfx)
		{
			m_GameBoard.Render();

			int playerMovesRemaining = m_MovesPerTurn;
			while (m_GameBoard.MovesRemaining > 0)
			{
				IMove move = null;
				while (true)
				{
					gfx.ShowUserTurn(CurrentPlayer);
					move = CurrentPlayer.GetNextMove(m_GameBoard);
					if (m_GameBoard.IsAvailableMove(move))
					{
						break;
					}
				}

				MoveScore s = m_GameBoard.MakeMove(move, CurrentPlayer);
				playerMovesRemaining--;

				if (GetFreeMoveOnScore())
				{
					if (s == MoveScore.One || s == MoveScore.Two)
					{
						playerMovesRemaining += m_RewardLines;
					}
				}

				m_GameBoard.Render();

				if (playerMovesRemaining == 0)
				{
					NextPlayer();
					playerMovesRemaining = m_MovesPerTurn;
				}
			}

			StringBuilder sb = new StringBuilder();
			foreach (Player p in m_Players)
			{
				sb.AppendFormat("{0}: {1}\n", p.GetDisplayName(), p.GetScore());
			}

			System.Windows.Forms.MessageBox.Show(sb.ToString());
		}
	}
}
