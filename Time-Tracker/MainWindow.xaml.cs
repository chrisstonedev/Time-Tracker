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
        List<string[]> m_oCollection = new List<string[]>();
        string m_sDocPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TimeTracker.csv";
        public MainWindow()
        {
            InitializeComponent();
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            loadFromFile();
        }
        private void addLineToGrid()
        {
            Label lblTime = new Label();
            Label lblKey = new Label();
            Label lblDesc = new Label();

            //lblUpdated.Content = String.Format("Last update ({0}):", time);
            lblTime.Content = m_oCollection[m_oCollection.Count - 1][0];
            lblKey.Content = m_oCollection[m_oCollection.Count - 1][1];
            lblDesc.Content = m_oCollection[m_oCollection.Count - 1][2];

            Grid.SetRow(lblTime, m_oCollection.Count - 1);
            Grid.SetRow(lblKey, m_oCollection.Count - 1);
            Grid.SetRow(lblDesc, m_oCollection.Count - 1);
            Grid.SetColumn(lblTime, 0);
            Grid.SetColumn(lblKey, 1);
            Grid.SetColumn(lblDesc, 2);

            myWindow.Height += 20;

            myGrid.RowDefinitions.Add(new RowDefinition());
            myGrid.Children.Add(lblTime);
            myGrid.Children.Add(lblKey);
            myGrid.Children.Add(lblDesc);

            txtKey.Text = "";
            txtDescription.Text = "";

            txtKey.Focus();
        }
        private void addLinetoFile()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine(String.Join(",", m_oCollection.Last()));
            try
            {
                using (StreamWriter outFile = new StreamWriter(m_sDocPath, true))
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
                    addTransaction(m_oCollection[m_oCollection.Count - 2]);
                    break;
            }
        }
        private void addTransaction(string[] transaction)
        {
            if (transaction.Length == 2)
            {
                m_oCollection.Add(new string[] {DateTime.Now.ToString("h:mm tt"), transaction[0], transaction[1]});
            }
            else
            {
                m_oCollection.Add(transaction);
            }
            addLinetoFile();
            addLineToGrid();
        }
        private void loadFromFile()
        {
            if (File.Exists(m_sDocPath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(m_sDocPath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();
                            string[] data = line.Split(',');
                            m_oCollection.Add(data);
                            addLineToGrid();
                        }
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
