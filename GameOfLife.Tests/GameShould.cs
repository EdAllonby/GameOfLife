using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GameShould
    {
        private Game game;
        private CellGrid gameCellGrid;
        private const int GridSize = 3;
        [SetUp]
        public void BeforeEachTest()
        {
            Cell[,] cells = new Cell[GridSize, GridSize];

            for (int column = 0; column < GridSize; column++)
            {
                for (int row = 0; row < GridSize; row++)
                {
                    cells[column, row] = new Cell(CellState.Dead);
                }
            }
            gameCellGrid = new CellGrid(cells);


            game = new Game(gameCellGrid);
        }

        [Test]
        public void StartAtTurnZeroForNewGame()
        {
            Assert.IsTrue(game.CurrentGeneration.Equals(0));
        }

        [Test]
        public void BeAbleToGoToNewTurn()
        {
            int lastTurn = game.CurrentGeneration;

            game.ProcessTurn();

            int newTurn = game.CurrentGeneration;

            Assert.AreEqual(++lastTurn, newTurn);
        }
    }
}