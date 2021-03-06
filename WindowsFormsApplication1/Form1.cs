﻿using System;
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
            TipsCollection.Initialize(this.label2);
            this.config.Read();

            // １秒単位でイベントを発生させる
            timer1.Interval = this.config.Interval;
            timer2.Interval = timer1.Interval;
            // タイマーを有効に
            timer1.Enabled = true;
            timer2.Enabled = true;

            this.svnGetter.svnPath = this.config.SvnPath;
            Console.WriteLine("this.config.Interval " + this.config.Interval);
            Console.WriteLine("this.config.SvnPath " + this.config.SvnPath);
            foreach (var item in this.config.Repository)
            {
                this.UrlListBox.Items.Add(item.url);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;                  // 終了処理キャンセル
                this.Visible = false;                 // フォーム非表示
                this.notifyIcon1.Visible = true;      // Notifyアイコン表示
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
            this.config.Write();
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
            Parallel.ForEach(this.UrlListBox.Items.Cast<String>().ToList(), x =>
            {
                Console.WriteLine("url " + x);
                svnGetter.GetInfomation(x);
                if (this.config.Repository.Contains(x) && this.config.Repository[x].revision < svnGetter.GetRevisionNumber())
                {
                    Console.WriteLine(svnGetter.GetRevisionNumber());
                    this.config.Repository[x].revision = svnGetter.GetRevisionNumber();
                    //バルーンヒントのタイトル
                    this.notifyIcon1.BalloonTipTitle = "Committed";
                    //バルーンヒントに表示するメッセージ
                    this.notifyIcon1.BalloonTipText = this.config.Repository[x].url;
                    this.notifyIcon1.ShowBalloonTip(10000); // バルーンTip表示
                    this.config.Repository[x].Update();
                }
                else if ( ! this.config.Repository.Contains(x))
                {
                    this.config.Repository.Add(new RepositoryData(x, 0));
                    this.config.Repository[x].revision = svnGetter.GetRevisionNumber();
                    //バルーンヒントのタイトル
                    this.notifyIcon1.BalloonTipTitle = "Committed";
                    //バルーンヒントに表示するメッセージ
                    this.notifyIcon1.BalloonTipText = this.config.Repository[x].url;
                    this.notifyIcon1.ShowBalloonTip(10000); // バルーンTip表示
                    this.config.Repository[x].Update();
                }
            });
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UrlListBox.Items.Add(UrlBox.Text);
            UrlListBox.TopIndex = UrlListBox.Items.Count - UrlListBox.Height / UrlListBox.ItemHeight; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (UrlListBox.Items.Count > 0 && UrlListBox.SelectedItems.Count > 0)
                UrlListBox.Items.Remove(UrlListBox.SelectedItems[0]);
        }

        private void notifyIcon1_MouseClick(object sender, EventArgs e)
        {
            string url = ((NotifyIcon)sender).BalloonTipText;
            if (this.config.Repository[url].IsUpdated())
            {
                this.config.Repository[url].Check();
                Console.WriteLine("notifyIcon1_MouseClick " + this.config.Repository[url].url);

                // start repobrowser
                MSDOSCommand.Excute("/c TortoiseProc.exe /command:repobrowser /path:" + url);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (var item in this.config.Repository)
            {
                if (item.IsUpdated())
                {
                    item.IsChecked();
                    Console.WriteLine(item.url);
                    //バルーンヒントのタイトル
                    this.notifyIcon1.BalloonTipTitle = "Committed";
                    //バルーンヒントに表示するメッセージ
                    this.notifyIcon1.BalloonTipText = item.url;
                    this.notifyIcon1.ShowBalloonTip(10000); // バルーンTip表示
                }
            }
        }
    }
}
