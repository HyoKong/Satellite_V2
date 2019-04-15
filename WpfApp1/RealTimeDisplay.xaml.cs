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
using System.Collections;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for RealTimeDisplay.xaml
    /// </summary>
    public partial class RealTimeDisplay : Window
    {
        private ObservableDataSource<Point> dataSource = new ObservableDataSource<Point>();
        //private PerformanceCounter cpuPerformance = new PerformanceCounter();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private PerformanceCounter performanceCounter = new PerformanceCounter();
        private int currentSecond = 0;
        private int i = 0;
        Queue q = new Queue();

        int xaxis = 0;
        int yaxis = 0;
        int group = 20;//默认组距  
        bool buttonbool = false;//标志是否滚屏  

        public RealTimeDisplay()
        {
            InitializeComponent();
        }

        private void AnimatedPlot(object sender, EventArgs e)
        {
            performanceCounter.CategoryName = "Processor";
            performanceCounter.CounterName = "% Processor Time";
            performanceCounter  .InstanceName = "_Total";

            double x = currentSecond;
            double y = performanceCounter.NextValue();

            Point point = new Point(x, y);
            dataSource.AppendAsync(base.Dispatcher, point);
            


            if (true)
            {
                if (q.Count < group)
                {
                    q.Enqueue((int)y);//入队  
                    yaxis = 0;
                    foreach (int c in q)
                        if (c > yaxis)
                            yaxis = c;
                    
                }
                else
                {
                    q.Dequeue();//出队  
                    q.Enqueue((int)y);//入队  
                    yaxis = 0;
                    foreach (int c in q)
                        if (c > yaxis)
                            yaxis = c;
                    dataSource.Collection.RemoveAt(0);
                    Console.WriteLine(dataSource.Collection.Count);
                }

                if (currentSecond - group > 0)
                    xaxis = currentSecond - group;
                else
                    xaxis = 0;
                //Console.WriteLine(xaxis.ToString());

                //Debug.Write("{0}\n", yaxis.ToString());
                plotter.Viewport.Visible = new System.Windows.Rect(xaxis, 10, group, yaxis);//主要注意这里一行  
            }

            //dataSource = new ObservableDataSource<Point>();
            cpuUsageText.Text = String.Format("{0:0}%", y);
            currentSecond++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            plotter.AddLineGraph(dataSource, Colors.Green, 2, "Percentage");
            //plotter.AddLineChart(dataSource);
            plotter.LegendVisible = true;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.01);
            dispatcherTimer.Tick += AnimatedPlot;
            dispatcherTimer.IsEnabled = true;
            plotter.Viewport.FitToView();
        }

        private void IfDynamic_Click(object sender, RoutedEventArgs e)
        {
            if (buttonbool)
            {
                buttonbool = false;
            }
            else
            {
                buttonbool = true;
            }
        }
    }
}