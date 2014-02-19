using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class Config
{
    private const string INTERVAL_TAG = "interval";
    private const string SVN_PATH_TAG = "path";
    private string filePath = "../../config.xml";
    Dictionary<string, Object> configDictionary = new Dictionary<string, object>();
    
    public int Interval
    {
        get { return int.Parse((string)configDictionary[INTERVAL_TAG]); }
    }
    public string SvnPath
    {
        get { return (string)configDictionary[SVN_PATH_TAG]; }
    }
    public void Read()
    {
        
        using (XmlReader reader = XmlReader.Create(filePath))
        {
            while (reader.Read())
            {
                // 開始タグを発見した場合
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "svn")
                    {
                        reader.MoveToAttribute(0);
                        configDictionary[SVN_PATH_TAG] = reader.Value;
                    }
                    else if (reader.LocalName == INTERVAL_TAG)
                    {
                        reader.MoveToAttribute(0);
                        configDictionary[INTERVAL_TAG] = (Object)reader.Value;
                        Console.WriteLine("configDictionary[INTERVAL_TAG] " + configDictionary[INTERVAL_TAG]);
                    }
                    Console.WriteLine("開始タグ : {0} (深さ : {1})",
                        reader.LocalName, reader.Depth);
                    // 属性があった場合
                    if (reader.HasAttributes)
                    {
                        // すべての属性を表示
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            // 属性ノードへ移動
                            reader.MoveToAttribute(i);
                            // 属性名、及び属性の値を表示
                            Console.Write("{1} = {2} ", i, reader.Name, reader.Value);
                        }
                        Console.Write(System.Environment.NewLine);
                        // すべての属性を出力したら、元のノード(エレメントノード)に戻る
                        reader.MoveToElement();
                    }
                }
            }
        }
    }
}