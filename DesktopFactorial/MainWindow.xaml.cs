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
        private  BackgroundWorker worker = new BackgroundWorker();
        private int value = 0;

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
        }

        private void OnClick1(object sender, RoutedEventArgs e)
        {
            string str = tbValue.Text;

            if (!Int32.TryParse(str, out value))
            {
                tbMonitor.Text = str + " is not a number";
                return;
            }
            btCalc.IsEnabled = false;
            tbValue.IsEnabled = false;

            tbMonitor.Text = String.Empty;
            pbStatus.Maximum = value;

            /*for (int i = 1; i <= value; i++)
            {
                BigInteger res = new BigInteger();
                if (i >= 2)
                {
                    res = GetFactorial(i);
                    tbMonitor.Text += String.Format("{0}!={1}\n", i, res.ToString());
                }
                pbStatus.Value = i;                
            }*/
            for (int i = 1; i <= value; i++)
            {
                worker.RunWorkerAsync(i);
                worker.CancelAsync();
            }           
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = GetFactorial((int)e.Argument, worker);                
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            tbMonitor.Text += String.Format("{0}!={1}\n", 1, e.Result.ToString());
            
            btCalc.IsEnabled = true;
            tbValue.IsEnabled = true;
        }

        private void worker_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        private BigInteger GetFactorial(int val, BackgroundWorker bworker)
        {
            if (val < 0)
            {
                throw new ArgumentException(
                    "value must be >= 0", "val");
            }
            BigInteger res = 1;
            for (int i = 1; i <= val; i++)
            {
                res *= i;

                int percentComplete =
                    (int)((float)i / (float)val * 100);
               // bworker.ReportProgress(percentComplete);
            }

            return res;
        }
    }
}