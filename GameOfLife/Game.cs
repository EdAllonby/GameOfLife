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

        public Task ProcessTurn()
        {
            CurrentGeneration++;

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