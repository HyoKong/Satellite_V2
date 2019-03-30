﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.Data;
//using Microsoft.Research.DynamicDataDisplay;
using InteractiveDataDisplay.WPF;




namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for PredictVariableWindow.xaml
    /// </summary>
    public partial class PredictVariableWindow : Window
    {
        public string[] items { get; set; }

        //存储读取txt文件的变量
        //public string[] dll_file = getFileName("./读取多个文件实验/动量轮");
        //public string[] fsj_file = getFileName("./读取多个文件实验/辐射计");
        //public string[] tcy_file = getFileName("./读取多个文件实验/探测仪");
        //public string[] dy_file = getFileName("./读取多个文件实验/电源");

        private int read_first_num = 100;//读取txt文件最先开始读的行数
        public PredictVariableWindow()
        {
            InitializeComponent();
            //this.items = info;
            //DataLoaded();

            double[] x = new double[20000];
            for (int i = 0; i < x.Length; i++)
                //x[i] = 3.1415 * i / (x.Length - 1);
                x[i] = i/100.0*Math.PI;
            //x[i] = i;

            //for (int i = 0; i < 25; i++)
            //{
            //    var lg = new LineGraph();
            //    lines.Children.Add(lg);
            //    lg.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(i * 10), 0));
            //    lg.Description = String.Format("Data series {0}", i + 1);
            //    lg.StrokeThickness = 2;
            //    lg.Plot(x, x.Select(v => Math.Sin(v + i / 10.0)).ToArray());
            //    //lg.Plot(x, x);
            //}
            var lg = new LineGraph();
            lines.Children.Add(lg);
            lg.Stroke = new SolidColorBrush(Color.FromRgb(255,0,0));
            lg.PlotY(x.Select(v => Math.Sin(v)));
        }


        private void DataLoaded()
        {
            FileStream fs = new FileStream(@"D:\Satellite\Satellite_V2\Satellite_V2\data.txt", FileMode.Open, FileAccess.Read);
            StreamReader m_streamReader = new StreamReader(fs);

            //使用StreamReader类来读取文件
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //从数据流中读取每一行，直到文件的最后一行，并在textBox中显示出内容，其中textBox为文本框，如果不用可以改为别的
            //this.tbTextBox.Text = "";
            string strLine = m_streamReader.ReadLine();
            ArrayList alDataList = new ArrayList();

            // 新建DataTable列表
            DataTable dt = new DataTable("Variable");
            foreach (string item in this.items)
            {
                dt.Columns.Add(item);
            }

            while (strLine != null)
            {
                //this.tbTextBox.Text += strLine + "\n";
                string[] dataList = strLine.Split('\t');

                //float floatLine = Convert.ToSingle(strLine);
                //foreach(string data in dataList)
                //{
                //    alDataList.Add(Convert.ToSingle(data));
                //}
                //alDataList.Add(dataList);

                DataRow dr = dt.NewRow();
                for (int i = 0; i < dataList.Length; i++)
                {
                    dr[i.ToString()] = dataList[i];
                }
                dt.Rows.Add(dr);
                strLine = m_streamReader.ReadLine();
            }
            //关闭此StreamReader对象
            m_streamReader.Close();

            int count = alDataList.Count;
            //var column = alDataList[0].Length;
            //for (int i = 0; i < count; i++)
            double[] x = new double[count];
            for (int i = 0; i < count; i++)
            {
                x[i] = i;
            }

            foreach (string[] array in alDataList)
            {
                var lg = new LineGraph();
                lines.Children.Add(lg);
                //lg.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, (byte)(i * 10), 0));
                //lg.Description = String.Format("Data series {0}", i + 1);
                //lg.StrokeThickness = 2;
                //lg.Plot(x, array[i]);
            }
        }

        //读取指定路径下所有文件名
        public static string[] getFileName(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            FileSystemInfo[] fsinfos = root.GetFileSystemInfos();
            //string[] txt_name_list = new string[]{};
            List<string> strList = new List<string>();
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

    }

    public class VisibilityToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility)value) == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
    }

}
