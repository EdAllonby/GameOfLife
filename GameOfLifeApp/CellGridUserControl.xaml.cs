﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
        private const int CellSize = 15;
        public readonly DependencyProperty GridSizeProperty;
        private Game game;
        private readonly DispatcherTimer simulationSpeed;
        private int simulationSpeedInMilliseconds;
        private ToggleButton[,] toggleButtons;

        public CellGridUserControl()
        {
            InitializeComponent();

            simulationSpeed = new DispatcherTimer();
            simulationSpeed.Tick += dispatcherTimer_Tick;
            simulationSpeed.Interval = new TimeSpan(0, 0, 0, 0, SimulationSpeedInMilliseconds);

            SimulationSpeedInMilliseconds = 200;

            GridSizeProperty = DependencyProperty.Register("GridSize", typeof (int), typeof (CellGridUserControl), new PropertyMetadata((OnSomeValuePropertyChanged)));
        }

        public event EventHandler<int> NewGeneration; 

        public int GridSize
        {
            get
            {
                int size = (int) GetValue(GridSizeProperty);
                return size;
            }
            set
            {
                StopSimulation();
                SetValue(GridSizeProperty, value);
                SetupNewGridSize();
            }
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

        private void OnSomeValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridSize = (int) e.NewValue;
        }

        private void SetupNewGridSize()
        {
            toggleButtons = new ToggleButton[GridSize, GridSize];
            game = new Game(new CellGrid(InitialiseCells()));
            InitializeComponent();

            SetupGrid();
        }

        public void StartSimulation()
        {
            simulationSpeed.Start();
        }

        private void StopSimulation()
        {
            simulationSpeed.Stop();
        }

        private async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            await game.ProcessTurnAsync();
            OnNewGeneration(game.CurrentGeneration);
            UpdateToggles();
        }

        private void UpdateToggles()
        {
            ThreadStart job = () =>
            {
                Parallel.For(0, game.GridSize, column =>
                {
                    Parallel.For(0, game.GridSize, row =>
                    {
                        Cell cell = game.GetCell(column, row);

                        if (cell.PreviousState != cell.State)
                        {
                            ToggleButton toggle = toggleButtons[column, row];

                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { toggle.IsChecked = cell.State == CellState.Alive; }));
                        }
                    });
                });
            };

            Thread thread = new Thread(job);
            thread.Start();
        }

        private Cell[,] InitialiseCells()
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
            CellGrid.Children.Clear();

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

                    toggleButtons[buttonColumn, stackPanelRow] = button;

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
                Width = CellSize,
                Height = CellSize,
                Background = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.Tomato)
            };

            string buttonName = string.Format("Button{0}r{1}", buttonColumn, stackPanelRow);

            button.Name = buttonName;

            button.Click += OnCellClicked;
            button.MouseEnter += button_MouseLeftButtonDown;
            return button;
        }

        private void button_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                ToggleButton clickedButton = e.Source as ToggleButton;

                if (clickedButton != null)
                {
                    if (clickedButton.IsChecked != null && clickedButton.IsChecked.Value.Equals(true))
                    {
                        clickedButton.IsChecked = false;
                    }
                    else
                    {
                        clickedButton.IsChecked = true;
                    }

                    CellClicked(clickedButton);
                }
            }
        }

        private void OnCellClicked(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = e.Source as ToggleButton;

            CellClicked(clickedButton);
        }

        private void CellClicked(ToggleButton clickedButton)
        {
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

        private Cell GetCellFromButtonName(string buttonName)
        {
            string position = buttonName.Remove(0, "button".Count());

            string[] components = position.Split('r');

            int column = int.Parse(components[0]);
            int row = int.Parse(components[1]);

            return game.GetCell(column, row);
        }

        protected virtual void OnNewGeneration(int e)
        {
            var handler = NewGeneration;
            if (handler != null) handler(this, e);
        }
    }
}