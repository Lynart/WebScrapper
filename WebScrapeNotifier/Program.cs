using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailClient;
using System.Net;
using System.IO;
using System.Configuration;
using System.Threading;

namespace WebScrapeNotifier
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var html = GetHTML();
                if (html != null && KeyNotFound(html))
                    SendEmail();
                Thread.Sleep(600000);
            }
        }

        static void SendEmail()
        {
            EmailClient emailClient = new EmailClient();
            emailClient.ConstructEmail(new string[] { "toemail@email.com" }, "Oculus Rift Found", "GO ORDER IT NOW!");
            emailClient.Send();
        }

        static bool KeyNotFound(string html)
        {
            string key = ConfigurationManager.AppSettings.Get("TermToNotMatch");
            int found = html.IndexOf(key);

            return found == -1;
        }

        static string GetHTML()
        {
            string urlAddress = ConfigurationManager.AppSettings.Get("ScrapThisUrl");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return data;
            }
            return null;
        }
    }
}
