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

        public TestForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
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
            var tg = new ThreadGun<int>(ActionThreadGun, Enumerable.Range(1, NumCount), 20);
            tg.Completed += tg_Completed;
            tg.ExceptionOccurred += tg_ExceptionOccurred;
            tg.Start();
        }

        private void tg_ExceptionOccurred(IEnumerable<int> inputs, Exception exception)
        {
            MessageBox.Show(exception.Message);
        }

        private void tg_Completed(object inputs)
        {
            lblInfoThreadGun.Text = $@"Item Count : {lstThreadGunResult.Items.Count}";
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
            lstThreadGunResult.Items.Add($@"> {i} <");
            lblInfoThread.Text = $@"Item Count : {lstThread.Items.Count}";
            Application.DoEvents();
        }

        public void ActionThreadGun(int i)
        {
            lstThreadGunResult.Items.Add($@"> {i} <");
            Application.DoEvents();
            if (i == 250)
                throw new Exception("ExceptionOccurred Test!");
        }

        public void ActionThreadPool(object i)
        {
            lstThreadPoolResult.Items.Add($@"> {i} <");
            Application.DoEvents();
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