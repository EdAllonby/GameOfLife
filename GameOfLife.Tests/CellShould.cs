using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class CellShould
    {
        [Test]
        public void HaveState()
        {
            Cell cell = new Cell(CellState.Dead);

            CellState state = cell.State;

            Assert.IsNotNull(state);
        }

        [Test]
        public void HaveAssignableNeighbourCount()
        {
            const int Neighbours = 2;
            Cell cell = new Cell(CellState.Alive) {Neighbours = Neighbours};

            Assert.AreEqual(cell.Neighbours, Neighbours);
        }

        [Test]
        public void InitialiseWithAState()
        {
            const CellState InitialState = CellState.Alive;

            Cell cell = new Cell(InitialState);

            Assert.AreEqual(InitialState, cell.State);
        }

        [Test]
        public void BeAbleToChangeStateToAlive()
        {
            Cell cell = new Cell(CellState.Alive);

            const CellState NewState = CellState.Alive;

            cell.State = NewState;

            Assert.AreEqual(cell.State, NewState);
        }

        [Test]
        public void BeAbleToChangeStateToDead()
        {
            Cell cell = new Cell(CellState.Alive);

            const CellState NewState = CellState.Dead;

            cell.State = NewState;

            Assert.AreEqual(cell.State, NewState);
        }
    }
}