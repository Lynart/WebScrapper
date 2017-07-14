using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var html = GetResourceFromURL();
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

        /// <summary>
        /// Returns text that is retrieved from the URL in the .config file
        /// </summary>
        /// <returns>null if HttpStatusCode is not OK</returns>
        static string GetResourceFromURL()
        {
            string urlAddress = ConfigurationManager.AppSettings.Get("ScrapThisUrl");
            string data = null;
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

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            return data;
        }
    }
}
