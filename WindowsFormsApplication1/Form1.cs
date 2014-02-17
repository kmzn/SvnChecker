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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // 秒数
        private int sec = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.ShowBalloonTip(500);

            // １秒単位でイベントを発生させる
            timer1.Interval = 10000;

            // タイマーを有効に
            timer1.Enabled = true;
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
            sec++;
            Console.WriteLine(sec.ToString() + "秒経過...");

            SvnGetter svnGetter = new SvnGetter();
            svnGetter.GetInfomation(url);
            Console.WriteLine(svnGetter.GetRevisionNumber());
        }
    }
}
