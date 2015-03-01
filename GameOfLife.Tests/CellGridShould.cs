using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    internal class CellGridShould
    {
        private const int GridSize = 10;

        private CellGrid cellGrid;
        private Cell[,] cells;

        [SetUp]
        public void BeforeEachTest()
        {
            cells = new Cell[GridSize, GridSize];

            cellGrid = new CellGrid(cells);
        }

        [Test]
        public void HaveA2DArrayOfCells()
        {
            CollectionAssert.AreEqual(cells, cellGrid.Cells);
        }

        [Test]
        public void HaveASize()
        {
            Assert.AreEqual(cellGrid.Size, GridSize);
        }

        [Test]
        public void KillCellIfItIsUnderPopulated()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Dead);
        }

        [Test]
        public void KeepCellAliveIfItHas2Neighbours()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Alive);
            cells[5, 6] = new Cell(CellState.Alive);
            cells[5, 4] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Alive);
        }

        [Test]
        public void KeepCellAliveIfItHas3Neighbours()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Alive);
            cells[5, 6] = new Cell(CellState.Alive);
            cells[5, 4] = new Cell(CellState.Alive);
            cells[6, 6] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Alive);
        }

        [Test]
        public void KillCellIfItHas4Neighbours()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Alive);
            cells[5, 6] = new Cell(CellState.Alive);
            cells[5, 4] = new Cell(CellState.Alive);
            cells[6, 6] = new Cell(CellState.Alive);
            cells[4, 4] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Dead);
        }

        [Test]
        public void ReviveCellIfItHas3Neighbours()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Dead);
            cells[5, 6] = new Cell(CellState.Alive);
            cells[6, 6] = new Cell(CellState.Alive);
            cells[4, 4] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Alive);
        }

        [Test]
        public void DoNotReviveCellIfItHas2Neighbours()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Dead);
            cells[6, 6] = new Cell(CellState.Alive);
            cells[4, 4] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Dead);
        }

        [Test]
        public void DoNotReviveCellIfItHas1Neighbour()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }

            cells[5, 5] = new Cell(CellState.Dead);
            cells[4, 4] = new Cell(CellState.Alive);

            CellGrid cellGrid = new CellGrid(cells);

            cellGrid.Iterate();

            Assert.IsTrue(cellGrid.Cells[5, 5].State == CellState.Dead);
        }
    }
}