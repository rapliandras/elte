using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using BoxGame.Persistence;
using BoxGame.Model;

namespace BoxGame
{
	/// <summary>
	/// Summary description for form.
	/// </summary>
	partial class GamePanel : System.Windows.Forms.Form
	{
		Game m_BoxGame;
		Thread m_GameThread;
        GraphicsProvider gfx;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		internal GamePanel()
		{
			InitializeComponent();

		}

		internal void Initialize(Game g)
		{
            gfx = new GraphicsProvider(inkPanel, this);

			m_BoxGame = g;
            m_BoxGame.Board.BoxRenderable += new EventHandler<GameEventArgs>(RenderBox);

		}

		void StartGame(object obj)
		{
			m_BoxGame.Start(gfx);
		}

        private void GamePanel_Load(object sender, EventArgs e)
		{
			GameBoard gb = m_BoxGame.Board;
			Graphics g = Graphics.FromHwnd(inkPanel.Handle);
			Size s = gb.GetMinimumGraphicsSize((int) Math.Ceiling(g.DpiX), (int) Math.Ceiling(g.DpiY));
			inkPanel.Image = new Bitmap(s.Width, s.Height);
			inkPanel.Height = inkPanel.Image.Height + inkPanel.Margin.Vertical;
			inkPanel.Width = inkPanel.Image.Width + inkPanel.Margin.Horizontal;

            Size = new Size(inkPanel.Image.Width + inkPanel.Margin.Horizontal * 2
                , inkPanel.Image.Height + inkPanel.Margin.Vertical * 2 + 50);

			m_BoxGame.Board.GraphicsProvider = new GraphicsProvider(inkPanel, this);

			m_GameThread = new Thread(new ParameterizedThreadStart(this.StartGame));
			m_GameThread.Start(inkPanel);
		}

        private void GamePanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_GameThread.Abort();
        }

        private void RenderBox(object sender, GameEventArgs e)
        {
            Box b = (Box) e._object;

            Point ul = b.Mapper.GetGraphicsPoint(b.Top.Start);
            Point lr = b.Mapper.GetGraphicsPoint(b.Right.End);

            Rectangle bounds = new Rectangle(ul, new Size(lr.X - ul.X, lr.Y - ul.Y));

            if (b.Player.Initials == "P1")
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

