using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
//using Xceed.Wpf.Toolkit;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for VariableTimeSelectWindow.xaml
    /// </summary>
    public partial class VariableTimeSelectWindow : Window
    {
        // 选择窗口类的属性
        public string startDate { get; set; }
        public string stopDate { get; set; }
        public string startHour { get; set; }
        public string startMinute { get; set; }
        public string stopHour { get; set; }
        public string stopMinute { get; set; }
        public string part { get; set; }
        public string variable { get; set; }
        public bool startTimeFlag { get; set; } = false;
        public bool stopTimeFlag { get; set; } = false;
        //private string[] itemtemp = 
        public string[] items { get; set; } = new string[0];

        


        private Dictionary<string, string[]> partAndVariationDictionary = new Dictionary<string, string[]>()
        {
            { "动量轮", new string[] { "动量轮1马达电流", "动量轮2马达电流", "动量轮4马达电流", "动量轮5马达电流", "动量轮1轴温","动量轮2轴温",
                                        "动量轮3轴温","动量轮4轴温","动量轮5轴温","动量轮6轴温",} },
            { "电源", new string[] { "1", "2", "3", "4" } },
            { "探测仪", new string[] { "1", "2", "3" } },
        };

        public VariableTimeSelectWindow()
        {
            InitializeComponent();
            InitPartComboBox();
        }

        private void InitPartComboBox()
        {
            // 初始化卫星部件列表
            ItemCollection coll = cbPart.Items;
            foreach (KeyValuePair<string, string[]> kvp in partAndVariationDictionary)
            {
                ComboBoxItem boxItem = new ComboBoxItem() { Content = kvp.Key };
                coll.Add(boxItem);
            }

            // 初始化时分列表
            //ArrayList hourArrayList = new ArrayList();
            //ArrayList minuteArrayList = new ArrayList();
            //ItemCollection startHourItemCollection = cbStartHour.Items;
            //ItemCollection stopHourItemCollection = cbStopHour.Items;
            //ItemCollection startMinuteItemCollection = cbStartMinute.Items;
            //ItemCollection stopMinuteItemCollection = cbStopMinute.Items;

            //for (int i = 0; i < 24; i++)
            //{
            //    hourArrayList.Add(i);
            //}
            //for(int i =0;i<60;i++)
            //{
            //    minuteArrayList.Add(i);
            //}

            //foreach(int hour in hourArrayList)
            //{
            //    ComboBoxItem startHourItem = new ComboBoxItem() { Content = hour };
            //    startHourItemCollection.Add(startHourItem);

            //    ComboBoxItem stopHourItem = new ComboBoxItem() { Content = hour };
            //    stopHourItemCollection.Add(stopHourItem);
            //}
            //foreach(int minute in minuteArrayList)
            //{
            //    ComboBoxItem startMinuteItem = new ComboBoxItem() { Content = minute };
            //    startMinuteItemCollection.Add(startMinuteItem);
            //    ComboBoxItem stopMinuteItem = new ComboBoxItem() { Content = minute };
            //    stopMinuteItemCollection.Add(stopMinuteItem);

            //}
            // 给ComboBox注册一个选项改变的事件
            cbPart.SelectionChanged += new SelectionChangedEventHandler(cbPart_SelectionChanged);
        }

        private void cbPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 当前部件和变量
            ItemCollection coll = cbVariable.Items;
            // 先清空
            coll.Clear();
            // 再添加
            foreach (KeyValuePair<string, string[]> kvp in partAndVariationDictionary)
            {
                // kvp.Value = { "轴温1", "轴温2", }
                // 此时的 cityComboxBox.SelectedValue = System.Windows.Controls.ComboBoxItem: 动量轮
                // 所以如果用这种方法获取选中的值，还需要切割字符串
                ComboBoxItem selectedCity = cbPart.SelectedItem as ComboBoxItem;
                string cityName = selectedCity.Content.ToString();

                if (cityName.Equals(kvp.Key))
                {
                    List<string> listItem = this.items.ToList();
                    foreach (var item in kvp.Value)
                    {
                        // item = "轴温1"
                        ComboBoxItem boxItem = new ComboBoxItem() { Content = item };
                        coll.Add(boxItem);                   
                        listItem.Add(item);
                    }
                    this.items = listItem.ToArray();
                }
            }
        }

        // 确认按钮判断，进入变量显示界面
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {

            if ( this.part != null && this.variable != null && this.startTimeFlag)
            {
                //MessageBox.Show(this.startDate+this.startHour+this.variable);

                PredictVariableWindow predictVariableWindow = new PredictVariableWindow();
                bool? b = predictVariableWindow.ShowDialog();
                //if (b == true)
                //{
                    
                //}
                //else
                //{
                //    predictVariableWindow.tbTextBlock.Text = "无输入！";
                //}
                //predictVariableWindow.tbTextBlock.Text = this.startDate + this.variable;

            }
            else
            {
                MessageBox.Show("请选择时间与部件变量！");
            }
        }

        // 取消按钮判断
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        // 获取选择的时间
        private void CbSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool startHourFlag = true;
            bool startMinuteFlag = true;
            bool stopHourFlag = true;
            bool stopMinuteFlag = true;
            bool partFlag = true;
            bool variableFlag = true;
            bool startDateFlag = true;
            bool stopDateFlag = true;

            string startHour = null;
            string startMinute = null;
            string stopHour = null;
            string stopMinute = null;
            string part = null;
            string variable = null;
            string startDate = null;
            string stopDate = null;
            //if (startDateFlag)
            //{
            //    DateTime? dt = dpStartDate.SelectedDate;
            //    startDate = dt.ToString();
            //}

            //if (stopDateFlag)
            //{
            //    DateTime? dt = dpStopDate.SelectedDate;
            //    stopDate = dt.ToString();
            //}


            //if (cbStartHour.SelectedIndex != -1 && startHourFlag)
            //{
            //    ComboBoxItem selectedStartHour = cbStartHour.SelectedItem as ComboBoxItem;
            //    startHour = selectedStartHour.Content.ToString();
            //    startHourFlag = false;
            //}

            //if (cbStartMinute.SelectedIndex != -1 && startMinuteFlag)
            //{
            //    ComboBoxItem selectedStartMinute = cbStartMinute.SelectedItem as ComboBoxItem;
            //    startMinute = selectedStartMinute.Content.ToString();
            //    startMinuteFlag = false;
            //}
            //if (cbStopHour.SelectedIndex != -1 && stopHourFlag)
            //{
            //    ComboBoxItem selectedStopHour = cbStopHour.SelectedItem as ComboBoxItem;
            //    stopHour = selectedStopHour.Content.ToString();
            //    stopHourFlag = false;
            //}

            //if (cbStopMinute.SelectedIndex != -1 && stopMinuteFlag)
            //{
            //    ComboBoxItem selectedStopMinute = cbStopMinute.SelectedItem as ComboBoxItem;
            //    stopMinute = selectedStopMinute.Content.ToString();
            //    stopMinuteFlag = false;
            //}

            if (cbPart.SelectedIndex != -1 && partFlag)
            {
                ComboBoxItem selectedPart = cbPart.SelectedItem as ComboBoxItem;
                part = selectedPart.Content.ToString();
                partFlag = false;
            }

            if (cbVariable.SelectedIndex != -1 && variableFlag)
            {
                ComboBoxItem selectedVariable = cbVariable.SelectedItem as ComboBoxItem;
                variable = selectedVariable.Content.ToString();
                variableFlag = false;
            }

            this.startHour = startHour;
            this.stopHour = stopHour;
            this.startMinute = startMinute;
            this.stopMinute = stopMinute;
            this.part = part;
            this.variable = variable;
            this.startDate = startDate;
            this.stopDate = stopDate;
            //return startHour,startMinute,
        }

        // 判断是否选择了起止日期,并更改标志位
        private void dtStartTime(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.startTimeFlag = true;
        }

        private void dtStopTime(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.stopTimeFlag = true;
        }

        //private void DpStartDate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    DateTime? dt = dpStartDate.SelectedDate;
        //    this.startDate = dt.ToString();
        //}

        //private void DpStopDate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    DateTime? dt = dpStopDate.SelectedDate;
        //    this.stopDate = dt.ToString();
        //}

    }


}
