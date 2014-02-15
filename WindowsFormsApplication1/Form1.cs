using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.ShowBalloonTip(500);
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
    }
}
