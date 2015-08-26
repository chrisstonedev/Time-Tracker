using System;
using System.Collections.Generic;
using System.IO;
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

namespace Time_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridData mGridData = new GridData();
        private string mDocPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TimeTracker.csv";
        private static double WINDOW_HEIGHT;
        private static int ROW_HEIGHT = 22;
        public MainWindow()
        {
            InitializeComponent();
            WINDOW_HEIGHT = myWindow.Height;
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            loadFromFile();
        }
        private void refreshGrid()
        {
            myGrid.Children.Clear();

            var keyDict = new Dictionary<string, int[]>();
            for (int i = 0; i < mGridData.Count; i++)
            {
                Label lblTime = new Label();
                Label lblKey = new Label();
                Label lblDesc = new Label();

                lblTime.Content = mGridData.Item(i).Time;
                lblKey.Content = mGridData.Item(i).Key;
                lblDesc.Content = mGridData.Item(i).Description;
                lblTime.Padding = new Thickness(1, 1, 1, 1);
                lblKey.Padding = new Thickness(1, 1, 1, 1);
                lblDesc.Padding = new Thickness(1, 1, 1, 1);

                Grid.SetRow(lblTime, i);
                Grid.SetRow(lblKey, i);
                Grid.SetRow(lblDesc, i);
                Grid.SetColumn(lblTime, 0);
                Grid.SetColumn(lblKey, 1);
                Grid.SetColumn(lblDesc, 2);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(ROW_HEIGHT);
                myGrid.RowDefinitions.Add(rowDef);

                myGrid.Children.Add(lblTime);
                myGrid.Children.Add(lblKey);
                myGrid.Children.Add(lblDesc);

                if (keyDict.ContainsKey(mGridData.Item(i).Key))
                {
                    int[] data = keyDict[mGridData.Item(i).Key];
                    data[0] = i;
                    data[1] += mGridData.TimeElapsedAt(i);
                    keyDict[mGridData.Item(i).Key] = data;
                }
                else
                {
                    keyDict.Add(mGridData.Item(i).Key, new int[] { i, mGridData.TimeElapsedAt(i) });
                }
            }

            foreach (KeyValuePair<string, int[]> entry in keyDict)
            {
                Label lblDynamic = new Label();
                lblDynamic.Content = StringUtilities.formatTimeText(entry.Value[1]);
                lblDynamic.Padding = new Thickness(1, 1, 1, 1);
                Grid.SetRow(lblDynamic, entry.Value[0]);
                Grid.SetColumn(lblDynamic, 3);
                myGrid.Children.Add(lblDynamic);
            }

            myWindow.Height = WINDOW_HEIGHT + ROW_HEIGHT * mGridData.Count;

            txtKey.Text = "";
            txtDescription.Text = "";

            txtKey.Focus();
        }
        private void addLinetoFile()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine(String.Join(",", mGridData.Last().ToArray()));
            try
            {
                using (StreamWriter outFile = new StreamWriter(mDocPath, true))
                {
                    outFile.Write(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Add operation failed. Please contact developer for more details. " +
                        "Try adding 'TimeHelp.csv' to your standard Documents directory.", "Coming Soon");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            addTransaction(new string[] { txtKey.Text, txtDescription.Text });
        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            switch(e.Reason)
            {
                case Microsoft.Win32.SessionSwitchReason.SessionLock:
                    addTransaction(new string[] { "BREAK", "Computer locked" });
                    break;
                case Microsoft.Win32.SessionSwitchReason.SessionUnlock:
                    addTransaction(mGridData.Item(mGridData.Count - 2).ToArray());
                    break;
            }
        }
        private void addTransaction(string[] transaction)
        {
            if (transaction.Length == 2)
            {
                mGridData.Add(new string[] { DateTime.Now.ToString("h:mm tt"), transaction[0], transaction[1] });
            }
            else
            {
                transaction[0] = DateTime.Now.ToString("h:mm tt");
                mGridData.Add(transaction);
            }
            addLinetoFile();
            refreshGrid();
        }
        private void loadFromFile()
        {
            if (File.Exists(mDocPath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(mDocPath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();
                            string[] data = line.Split(',');
                            mGridData.Add(data);
                        }
                        refreshGrid();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Add operation failed. Please contact developer for more details. " +
                            "Try adding 'TimeHelp.csv' to your standard Documents directory.", "Coming Soon");
                }
            }
        }
    }
}