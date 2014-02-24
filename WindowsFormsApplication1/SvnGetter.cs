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
            svnInfomationXml = MSDOSCommand.Excute("/c " + svnPath + " info --xml " + queryUrl);
            Console.WriteLine("svnInfomationXml " + svnInfomationXml); 
            return true;
        }

        public int GetRevisionNumber()
        {
            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(svnInfomationXml)))
            {
                try
                {
                    reader.ReadToFollowing("entry");
                }   
                catch (Exception e)
                {
                    Console.WriteLine(e + " " +svnInfomationXml);    
                }
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name == "revision")
                    {
                        try
                        {
                            return int.Parse(reader.Value);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " reader.Value (" + reader.Value + ")");
                        }
                    }
                }
            }
            return 0;
        }
    }

