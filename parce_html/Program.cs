using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace parce_html
{
    class Program
    {
        static string fileName = "Сообщения2.html";
        static void generateHTML()
        {
            Random rnd = new Random();
            int code = Convert.ToInt32('А');
            StreamWriter sw = new StreamWriter("index.html");
            sw.WriteLine("<html><body><table border=\"1\">");
            for (int i = 0; i < 10; i++)
                sw.WriteLine("<tr><td>" + i.ToString() + "</td><td>" + (rnd.Next(32) + code).ToString() + "</td></tr>");
            sw.WriteLine("</table></body></html>");
            sw.Close();
        }
        static void parceUseString()
        {
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                int b1 = 0, b2 = 0;
                string s = sr.ReadLine();

                if (s.Contains("news"))
                {
                    while (b1 != -1)
                    {
                        b1 = s.IndexOf("<h2>");
                        b2 = s.IndexOf("</h2>");
                        Console.WriteLine(s.Substring(b1 + 4, b2 - b1 - 4));
                        s = s.Substring(b2 + 4);
                        //Console.WriteLine("!" + s);

                    }
                    break;
                }

            }
            sr.Close();
        }
       public static string GetPage(string url)
       {
            var result = String.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
 
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader streamReader;
                    if (response.CharacterSet != null)
                        streamReader = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet));
                    else
                        streamReader = new StreamReader(responseStream);
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                response.Close();
            }
            return result;
       }

        static void Main(string[] args)
        {
           /* var srcEncoding = Encoding.GetEncoding(1251);
            StreamReader sr = new StreamReader(fileName, encoding: srcEncoding);
            var txtHTML = sr.ReadToEnd();
            sr.Close();
            */
            var txtHTML = GetPage(@"https://vk.com/im?peers=176834784_221576632_c69_364969321_c210_233677838_265470711_c185&sel=c208");
            Console.WriteLine(txtHTML);
            StreamWriter sw = new StreamWriter("test.txt");
            sw.WriteLine(txtHTML);
            sw.Close();
            var doc = new HtmlDocument();   
            doc.LoadHtml(txtHTML);
            /*
            // Вывести все содержимое тега
            var mess = doc.DocumentNode.SelectNodes("//div[contains(@class, 'im-mess-stack--content')]").Last();
            //Console.WriteLine(mess.InnerHtml);
            //Console.WriteLine(mess.InnerText);
            
            var allTag = mess.SelectNodes(".//a");
            Console.WriteLine(allTag.Count());
            foreach (var t in allTag)
            {
                if (t.InnerText != "")
                {
                    Console.WriteLine(t.InnerText);
                }
            }
            */

            //Вывести всех кто писал сообщения
            Dictionary<string, int> userCount = new Dictionary<string, int>();
            var messNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'im-mess-stack--content')]");
            try
            {
                Console.WriteLine(messNodes.Count);
                foreach (var m in messNodes)
                {
                    var allA = m.SelectSingleNode(".//a[contains(@class, 'im-mess-stack--lnk')]");

                    //Console.WriteLine(allA.InnerText);
                    if (userCount.ContainsKey(allA.InnerText))
                        userCount[allA.InnerText] += 1;
                    else
                        userCount[allA.InnerText] = 1;
                }

                foreach (var user in userCount)
                    Console.WriteLine(user.Key + " " + user.Value);

            }
            catch
            {
                Console.WriteLine("Не нашлось!");
            }





        }
    }
}
