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
        private int value = 0;

        public MainWindow()
        {
            InitializeComponent();
            worker = new BackgroundWorker(); // variable declared in the class
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void OnClick1(object sender, RoutedEventArgs e)
        {
            string str = tbValue.Text;

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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= (int)e.Argument; i++)
            {
                BigInteger res = new BigInteger();
                if (i >= 2)
                {
                    res = GetFactorial(i);
                    try
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate()
                        {
                            tbMonitor.Text += String.Format("{0}!={1}\n", i, res.ToString());
                        });
                    }catch(NullReferenceException )
                    {
                        MessageBox.Show("Exception appears");
                    }

                    
                    worker.ReportProgress(i);
                }
                Thread.Sleep(50);
            }
            
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            btCalc.IsEnabled = true;
            tbValue.IsEnabled = true;
        }

        private BigInteger GetFactorial(int val)
        {
            
            BigInteger res = 1;
            for (int i = 1; i <= val; i++)
            {
                res *= i;
            }

            return res;
        }
    }
}