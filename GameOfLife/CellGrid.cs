using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class CellGrid
    {
        public CellGrid(Cell[,] cells)
        {
            Cells = cells;
        }

        public Cell[,] Cells { get; private set; }

        public int Size
        {
            get { return Cells.GetLength(0); }
        }

        public void Iterate()
        {
            Parallel.For(0, Size, column =>
            {
                Parallel.For(0, Size, row =>
                {
                    UpdateCellNeighbourCount(column, row);
                });
            });

            Parallel.For(0, Size, column =>
            {
                Parallel.For(0, Size, row =>
                {
                    UpdateCellState(column, row);
                });
            });
        }

        private void UpdateCellNeighbourCount(int column, int row)
        {
            int livingNeighbours = SurroundingCellStates(column, row).Count(cellState => cellState == CellState.Alive);
            Cells[column, row].Neighbours = livingNeighbours;
        }

        private void UpdateCellState(int column, int row)
        {
            if (IsCellAlive(column, row) && IsCellUnderPopulated(column, row))
            {
                Cells[column, row].PreviousState = Cells[column, row].State;

                Cells[column, row].State = CellState.Dead;
            }
            if (IsCellAlive(column, row) && !IsCellHealthy(column, row))
            {
                Cells[column, row].PreviousState = Cells[column, row].State;

                Cells[column, row].State = CellState.Dead;
            }
            if (IsCellAlive(column, row) && IsCellOverPopulated(column, row))
            {
                Cells[column, row].PreviousState = Cells[column, row].State;

                Cells[column, row].State = CellState.Dead;
            }
            if (!IsCellAlive(column, row) && IsCellAllowedToRelive(column, row))
            {
                Cells[column, row].PreviousState = Cells[column, row].State;

                Cells[column, row].State = CellState.Alive;
            }
        }

        private bool IsCellAlive(int column, int row)
        {
            return Cells[column, row].State == CellState.Alive;
        }

        private bool IsCellUnderPopulated(int column, int row)
        {
            return Cells[column, row].Neighbours < 2;
        }

        private bool IsCellHealthy(int column, int row)
        {
            return Cells[column, row].Neighbours == 2 || Cells[column, row].Neighbours == 3;
        }

        private bool IsCellOverPopulated(int column, int row)
        {
            return Cells[column, row].Neighbours > 3;
        }

        private bool IsCellAllowedToRelive(int column, int row)
        {
            return Cells[column, row].Neighbours == 3;
        }

        private IEnumerable<CellState> SurroundingCellStates(int column, int row)
        {
            return new List<CellState>
            {
                GetTopLeftCellState(column, row),
                GetTopRightCellState(column, row),
                GetBottomLeftCellState(column, row),
                GetBottomRightCellState(column, row),
                GetTopCellState(column, row),
                GetBottomCellState(column, row),
                GetLeftCellState(column, row),
                GetRightCellState(column, row)
            };
        }

        private CellState GetTopLeftCellState(int column, int row)
        {
            if (HasCellAbove(row) && HasCellToLeft(column))
            {
                return Cells[column - 1, row - 1].State;
            }

            return CellState.Dead;
        }

        private CellState GetTopRightCellState(int column, int row)
        {
            if (HasCellAbove(row) && HasCellToRight(column))
            {
                return Cells[column + 1, row - 1].State;
            }

            return CellState.Dead;

        }

        private CellState GetBottomLeftCellState(int column, int row)
        {
            if (HasCellBelow(row) && HasCellToLeft(column))
            {
                return Cells[column - 1, row + 1].State;
            }

            return CellState.Dead;
        }

        private CellState GetBottomRightCellState(int column, int row)
        {
            if (HasCellBelow(row) && HasCellToRight(column))
            {
                return Cells[column + 1, row + 1].State;
            }

            return CellState.Dead;
        }

        private CellState GetTopCellState(int column, int row)
        {
            if (HasCellAbove(row))
            {
                return Cells[column, row - 1].State;
            }

            return CellState.Dead;
        }

        private CellState GetBottomCellState(int column, int row)
        {
            if (HasCellBelow(row))
            {
                return Cells[column, row + 1].State;
            }

            return CellState.Dead;
        }

        private CellState GetLeftCellState(int column, int row)
        {
            if (HasCellToLeft(column))
            {
                return Cells[column - 1, row].State;
            }

            return CellState.Dead;
        }

        private CellState GetRightCellState(int column, int row)
        {
            if (HasCellToRight(column))
            {
                return Cells[column + 1, row].State;
            }

            return CellState.Dead;
        }

        private static bool HasCellAbove(int row)
        {
            if (row - 1 < 0)
            {
                return false;
            }

            return true;
        }

        private bool HasCellBelow(int row)
        {
            if (row + 1 > Size - 1)
            {
                return false;
            }

            return true;
        }

        private static bool HasCellToLeft(int column)
        {
            if (column - 1 < 0)
            {
                return false;
            }

            return true;
        }

        private bool HasCellToRight(int column)
        {
            if (column + 1 > Size - 1)
            {
                return false;
            }

            return true;
        }
    }
}