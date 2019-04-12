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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for RealTimeDisplay.xaml
    /// </summary>
    public partial class RealTimeDisplay : Window
    {
        private ObservableDataSource<Point> dataSource = new ObservableDataSource<Point>();
        private PerformanceCounter cpuPerformance = new PerformanceCounter();
        private DispatcherTimer timer = new DispatcherTimer();
        private int i = 0;


        public RealTimeDisplay()
        {
            InitializeComponent();
        }

        private void AnimatedPlot(object sender, EventArgs e)
        {
            cpuPerformance.CategoryName = "Processor";
            cpuPerformance.CounterName = "% Processor Time";
            cpuPerformance.InstanceName = "_Total";

            double x = i;
            double y = cpuPerformance.NextValue();

            Point point = new Point(x, y);
            dataSource.AppendAsync(base.Dispatcher, point);

            cpuUsageText.Text = String.Format("{0:0}%", y);
            i++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            plotter.AddLineGraph(dataSource, Colors.Green, 2, "Percentage");
            //plotter.AddLineChart(dataSource);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(AnimatedPlot);
            timer.IsEnabled = true;
            plotter.Viewport.FitToView();
        }
    }
}