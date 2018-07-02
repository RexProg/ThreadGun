using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using RestSharp;
using ThreadGun;

namespace ProxyGather
{
    class Program
    {
        public static int ProxyType;
        public static List<string> ProxyList = new List<string>();

        private static void Main()
        {
            while (ProxyType < 1 || ProxyType > 5)
            {
                Console.Clear();
                Console.WriteLine("What kind of proxy you needed?\r\n[1] SOCKS5\r\n[2] Https\r\n[3] Http\r\n[4] Http/Https\r\n[5] Everything");
                try
                {

                    ProxyType = int.Parse(Console.ReadLine() ?? "0");
                }
                catch { /* ignore */ }
            }

            Gathering();
            SaveProxies();
        }

        private static void Gathering()
        {
            var htmlDocument = new HtmlDocument();
            var content = new RestClient("https://checkerproxy.net/getAllProxy")
                .Execute(new RestRequest(Method.GET)).Content;
            htmlDocument.LoadHtml(content);
            var pages = htmlDocument.DocumentNode.SelectNodes("//div/ul/li/a")
                .Select(node => "https://checkerproxy.net/api" + node.Attributes["href"].Value).ToList();

            new ThreadGun<string>(page =>
            {
                foreach (var item in JArray.Parse(new RestClient(page).Execute(new RestRequest(Method.GET)).Content).Where(item => CheckProxyType(int.Parse(item["type"].ToString()))))
                    ProxyList.Add(item["addr"].ToString());
            }, pages, pages.Count).FillingMagazine().Start().Join();
        }

        private static bool CheckProxyType(int type)
        {
            switch (ProxyType)
            {
                case 49:
                    return type == 4;
                case 2:
                    return type == 2;
                case 3:
                    return type == 1;
                case 4:
                    return type == 1 || type == 2;
                default:
                    return true;
            }
        }

        private static void SaveProxies()
        {
            var file = new StreamWriter("proxylist.txt");
            foreach (var proxy in ProxyList)
                file.WriteLine(proxy);
            file.Close();
            Console.WriteLine("Saved!");
        }
    }
}
