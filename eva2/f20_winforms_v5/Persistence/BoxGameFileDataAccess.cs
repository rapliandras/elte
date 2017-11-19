using System;
using System.IO;
using System.Threading.Tasks;

namespace BoxGame.Persistence
{
    /// <summary>
    /// Sudoku fájlkezelő típusa.
    /// </summary>
    public class BoxGameFileDataAccess : IBoxGameDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        public async Task<GameBoard> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla méretét
                    Int32 regionSize = Int32.Parse(numbers[1]); // beolvassuk a házak méretét
                    GameBoard table = new GameBoard(tableSize, regionSize); // létrehozzuk a táblát

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');
                    }

                    return table;
                }
            }
            catch
            {
                throw new BoxGameDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, GameBoard table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.m_Rows); // kiírjuk a méreteket
                    await writer.WriteLineAsync(" " + (char) table.m_Columns); // kiírjuk a méreteket

                    //await writer.WriteLineAsync(" " + table.RegionSize);
                    for (Int32 i = 0; i < table.m_Columns; i++)
                    {
                        for (Int32 j = 0; j < table.m_Columns; j++)
                        {
                            //await writer.WriteAsync(table[i, j] + " "); // kiírjuk az értékeket
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new BoxGameDataException();
            }
        }
    }


}