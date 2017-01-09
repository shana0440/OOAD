using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace WPF_Windows_Spotlight.Models
{
    class Crawler
    {
        public static string GetResponse(string url)
        {
            var isNetWorkAvailable = NetworkInterface.GetIsNetworkAvailable();
            if (!isNetWorkAvailable)
            {
                throw new WebException("沒有連接至網際網路");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string html = stream.ReadToEnd();
                return html;
            }
        }
    }
}
