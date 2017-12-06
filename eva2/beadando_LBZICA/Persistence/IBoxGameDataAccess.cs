using System;
using System.Threading.Tasks;

namespace BoxGame.Persistence
{
    /// <summary>
    /// Sudoku fájl kezelő felülete.
    /// </summary>
    public interface IBoxGameDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<GameBoard> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="fileName">Elérési útvonal.</param>
        /// <param name="path">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String fileName, GameBoard path);
    }
}