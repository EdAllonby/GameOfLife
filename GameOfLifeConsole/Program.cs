using System;
using GameOfLife;

namespace GameOfLifeConsole
{
    class Program
    {
        private const int GridSize = 5;

        static void Main(string[] args)
        {
            Game game = new Game(new CellGrid(InitialiseCells()));

            for (int i = 0; i < 15; i++)
            {
                game.ChangeCellState(CellState.Alive, i, i);
            }

            while (true)
            {
                game.ProcessTurnAsync();
                Console.WriteLine(game.CurrentGeneration);
            }
        }

        private static Cell[,] InitialiseCells()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            return cells;
        }
    }
}
