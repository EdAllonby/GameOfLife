namespace GameOfLife
{
    public class Cell
    {
        private int neighbours;

        public Cell(CellState cellState)
        {
            State = cellState;
        }

        public CellState State { get; set; }

        public int Neighbours
        {
            get { return neighbours; }
            set
            {
                if (value >= 0)
                {
                    neighbours = value;
                }
            }
        }
    }
}