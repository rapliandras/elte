using ELTE.Forms.Sudoku.Model;
using ELTE.Forms.Sudoku.Persistence;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace ELTE.Forms.Sudoku.View
{
    /// <summary>
    /// Játékablak típusa.
    /// </summary>
    public partial class GameForm : Form
    {
        #region Fields

        private ISudokuDataAccess _dataAccess; // adatelérés
        private SudokuGameModel _model; // játékmodell
        private Button[,] _buttonGrid; // gombrács
        private Timer _timer; // időzítő

        #endregion

        #region Constructors

        /// <summary>
        /// Játékablak példányosítása.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form event handlers

        /// <summary>
        /// Játékablak betöltésének eseménykezelője.
        /// </summary>
        private void GameForm_Load(Object sender, EventArgs e)
        {
            // adatelérés példányosítása
            _dataAccess = new SudokuFileDataAccess();

            // modell létrehozása és az eseménykezelők társítása
            _model = new SudokuGameModel(_dataAccess);
            _model.GameAdvanced += new EventHandler<SudokuEventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<SudokuEventArgs>(Game_GameOver);

            // időzítő létrehozása
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);

            // játéktábla és menük inicializálása
            GenerateTable();
            SetupMenus();

            // új játék indítása
            _model.NewGame();
            SetupTable();

            _timer.Start();
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Game_GameAdvanced(Object sender, SudokuEventArgs e)
        {
            _toolLabelGameTime.Text = TimeSpan.FromSeconds(e.GameTime).ToString("g");
            _toolLabelGameSteps.Text = e.GameStepCount.ToString();
            // játékidő frissítése
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Game_GameOver(Object sender, SudokuEventArgs e)
        {
            _timer.Stop();

            foreach (Button button in _buttonGrid) // kikapcsoljuk a gombokat
                button.Enabled = false;

            _menuFileSaveGame.Enabled = false;

            if (e.IsWon) // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show("Gratulálok, győztél!" + Environment.NewLine +
                                "Összesen " + e.GameStepCount + " lépést tettél meg és " +
                                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                                "Sudoku játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Sajnálom, vesztettél, lejárt az idő!",
                                "Sudoku játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
        }

        #endregion

        #region Grid event handlers

        /// <summary>
        /// Gombrács eseménykezelője.
        /// </summary>
        private void ButtonGrid_MouseClick(Object sender, MouseEventArgs e)
        {
            Color PlayerColor = _model.GameStepCount % 2 == 0 ? Color.FromArgb(255, 0, 0) : Color.FromArgb(0, 255, 255);

            // a TabIndex-ből megkapjuk a sort és oszlopot
            Int32 currentLine = ((sender as Button).TabIndex - 100) / _model.Table.Size;
            Int32 currentColumn = ((sender as Button).TabIndex - 100) % _model.Table.Size;

            Console.WriteLine(currentLine);
            Console.WriteLine(currentColumn);
            Console.WriteLine("Currently selected:");
            Console.WriteLine(_model.CurrentlySelectedTileX);
            Console.WriteLine(_model.CurrentlySelectedTileY);
            Console.WriteLine((sender as Button).TabIndex);

            DrawLineOnTileClick(PlayerColor, currentLine, currentColumn);

        }

        private void DrawSquareOnTileClick(Color playerColor)
        {

            Edge MostRecentEdge = _model.EdgeList[_model.EdgeList.Count - 1];

            int HorizontalEdgeCount = 0;
            int VerticalEdgeCount = 0;

            List<Edge> NeighbourEdges = new List<Edge>();

            foreach (Edge E in _model.EdgeList)
            {
                
                Edge CurrentEdge = E;

                // eseményes él szomszédjai
                NeighbourEdges = _model.NeighboursForEdge(CurrentEdge);

                // amíg vannak szomszédjai
                while (NeighbourEdges.Count() > 0)
                {
                    NeighbourEdges = _model.NeighboursForEdge(NeighbourEdges.Last());
                    NeighbourEdges.RemoveAt(NeighbourEdges.Count - 1);
                }
             
            }
        }

        private void DrawLineOnTileClick(Color PlayerColor, int currentLine, int currentColumn)
        {
            if (_model.IsAnyFieldSelected())
            {
                _buttonGrid[
                    _model.CurrentlySelectedTileX,
                    _model.CurrentlySelectedTileY
                    ].BackgroundImage = (Image)Properties.Resources.tile;


                if (currentLine == _model.CurrentlySelectedTileX)
                {
                    if (currentColumn > _model.CurrentlySelectedTileY)
                    {
                        DrawLine(PlayerColor, Direction.Right);
                        _model.GameStepCount++;
                    }

                    if (currentColumn < _model.CurrentlySelectedTileY)
                    {
                        DrawLine(PlayerColor, Direction.Left);
                        _model.GameStepCount++;
                    }
                }


                if (currentColumn == _model.CurrentlySelectedTileY)
                {
                    if (currentLine > _model.CurrentlySelectedTileX)
                    {
                        DrawLine(PlayerColor, Direction.Down);
                        _model.GameStepCount++;
                    }

                    if (currentLine < _model.CurrentlySelectedTileX)
                    {
                        DrawLine(PlayerColor, Direction.Up);
                        _model.GameStepCount++;
                    }

                }

                _model.EdgeList.Add(new Edge(new TableGridPoint(1, 2), new TableGridPoint(3, 4)));

                _model.ClearSelectedTiles();
                DrawSquareOnTileClick(PlayerColor);


            }
            else
            {
                _model.Select(currentLine, currentColumn);
                _buttonGrid[currentLine, currentColumn].BackgroundImage = (Image)Properties.Resources.orange_tile;

            }
        }

        private void DrawLine(Color C, Direction D)
        {
            Panel Line = new Panel
            {
                BackColor = C,
                Visible = true,
                AutoSize = true
            };

            if (Direction.Right == D)
            {
                Line.Width = 50;
                Line.Height = 10;
                Line.Location = new Point((_model.CurrentlySelectedTileY + 1) * 50 - 20, (_model.CurrentlySelectedTileX + 1) * 50 - 5);
            }

            if (Direction.Left == D)
            {
                Line.Width = 50;
                Line.Height = 10;
                Line.Location = new Point((_model.CurrentlySelectedTileY + 1) * 50 - 70, (_model.CurrentlySelectedTileX + 1) * 50 - 5);
            }

            if (Direction.Down == D)
            {
                Line.Width = 10;
                Line.Height = 50;
                Line.Location = new Point((_model.CurrentlySelectedTileY + 1) * 50 - 25, (_model.CurrentlySelectedTileX + 1) * 50);
            }

            if (Direction.Up == D)
            {
                Line.Width = 10;
                Line.Height = 50;
                Line.Location = new Point((_model.CurrentlySelectedTileY + 1) * 50 - 25, (_model.CurrentlySelectedTileX + 1) * 50 - 50);
            }

            Controls.Add(Line);
            Controls.SetChildIndex(Line, 4);

        }

        #endregion

        #region Menu event handlers

        /// <summary>
        /// Új játék eseménykezelője.
        /// </summary>
        private void MenuFileNewGame_Click(Object sender, EventArgs e)
        {
            _menuFileSaveGame.Enabled = true;

            _model.NewGame();
            SetupTable();

            _timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (SudokuDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuFileSaveGame.Enabled = true;
                }

                SetupTable();
            }

            if (restartTimer)
                _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (SudokuDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (restartTimer)
                _timer.Start();
        }

        /// <summary>
        /// Kilépés eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuFileExit_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();
            
            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Sudoku játék", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
            else
            {
                if (restartTimer)
                    _timer.Start();
            }
        }

        private void MenuGameEasy_Click(Object sender, EventArgs e)
        {
            _model.GameDifficulty = GameDifficulty.Easy;
        }

        private void MenuGameMedium_Click(Object sender, EventArgs e)
        {
            _model.GameDifficulty = GameDifficulty.Medium;
        }

        private void MenuGameHard_Click(Object sender, EventArgs e)
        {
            _model.GameDifficulty = GameDifficulty.Hard;
        }

        #endregion

        #region Timer event handlers

        /// <summary>
        /// Időzítő eseménykeztelője.
        /// </summary>
        private void Timer_Tick(Object sender, EventArgs e)
        {
            _model.AdvanceTime(); // játék léptetése
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Új tábla létrehozása.
        /// </summary>
        private void GenerateTable()
        {
            _buttonGrid = new Button[_model.Table.Size, _model.Table.Size];
            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 25 + 50 * i); // elhelyezkedés
                    _buttonGrid[i, j].BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("tile"); //
                    _buttonGrid[i, j].BackColor = Color.Transparent;
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betűtípus
                    _buttonGrid[i, j].Enabled = false; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.Table.Size + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelő hozzárendelése minden gombhoz

                    Controls.Add(_buttonGrid[i, j]);
                    // felevesszük az ablakra a gombot
                }
        }

        /// <summary>
        /// Tábla beállítása.
        /// </summary>
        private void SetupTable() 
        {
            for (Int32 i = 0; i < _buttonGrid.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _buttonGrid.GetLength(1); j++)
                {
                    if (_model.Table.IsEmpty(i, j)) // ha nincs kitöltve a mező
                    {
                        _buttonGrid[i, j].Text = String.Empty;
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    else // ha ki van töltve
                    {
                        _buttonGrid[i, j].Text = _model.Table[i, j].ToString();
                        _buttonGrid[i, j].Enabled = false; // gomb bekapcsolása
                        _buttonGrid[i, j].BackColor = Color.Yellow; 
                            // háttérszín sárga, ha zárolni kell a mezőt, különben fehér
                    }
                }
            }

            _toolLabelGameSteps.Text = _model.GameStepCount.ToString();
            _toolLabelGameTime.Text = TimeSpan.FromSeconds(_model.GameTime).ToString("g");
        }

        /// <summary>
        /// Menük beállítása.
        /// </summary>
        private void SetupMenus()
        {
            _menuGameEasy.Checked = (_model.GameDifficulty == GameDifficulty.Easy);
            _menuGameMedium.Checked = (_model.GameDifficulty == GameDifficulty.Medium);
            _menuGameHard.Checked = (_model.GameDifficulty == GameDifficulty.Hard);
        }

        #endregion
    }
}
