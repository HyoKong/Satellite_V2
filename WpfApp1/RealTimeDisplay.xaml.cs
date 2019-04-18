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
using System.IO;
using TEST;

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
                dllPlotter.Viewport.Visible = new System.Windows.Rect(xaxis, 0, group, yaxis);//主要注意这里一行  
                dyPlotter.Viewport.Visible = new System.Windows.Rect(xaxis, 0, group, yaxis);
                fsjPlotter.Viewport.Visible = new System.Windows.Rect(xaxis, 0, group, yaxis);
                tcyPlotter.Viewport.Visible = new System.Windows.Rect(xaxis, 0, group, yaxis);

            }

            //dataSource = new ObservableDataSource<Point>();
            cpuUsageText.Text = String.Format("{0:0}%", y);
            currentSecond++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dllPlotter.AddLineGraph(dataSource, Colors.Green, 2, "Percentage");
            dyPlotter.AddLineGraph(dataSource, Colors.Green, 2, "percentage");
            fsjPlotter.AddLineGraph(dataSource, Colors.Green, 2, "percentage");
            tcyPlotter.AddLineGraph(dataSource, Colors.Green, 2, "percentage");
            //plotter.AddLineChart(dataSource);
            dllPlotter.LegendVisible = true;
            dyPlotter.LegendVisible = true;
            fsjPlotter.LegendVisible = true;
            tcyPlotter.LegendVisible = true;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.01);
            dispatcherTimer.Tick += AnimatedPlot;
            dispatcherTimer.IsEnabled = true;
            dllPlotter.Viewport.FitToView();
            dyPlotter.Viewport.FitToView();

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

    public partial class ReadFile
    {
        // txt file name list
        public string[] dllFile = getFileName("./Dataset/DLL");
        public string[] dyFile = getFileName("./Dataset/DY");
        public string[] fsjFile = getFileName("./Dataset/FSJ");
        public string[] tcyFile = getFileName("./Dataset/TCY");

        public int read_first_num = 100;

        public ReadFile(string part)
        {
            if (part == "DLL")
            {
                ReadDLL(out string time, out double pro);
            }
            else if (part == "DY")
            {
                ReadDY();
            }
            else if (part=="FSJ")
            {
                ReadFSJ();
            }
            else
            {
                ReadTCY();
            }
        }
 
        public void ReadDLL(out string time,out double pro)
        {
            // for each txt file of DLL
            // Unreachable code detected
            for (int t = 0; t < dllFile.Length; t++)
            // Unreachable code detected
            {
                using (StreamReader dllStreamReader = new StreamReader("./Dataset/DLL/" + dllFile[t]))
                {
                   

                    string dllLine = dllStreamReader.ReadLine();
                    string[] line_sp = dllLine.Split('\t');
                    double[] value_nums = new double[10];
                    value_nums[0] = FindFaultData(2.0, -2.0, double.Parse(line_sp[1]));
                    value_nums[1] = FindFaultData(2.0, -2.0, double.Parse(line_sp[2]));
                    value_nums[2] = FindFaultData(2.0, -2.0, double.Parse(line_sp[4]));
                    value_nums[3] = FindFaultData(2.0, -2.0, double.Parse(line_sp[5]));
                    value_nums[4] = FindFaultData(45.0, -5.0, double.Parse(line_sp[7]));
                    value_nums[5] = FindFaultData(45.0, -5.0, double.Parse(line_sp[8]));
                    value_nums[6] = FindFaultData(45.0, -5.0, double.Parse(line_sp[9]));
                    value_nums[7] = FindFaultData(45.0, -5.0, double.Parse(line_sp[10]));
                    value_nums[8] = FindFaultData(45.0, -5.0, double.Parse(line_sp[11]));
                    value_nums[9] = FindFaultData(45.0, -5.0, double.Parse(line_sp[12]));

                    pro = TEST.Class1.test_dll(value_nums);

                    time = line_sp[0];
                    
                }
            } 
        }

        public void ReadDY()
        {

        }
        public void ReadFSJ()
        {

        }
        
        public void ReadTCY()
        {

        }

        // Eliminate fault data caused by transcoding or other reasons
        private double FindFaultData(double high, double low, double data)
        {
            if (data >= high * 1000)
            {
                return (low + high) / 2;
            }
            return data;

        }
        #region 读取文件夹下所有txt文件(按文件创建时间排序)
        //按照文件创建时间排序
        public static void SortAsFileCreationTime(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate (FileInfo x, FileInfo y)
            {
                return x.LastWriteTime.CompareTo(y.LastWriteTime);
            }
            );
        }

        //读取指定路径下所有文件名d
        public static string[] getFileName(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] fsinfos = root.GetFiles("*.*");
            //string[] txt_name_list = new string[]{};
            List<string> strList = new List<string>();
            SortAsFileCreationTime(ref fsinfos);
            foreach (FileSystemInfo fsinfo in fsinfos)
            {

                if (fsinfo is DirectoryInfo)
                {
                    getFileName(fsinfo.Name);
                }
                else
                {
                    //Console.WriteLine(fsinfo.Name);
                    strList.Add(fsinfo.Name);
                }
            }
            string[] strArray = strList.ToArray();
            return strArray;
        }
        #endregion
    }
}