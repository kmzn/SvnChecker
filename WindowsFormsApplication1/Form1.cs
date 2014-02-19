using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private SvnGetter svnGetter = new SvnGetter();
        private Config config = new Config();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.config.Read();

            // １秒単位でイベントを発生させる
            timer1.Interval = this.config.Interval;

            // タイマーを有効に
            timer1.Enabled = true;

            this.svnGetter.svnPath = this.config.SvnPath;

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;                  // 終了処理キャンセル
                this.Visible = false;                 // フォーム非表示
                this.notifyIcon1.Visible = true;      // Notifyアイコン表示
                this.notifyIcon1.ShowBalloonTip(500); // バルーンTip表示
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;               // フォームの表示
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal; // 最小化をやめる
            }
            this.notifyIcon1.Visible = false;  // Notifyアイコン非表示
            this.Activate();   
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            // タスクトレイからアイコンを取り除く
            Application.Exit();
            // アプリケーション終了
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(GetSvnInfomation));
            thread.Start();
        }

        private void GetSvnInfomation()
        {
            Parallel.ForEach(this.config.RepositoryData, x =>
            {
                svnGetter.GetInfomation(x.url);
                Console.WriteLine(svnGetter.GetRevisionNumber());

            });
            
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            this.label2.Text = "Input New URL";
            Console.WriteLine(((Control)sender).Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text);
            listBox1.TopIndex = listBox1.Items.Count - listBox1.Height / listBox1.ItemHeight; 
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            this.label2.Text = "Add New URL";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0 && listBox1.SelectedItems.Count > 0)
                listBox1.Items.Remove(listBox1.SelectedItems[0]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
