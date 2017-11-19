using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace BoxGame
{
	/// <summary>
	/// A class that can draw itself onto a Graphics instance.
	/// </summary>
	public interface IRenderable
	{
		
	}

	public interface IPlayer
	{
        string GetDisplayName();

        string GetInitials();

        Color GetColor();

        int GetScore();
        IMove GetNextMove(IGameBoard board);
	}

	public interface IGame
	{
        IPlayer[] GetPlayers();
        IGameBoard GetBoard();
        void Start(IGraphicProvider gfx);
	}

	public interface IGameBoard : IRenderable
	{
        int GetRows();

        int GetColumns();

        ICorner[] this[int index]
		{
			get;
		}
	}

	public interface IMove : IRenderable, IComparable<IMove>
	{
        ILine GetLine();
        IPlayer GetPlayer();
    }

	public interface ICorner : IRenderable, IComparable<ICorner>
	{
        Point GetLocation();
    }

	public interface ILine: IComparable<ILine>
	{
        ICorner GetStart();

        ICorner GetEnd();
    }

	interface IBox : IRenderable
	{
        ILine GetTop();

        ILine GetBottom();

        ILine GetLeft();

        ILine GetRight();
    }

	public interface IGraphicProvider
	{
		void Invalidate();
		void DrawLine(Point start, Point end, Color color);
		void DrawRectangle(Rectangle rect, Color color);
		void DrawImage(Image img, int x, int y);
		void DrawString(Point loc, Font font, Color color, string str);
		void ShowUserTurn(IPlayer player);

        int GetDpiX();

        int GetDpiY();

        System.Drawing.Imaging.PixelFormat GetPixelFormat();
    }

	public interface ICornerMapper
	{
		Point GetGraphicsPoint(ICorner corner);
		ICorner ClosestCornerFromGraphicsPoint(Point point);
		Size GetMinimumGraphicsSize(int DpiX, int DpiY);
	}
}
