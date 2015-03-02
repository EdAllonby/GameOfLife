using System;
using System.Windows;

namespace GameOfLifeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initialises the Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CellGrid.NewGeneration += CellGrid_NewGeneration;
        }

        void CellGrid_NewGeneration(object sender, int e)
        {
            GenerationLabel.Content = "Generation: " + e;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CellGrid.StartSimulation();
        }

        private void OnSimulationSpeedChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CellGrid != null)
            {
                CellGrid.SimulationSpeedInMilliseconds = (int) e.NewValue;
            }
        }
    }
}