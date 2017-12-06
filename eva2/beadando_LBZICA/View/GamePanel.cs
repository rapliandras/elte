using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using BoxGame.Persistence;
using BoxGame.Model;

namespace BoxGame.View
{
    /// <summary>
    /// Summary description for form.
    /// </summary>
    public partial class GamePanel : System.Windows.Forms.Form
	{
		Game m_BoxGame;
		Thread m_GameThread;
        GraphicsProvider gfx;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public GamePanel()
		{
			InitializeComponent();

		}

		internal void Initialize(Game g)
		{
            gfx = new GraphicsProvider(inkPanel, this);

			m_BoxGame = g;
            m_BoxGame.Board.BoxRenderable += new EventHandler<GameEventArgs>(RenderBox);
            m_BoxGame.Board.MoveRenderable += new EventHandler<GameEventArgs>(RenderMove);
            m_BoxGame.Board.DotRenderable += new EventHandler<GameEventArgs>(RenderDot);
            m_BoxGame.Board.BackgroundRenderable += new EventHandler<GameEventArgs>(RenderBackground);
            m_BoxGame.Board.ReadyToInvalidate += new EventHandler<GameEventArgs>(InvalidateBoard);


        }

        private static void SetBGColor(Bitmap b, Color c)
        {
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    b.SetPixel(x, y, c);
                }
            }
        }

        public Size GetMinimumGraphicsSize(int DpiX, int DpiY)
        {
            int SpacingX = DpiX / m_BoxGame.Board.m_PointsPerInch;
            int SpacingY = DpiY / m_BoxGame.Board.m_PointsPerInch;
            int width = m_BoxGame.Board.m_Columns * SpacingX + SpacingX;
            int height = m_BoxGame.Board.m_Rows * SpacingY + SpacingY;

            return new Size(width, height);
        }
        private Bitmap GenerateEmptyBackground(Color bgColor)
        {
            Size s = GetMinimumGraphicsSize(gfx.GetDpiX(), gfx.GetDpiY());
            Bitmap b = new Bitmap(s.Width, s.Height, gfx.GetPixelFormat());
            SetBGColor(b, bgColor);

            return b;
        }

        private void RenderBackground(object sender, GameEventArgs e)
        {
            Bitmap b = GenerateEmptyBackground(Color.White);
            gfx.DrawImage(b, 0, 0);
            this.m_BoxGame.Board.bgRendered = true;
            
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

			m_BoxGame.Board.SetGraphicsProvider(new GraphicsProvider(inkPanel, this));

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

            Point ul = b.Mapper.GetGraphicsPoint(b.GetTop().GetStart());
            Point lr = b.Mapper.GetGraphicsPoint(b.GetRight().GetEnd());

            Rectangle bounds = new Rectangle(ul, new Size(lr.X - ul.X, lr.Y - ul.Y));

            if (b.Player.GetInitials() == "P1")
            {
                gfx.DrawImage(Properties.Resources.red, lr.X - 48, lr.Y - 48);
            }
            else
            {
                gfx.DrawImage(Properties.Resources.blue, lr.X - 48, lr.Y - 48);
            }
        }

        private void RenderMove(object sender, GameEventArgs e)
        {
            Move m = (Move)e._object;

            Point start = (m.GetLine().GetStart() as Corner).GraphicsPoint;
            Point end = (m.GetLine().GetEnd() as Corner).GraphicsPoint;

            gfx.DrawLine(start, end, m.GetPlayer().GetColor());
        }

        private void RenderDot(object sender, GameEventArgs e)
        {
            Corner c = (Corner)e._object;

            c.GraphicsPoint.Offset(-1, -1);
            gfx.DrawRectangle(new Rectangle(c.GraphicsPoint, new Size(3, 3)), c.Color);
        }

        private void InvalidateBoard(object sender, GameEventArgs e)
        {
            gfx.Invalidate();
        }
    }
}

