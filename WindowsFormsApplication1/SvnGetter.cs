using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


    
class SvnGetter
{
    public string svnPath = "C:/cygwin/bin/svn.exe";
    private string svnInfomationXml = "";

        public bool GetInfomation(string queryUrl)
        {
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            //出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            //ウィンドウを表示しないようにする
            p.StartInfo.CreateNoWindow = true;
            //コマンドラインを指定（"/c"は実行後閉じるために必要）
            // svn info $url --xml
            p.StartInfo.Arguments = "/c c:/\"Program Files (x86)\"/Subversion/bin/svn.exe info --xml " + queryUrl;
            //p.StartInfo.Arguments = "/c " + svn +  " info --xml " + queryUrl;
            //起動
            p.Start();

            //出力を読み取る
            svnInfomationXml = p.StandardOutput.ReadToEnd();
        
            //プロセス終了まで待機する
            //WaitForExitはReadToEndの後である必要がある
            //(親プロセス、子プロセスでブロック防止のため)
            p.WaitForExit();
            p.Close();

            return true;
        }

        public string GetRevisionNumber()
        {
            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(svnInfomationXml)))
            {
                reader.ReadToFollowing("entry");
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name == "revision")
                        return reader.Value;
                }
            }
            return "";
        }
    }

