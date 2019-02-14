using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PathFindingALgorithmsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum Algorithms { ASTAR, DIJKSTRA, BFS, DFS }    
    public partial class MainWindow : Window
    {
        public delegate void ChoiceSelectedEventHandler(object sender, NewSimPropertiesEventArgs args);
        public event ChoiceSelectedEventHandler OnChoiceSelected;

        string size, speed;       
        Algorithms selected;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SimulationSpeedInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= SimulationSpeedInput_GotFocus;
        }

        private void GridSizeInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= SimulationSpeedInput_GotFocus;           
        }

        private void AStar_Click(object sender, RoutedEventArgs e)
        {
            size = GridSizeInput.Text;
            speed = SimulationSpeedInput.Text;
            selected = Algorithms.ASTAR;
            ChoiceSelected();
            Close();
        }

        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            size = GridSizeInput.Text;
            speed = SimulationSpeedInput.Text;
            selected = Algorithms.DIJKSTRA;
            ChoiceSelected();
            Close();
        }

        private void BFS_Click(object sender, RoutedEventArgs e)
        {
            size = GridSizeInput.Text;
            speed = SimulationSpeedInput.Text;
            selected = Algorithms.BFS;
            ChoiceSelected();
            Close();
        }

        private void DFS_Click(object sender, RoutedEventArgs e)
        {
            size = GridSizeInput.Text;
            speed = SimulationSpeedInput.Text;
            selected = Algorithms.DFS;
            ChoiceSelected();
            Close();
        }

        private void ChoiceSelected()
        {
            if (OnChoiceSelected != null)
                OnChoiceSelected(this, new NewSimPropertiesEventArgs(size, speed, selected));
        }
    }
}
