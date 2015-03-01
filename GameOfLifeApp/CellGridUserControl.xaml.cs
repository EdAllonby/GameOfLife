using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using GameOfLife;

namespace GameOfLifeApp
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CellGridUserControl
    {
        private const int GridSize = 20;
        private readonly Game game;
        private DispatcherTimer simulationSpeed;
        private int simulationSpeedInMilliseconds;

        public CellGridUserControl()
        {
            game = new Game(new CellGrid(InitialiseCells()));

            InitializeComponent();
         
            SetupGrid();
        }

        public int SimulationSpeedInMilliseconds
        {
            get { return simulationSpeedInMilliseconds; }
            set
            {
                if (value > 0)
                {
                    simulationSpeedInMilliseconds = value;

                    if (simulationSpeed != null)
                    {
                        simulationSpeed.Interval = new TimeSpan(0, 0, 0, 0, value);
                    }
                }
            }
        }

        public void StartSimulation()
        {
            simulationSpeed = new DispatcherTimer();
            simulationSpeed.Tick += dispatcherTimer_Tick;
            simulationSpeed.Interval = new TimeSpan(0, 0, 0, 0, SimulationSpeedInMilliseconds);
            simulationSpeed.Start();
        }

        private async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            await game.ProcessTurn();
            UpdateToggles();
        }

        private void UpdateToggles()
        {
            for (int column = 0; column < game.GridSize; column++)
            {
                for (int row = 0; row < game.GridSize; row++)
                {
                    ToggleButton toggle = GetToggleFromCellPosition(column, row);

                    CellState cellState = game.GetCell(column, row).State;

                    toggle.IsChecked = cellState == CellState.Alive;
                }
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

        private void SetupGrid()
        {
            for (int stackPanelRow = 0; stackPanelRow < GridSize; stackPanelRow++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = new GridLength()
                };
                
                CellGrid.RowDefinitions.Add(rowDefinition);

                StackPanel stackPanel = new StackPanel {Orientation = Orientation.Horizontal};

                for (int buttonColumn = 0; buttonColumn < GridSize; buttonColumn++)
                {
                    ToggleButton button = CreateButton(buttonColumn, stackPanelRow);

                    stackPanel.Children.Add(button);
                }

                stackPanel.SetValue(Grid.RowProperty, stackPanelRow);

                CellGrid.Children.Add(stackPanel);
            }
        }

        private ToggleButton CreateButton(int buttonColumn, int stackPanelRow)
        {
            ToggleButton button = new ToggleButton
            {
                Width = 20,
                Height = 20,
                Background = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.Tomato)
            };

            string buttonName = string.Format("Button{0}r{1}", buttonColumn, stackPanelRow);

            button.Name = buttonName;

            button.Click += OnCellClicked;
            return button;
        }

        private void OnCellClicked(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = e.Source as ToggleButton;

            if (clickedButton != null)
            {
                Cell pressedCell = GetCellFromButtonName(clickedButton.Name);

                if (clickedButton.IsChecked != null && clickedButton.IsChecked.Value)
                {
                    pressedCell.State = CellState.Alive;
                }
                else
                {
                    pressedCell.State = CellState.Dead;
                }
            }
        }

        private ToggleButton GetToggleFromCellPosition(int column, int row)
        {
            foreach (object gridChild in CellGrid.Children)
            {
                StackPanel stackPanel = gridChild as StackPanel;

                if (stackPanel != null)
                {
                    foreach (object stackPanelChild in stackPanel.Children)
                    {
                        ToggleButton toggle = stackPanelChild as ToggleButton;

                        if (toggle != null)
                        {
                            string toggleNameToFind = "Button" + column + "r" + row;

                            if (toggle.Name == toggleNameToFind)
                            {
                                return toggle;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private Cell GetCellFromButtonName(string buttonName)
        {
            string position = buttonName.Remove(0, "button".Count());

            string[] components = position.Split('r');

            int column = int.Parse(components[0]);
            int row = int.Parse(components[1]);

            return game.GetCell(column, row);
        }
    }
}