using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoxGame;
using BoxGame.Model;
using BoxGame.View;
using BoxGame.Persistence;
using System.Collections.Generic;

namespace BoxGameTest
{
    [TestClass]
    public class UnitTest1
    {
        private GameBoard _gameBoard;
        private GamePanel _gamePanel;

        [TestInitialize]
        public void Initialize()
        {
            _gameBoard = new GameBoard(3, 3);
            _gamePanel = new GamePanel();
            _gamePanel.InitializeComponent();
            
        }

        [TestMethod]
        public void TestGameBoardHasBeenInitialized()
        {
            Assert.IsNotNull(_gameBoard);
            Assert.AreEqual(3, _gameBoard.m_Columns);
            Assert.AreEqual(3, _gameBoard.m_Rows);
            Assert.AreNotEqual(null, _gameBoard.m_PossibleLines);
            Assert.AreEqual(2, _gameBoard.m_PointsPerInch);
        }

        [TestMethod]
        public void TestMove()
        {
            Line L = new Line(
                new Corner(new System.Drawing.Point(1, 1), _gameBoard, System.Drawing.Color.Black),
                new Corner(new System.Drawing.Point(1, 2), _gameBoard, System.Drawing.Color.Black)
                );

            Player P = new InkInputPlayer("andrew","P1", System.Drawing.Color.Blue, _gamePanel.inkPanel, _gameBoard);

            Move M = new Move(L, P);

            _gameBoard.MakeMove(M, P);

            Assert.IsNotNull(_gameBoard.Moves[0]);

            Assert.AreEqual(
                _gameBoard.Moves[0].GetLine().GetStart().GetLocation().X, 
                M.GetLine().GetStart().GetLocation().X
            );

            Assert.AreEqual(
                _gameBoard.Moves[0].GetLine().GetStart().GetLocation().Y,
                M.GetLine().GetStart().GetLocation().Y
            );

            Assert.AreEqual(_gameBoard.Moves[0].GetPlayer().GetDisplayName(), "andrew");

            Assert.AreEqual(_gameBoard.Moves[0].GetPlayer().GetInitials(), "P1");

        }

        [TestMethod]
        public void TestBox()
        {
            Line L1 = new Line(
                new Corner(new System.Drawing.Point(1, 1), _gameBoard, System.Drawing.Color.Black),
                new Corner(new System.Drawing.Point(1, 2), _gameBoard, System.Drawing.Color.Black)
                );

            Line L2 = new Line(
                new Corner(new System.Drawing.Point(1, 2), _gameBoard, System.Drawing.Color.Black),
                new Corner(new System.Drawing.Point(2, 2), _gameBoard, System.Drawing.Color.Black)
                );

            Line L3 = new Line(
                new Corner(new System.Drawing.Point(2, 2), _gameBoard, System.Drawing.Color.Black),
                new Corner(new System.Drawing.Point(2, 1), _gameBoard, System.Drawing.Color.Black)
                );

            Line L4 = new Line(
                new Corner(new System.Drawing.Point(2, 1), _gameBoard, System.Drawing.Color.Black),
                new Corner(new System.Drawing.Point(1, 1), _gameBoard, System.Drawing.Color.Black)
                );

            Player P1 = new InkInputPlayer("andrew", "P1", System.Drawing.Color.Blue, _gamePanel.inkPanel, _gameBoard);
            Player P2 = new InkInputPlayer("johndoe", "P2", System.Drawing.Color.Red, _gamePanel.inkPanel, _gameBoard);

            Move M1 = new Move(L1, P1);
            Move M2 = new Move(L2, P2);
            Move M3 = new Move(L3, P1);
            Move M4 = new Move(L4, P2);

            _gameBoard.MakeMove(M1, P1);
            _gameBoard.MakeMove(M2, P2);
            _gameBoard.MakeMove(M3, P1);
            _gameBoard.MakeMove(M4, P2);

            //Assert that the fourth move has created a box.
            Assert.IsNotNull(_gameBoard.BoxesCreatedByMove(M4, P2));
           
        }

    }
}
