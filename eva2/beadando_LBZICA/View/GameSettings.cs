using System;
using System.Drawing;
using System.Globalization;
using BoxGame.Persistence;
using BoxGame.Model;


namespace BoxGame.View
{
    partial class GameSettings: System.Windows.Forms.Form
	{
		public GameSettings()
		{
			InitializeComponent();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			GameBoard gb = new GameBoard(
					int.Parse(cbBoardHeight.Text, CultureInfo.CurrentCulture), 
                    int.Parse(cbBoardWidth.Text, CultureInfo.CurrentCulture)
				);

			GamePanel f = new GamePanel();

			Player[] players = new Player[2];

			switch (this.cbPlayer1Type.SelectedIndex)
			{
				case 0: // human
					players[0] = new InkInputPlayer(txtPlayer1Name.Text, "P1", Color.Tomato, f.inkPanel, gb);
					break;
			}

			switch (this.cbPlayer2Type.SelectedIndex)
			{
				case 0: // human
					players[1] = new InkInputPlayer(txtPlayer2Name.Text, "P2", Color.Navy, f.inkPanel, gb);
					break;
			}

			Game g = new Game(players, gb);
			g.SetFreeMoveOnScore(cbFreeMove.Checked);
			f.Initialize(g);
			f.ShowDialog();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void GameSettings_Load(object sender, EventArgs e)
		{
			cbPlayer1Type.SelectedIndex = 0;
			cbPlayer2Type.SelectedIndex = 0;
			cbBoardHeight.SelectedIndex = 1;
			cbBoardWidth.SelectedIndex = 1;
		}
	}
}