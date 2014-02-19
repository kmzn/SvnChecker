using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class Config
{
    private const string REPOSITORY_TAG = "repository";
    private const string INTERVAL_TAG = "interval";
    private const string SVN_PATH_TAG = "path";
    private string filePath = "../../config.xml";
    private Dictionary<string, Object> configDictionary = new Dictionary<string, object>();
    private RepositoryDataCollection svnDataCollection = new RepositoryDataCollection();
    
    public int Interval
    {
        get { return int.Parse((string)configDictionary[INTERVAL_TAG]); }
    }
    public string SvnPath
    {
        get { return (string)configDictionary[SVN_PATH_TAG]; }
    }
    public RepositoryDataCollection RepositoryData
    {
        get { return (RepositoryDataCollection)configDictionary[REPOSITORY_TAG]; }
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
                        configDictionary[INTERVAL_TAG] = reader.Value;
                    }
                    else if (reader.LocalName == REPOSITORY_TAG)
                    {
                        // 属性があった場合
                        if (reader.HasAttributes)
                        {
                            int rev = 0;
                            string url = "";
                            // すべての属性を表示
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                // 属性ノードへ移動
                                reader.MoveToAttribute(i);
                                
                                if (reader.Name == "url")
                                {
                                    url = reader.Value;
                                }
                                else if (reader.Name == "revision")
                                {
                                    try {
                                        rev = int.Parse(reader.Value);
                                    }
                                    catch (Exception e) {
                                        Console.WriteLine(e + " reader.Value (" + reader.Value + ")");
                                    }
                                }
                                
                            }
                            // すべての属性を出力したら、元のノード(エレメントノード)に戻る
                            reader.MoveToElement();
                            svnDataCollection.Add(new RepositoryData(url, rev));
                        }
                    }
                }
            }
            configDictionary[REPOSITORY_TAG] = svnDataCollection;
        }
    }
}