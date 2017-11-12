using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ELTE.Forms.Sudoku.Model;

namespace ELTE.Forms.Sudoku.Test
{
    [TestClass]
    public class SudokuGameModelTest
    {
        private SudokuGameModel _model; // a tesztelendő modell

        [TestInitialize]
        public void Initialize()
        {
            _model = new SudokuGameModel(null);
            // perzisztencia nélküli modellt hozunk létre

            _model.GameAdvanced += new EventHandler<SudokuEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<SudokuEventArgs>(Model_GameOver);
        }


        [TestMethod]
        public void SudokuGameModelNewGameMediumTest()
        {
            _model.NewGame();

            Assert.AreEqual(0, _model.GameStepCount); // még nem léptünk
            Assert.AreEqual(GameDifficulty.Medium, _model.GameDifficulty); // a nehézség beállítódott
            Assert.AreEqual(1200, _model.GameTime); // alapból ennyi időnk van

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < 9; i++)
                for (Int32 j = 0; j < 9; j++)
                    if (_model.Table.IsEmpty(i, j))
                        emptyFields++;

            Assert.AreEqual(69, emptyFields); // szabad mezők száma is megfelelő
        }

        [TestMethod]
        public void SudokuGameModelNewGameEasyTest()
        {
            _model.GameDifficulty = GameDifficulty.Easy;
            _model.NewGame();

            Assert.AreEqual(0, _model.GameStepCount); // még nem léptünk
            Assert.AreEqual(GameDifficulty.Easy, _model.GameDifficulty); // a nehézség beállítódott
            Assert.AreEqual(3600, _model.GameTime); // alapból ennyi időnk van

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < 9; i++)
                for (Int32 j = 0; j < 9; j++)
                    if (_model.Table.IsEmpty(i, j))
                        emptyFields++;

            Assert.AreEqual(75, emptyFields); // szabad mezők száma is megfelelő
        }

        [TestMethod]
        public void SudokuGameModelNewGameHardTest()
        {
            _model.GameDifficulty = GameDifficulty.Hard;
            _model.NewGame();

            Assert.AreEqual(0, _model.GameStepCount); // még nem léptünk
            Assert.AreEqual(GameDifficulty.Hard, _model.GameDifficulty); // a nehézség beállítódott
            Assert.AreEqual(600, _model.GameTime); // alapból ennyi időnk van

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < 9; i++)
                for (Int32 j = 0; j < 9; j++)
                    if (_model.Table.IsEmpty(i, j))
                        emptyFields++;

            Assert.AreEqual(63, emptyFields); // szabad mezők száma is megfelelő
        }

        [TestMethod]
        public void SudokuGameModelStepTest()
        {
            Assert.AreEqual(0, _model.GameStepCount); // még nem léptünk

            _model.Step(0, 0);
            
            Assert.AreEqual(0, _model.GameStepCount); // mivel a játék áll, nem szabad, hogy lépjünk

            _model.NewGame();

            Random random = new Random();
            Int32 x = 0, y = 0;
            do
            {
                x = random.Next(0, 9);
                y = random.Next(0, 9);
            }
            while (!_model.Table.IsEmpty(x, y));

            _model.Step(x, y);

            Assert.AreEqual(1, _model.GameStepCount); // most már léptünk
            Assert.AreEqual(1, _model.Table[x, y]); // 1-esnek kell lennie a táblán
            Assert.AreEqual(1200, _model.GameTime); // az idő viszont nem változott

            Int32 currentValue = 1;
            for (Int32 i = 2; i < 1E6; i++) // egymillió lépés végrehajtása
            {
                _model.Step(x, y);
                Assert.IsTrue(currentValue < _model.Table[x, y] || _model.Table[x, y] == 0); // az értékeknek ciklikusan kell váltakozniuk
                Assert.AreEqual(i, _model.GameStepCount); // akárhányszor léphetünk a mezóre

                currentValue = _model.Table[x, y];
            }
        }

        [TestMethod]
        public void SudokuGameModelAdvanceTimeTest()
        {
            _model.NewGame();

            Int32 time = _model.GameTime;
            while (!_model.IsGameOver)
            {
                _model.AdvanceTime(); 

                time--;

                Assert.AreEqual(time, _model.GameTime); // az idő csökkent
                Assert.AreEqual(0, _model.GameStepCount); // de a lépésszám nem változott
            }

            Assert.AreEqual(0, _model.GameTime); // a játék végére elfogyott a játékidő
        }

        private void Model_GameAdvanced(Object sender, SudokuEventArgs e)
        {
            Assert.IsTrue(_model.GameTime >= 0); // a játékidő nem lehet negatív
            Assert.AreEqual(_model.GameTime == 0, _model.IsGameOver); // a tesztben a játéknak csak akkor lehet vége, ha lejárt az idő

            Assert.AreEqual(e.GameStepCount, _model.GameStepCount); // a két értéknek egyeznie kell
            Assert.AreEqual(e.GameTime, _model.GameTime); // a két értéknek egyeznie kell
            Assert.IsFalse(e.IsWon); // még nem nyerték meg a játékot
        }

        private void Model_GameOver(Object sender, SudokuEventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver); // biztosan vége van a játéknak
            Assert.AreEqual(0, e.GameTime); // a tesztben csak akkor váltódhat ki, ha elfogy az idő
            Assert.IsFalse(e.IsWon);
        }

    }
}
