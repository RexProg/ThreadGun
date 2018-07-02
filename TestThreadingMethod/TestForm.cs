#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ThreadGun;

#endregion

namespace TestThreadingMethod
{
    public partial class TestForm : Form
    {
        private const int NumCount = 20000;
        private ThreadGun<int> _tg;

        public TestForm()
        {
            InitializeComponent();
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            lstThread.Items.Clear();
            foreach (var i in Enumerable.Range(1, NumCount)) ActionThread(i);
        }

        private void btnThread_Click(object sender, EventArgs e)
        {
            lstThread.Items.Clear();
            new Thread(() =>
            {
                foreach (var i in Enumerable.Range(1, NumCount)) ActionThread(i);
            }).Start();
        }

        private void btnThreadGun_Click(object sender, EventArgs e)
        {
            lstThreadGunResult.Items.Clear();
            _tg = new ThreadGun<int>((Action<int>) ActionThreadGun, Enumerable.Range(1, NumCount), 20,
                tg_ExceptionOccurred);
            _tg.Completed += tg_Completed;
            _tg.FillingMagazine();
            _tg.Start();
        }

        private void tg_ExceptionOccurred(ThreadGun<int> gun, IEnumerable<int> inputs, object input,
            Exception exception)
        {
            MessageBox.Show($@"Exception Occurred!!!

Message :
{exception.Message}

Input :
{(int) input}");
        }

        private void tg_Completed(object inputs)
        {
            MessageBox.Show(@"ThreadGun Process Completed!");
        }

        private void btnThreadPool_Click(object sender, EventArgs e)
        {
            lstThreadPoolResult.Items.Clear();
            ThreadPool.SetMaxThreads(20, 20);
            ThreadPool.SetMinThreads(20, 20);
            foreach (var i in Enumerable.Range(1, NumCount))
                ThreadPool.QueueUserWorkItem(ActionThreadPool, i);
        }

        public void ActionThread(int i)
        {
            Invoke(new MethodInvoker(delegate
            {
                lstThreadGunResult.Items.Add($@"> {i} <");
                lblInfoThread.Text = $@"Item Count : {lstThread.Items.Count}";
            }));
            Application.DoEvents();
        }

        public void ActionThreadGun(int i)
        {
            Invoke(new MethodInvoker(delegate
            {
                lstThreadGunResult.Items.Add($@"> {i} <");
                lblInfoThreadGun.Text = $@"Item Count : {lstThreadGunResult.Items.Count}";
            }));
            Application.DoEvents();
            if (i == 250)
                throw new Exception("ExceptionOccurred Test!");
            //if (i == 500)
            //{
            //    MessageBox.Show(@"I'll wait ten minutes");
            //    _tg.Wait(10 * 60 * 1000, () => MessageBox.Show(@"I continue process"));
            //}
        }

        public void ActionThreadPool(object i)
        {
            try
            {
                if ((int) i == NumCount)
                    MessageBox.Show(@"ThreadPool Process Completed!");
                Invoke(new MethodInvoker(delegate
                {
                    lstThreadPoolResult.Items.Add($@"> {i} <");
                    lblInfoThreadPool.Text = $@"Item Count : {lstThreadPoolResult.Items.Count}";
                }));
                Application.DoEvents();
                if ((int) i == 250)
                    throw new Exception("ExceptionOccurred Test!");
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Exception Occurred!!!

Message :
{exception.Message}

Input :
{i}");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lblInfoThreadPool.Text = $@"Item Count : {lstThreadPoolResult.Items.Count}";
            lblInfoThreadGun.Text = $@"Item Count : {lstThreadGunResult.Items.Count}";
            lblInfoThread.Text = $@"Item Count : {lstThread.Items.Count}";
        }
    }
}