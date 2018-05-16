using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp5
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string hsps = GrabAllHsp().ToString();
            DirectoryInfo d = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent;
            string jsonpath = d.FullName + "\\" + ConfigurationManager.AppSettings["datafile"];

            FileStream fs = new FileStream(jsonpath, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(hsps);
            sw.Close(); 

            Console.ReadKey();
        }
        //抓取网页 所有医院 信息
        static StringBuilder GrabAllHsp()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"cityList\": { ");
            HtmlWeb web = new HtmlWeb();
            string url = "http://z.xywy.com/yiyuan.htm";
            web.OverrideEncoding = Encoding.Default;

            HtmlDocument doc = web.Load(url);
            //取指定ul标签
            List<HtmlNode> ul_node = doc.DocumentNode.Descendants("ul").Where(c => c.Attributes["class"].Value.Equals("jib-classification clearfix")).ToList();

            //取指定ul下所有li标签
            List<HtmlNode> li_nodes = ul_node[0].ChildNodes.Where(c => c.Name.Equals("li")).ToList();
            Console.WriteLine("start...");
            //Dictionary<string, string> url_name_s = new Dictionary<string, string>();
            for (int i = 0; i < li_nodes.Count; i++)
            {
                //取li标签下 a 和 span
                HtmlNode li_node = li_nodes[i].ChildNodes.Where(c => c.Name.Equals("a")).First();
                string li_node_url = li_node.Attributes["href"].Value;
                string li_node_name = li_node.Descendants("span").First().InnerText;
                //url_name_s.Add(li_node_name, li_node_url);

                json.Append("\"" + li_node_name + "\":{");
                Console.WriteLine("开始爬取--" + li_node_name);

                HtmlDocument doc_city = web.Load(li_node_url);
                List<HtmlNode> dd_nodes = doc_city.DocumentNode.Descendants("dd").Where(c => !c.InnerText.Equals("全部")).ToList();

                for (int j = 0; j < dd_nodes.Count; j++)
                {
                    HtmlNode dd_node = dd_nodes[j].ChildNodes.Where(c => c.Name.Equals("a")).First();
                    string dd_node_url = dd_node.Attributes["href"].Value;
                    string dd_node_name = dd_node.InnerText;

                    json.Append("\"" + dd_node_name + "\":[");
                    Console.WriteLine("开始爬取--" + li_node_name + "--" + dd_node_name);

                    HtmlDocument doc_hsp = web.Load(dd_node_url);
                    List<HtmlNode> ul_hsp_nodes = doc_hsp.DocumentNode.Descendants("ul").Where(c => c.Attributes["class"].Value.Equals("clearfix")).ToList();
                    List<HtmlNode> li_hsp_nodes = ul_hsp_nodes[0].ChildNodes.Where(c => c.Name.Equals("li")).ToList();
                    for (int k = 0; k < li_hsp_nodes.Count; k++)
                    {
                        HtmlNode li_hsp_node = li_hsp_nodes[k].ChildNodes.Where(c => c.Name.Equals("a")).First();
                        HtmlNode li_hsp_node2 = li_hsp_nodes[k].ChildNodes.Where(c => c.Name.Equals("span")).First();
                        string hsp_name = li_hsp_node.InnerText;
                        string hsp_attr = li_hsp_node2.InnerText;
                        json.Append("\"" + hsp_name + "\",");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("],");
                    Console.WriteLine("结束爬取--" + li_node_name + "--" + dd_node_name);
                }
                json.Remove(json.Length - 1, 1);
                json.Append("},");
                Console.WriteLine("结束爬取--" + li_node_name);
            }
            json.Remove(json.Length - 1, 1);
            json.Append("}}");
            Console.WriteLine("end...");
            return json;
        }
    }
}
