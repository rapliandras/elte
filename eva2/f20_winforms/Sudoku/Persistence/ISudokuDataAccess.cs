using System;
using System.Threading.Tasks;

namespace ELTE.Forms.Sudoku.Persistence
{
    /// <summary>
    /// Sudoku fájl kezelő felülete.
    /// </summary>
    public interface ISudokuDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<SudokuTable> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="fileName">Elérési útvonal.</param>
        /// <param name="path">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String fileName, SudokuTable path);
    }
}
