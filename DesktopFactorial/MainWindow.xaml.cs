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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using System.Threading;
using System.ComponentModel;

namespace DesktopFactorial
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private  BackgroundWorker worker;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }
        private void InitializeBackgroundWorker()
        {
            worker = new BackgroundWorker(); // variable declared in the class
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void button_ClickCalc(object sender, RoutedEventArgs e)
        {
            string str = tbValue.Text;
            int value = 0;

            if (!Int32.TryParse(str, out value))
            {
                tbMonitor.Text = str + " is not a number";
                return;
            }
            if(value < 0)
            {
                tbMonitor.Text = "Your number is too small "+ str;
                return;
            }

            btCalc.IsEnabled = false;
            tbValue.IsEnabled = false;

            tbMonitor.Text = String.Empty;
            pbStatus.Maximum = value;

            worker.RunWorkerAsync(value);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 2; i <= (int)e.Argument; i++)
            {
                BigInteger res = new BigInteger();
                res = GetFactorial(i);
                
                worker.ReportProgress(i, new[]{i, res});
                
                Thread.Sleep(50);
            }
            
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
            BigInteger[] ar = (BigInteger[])e.UserState;
            tbMonitor.Text += String.Format("{0}!={1}\n", ar[0], ar[1]);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            btCalc.IsEnabled = true;
            tbValue.IsEnabled = true;
        }

        private BigInteger GetFactorial(int val)
        {
            BigInteger res = 1;
            for (int i = 1; i <= val; i++)
                res *= i;
            return res;
        }
    }
}