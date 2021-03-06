using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Game
    {
        private readonly CellGrid cellGrid;

        public Game(CellGrid grid)
        {
            cellGrid = grid;
        }

        public int CurrentGeneration { get; private set; }

        public int GridSize
        {
            get { return cellGrid.Size; }
        }

        public Task ProcessTurnAsync()
        {
            CurrentGeneration++;

            Parallel.ForEach(cellGrid.Cells.Cast<Cell>() , cell =>
            {
                cell.PreviousState = cell.State;
            });

            return Task.Factory.StartNew(() => cellGrid.Iterate());
        }

        public Cell GetCell(int column, int row)
        {
            return cellGrid.Cells[column, row];
        }

        public void ChangeCellState(CellState newCellState, int column, int row)
        {
            cellGrid.Cells[column, row].State = newCellState;
        }
    }
}