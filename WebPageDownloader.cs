using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebPageDownloader
{
    class WebPageDownloader
    {
        public void Download(Uri parent)
        {
            var WCParent = new WebClient();
            string pattern = @"href=""(?<url>http(s)?[\w\.:?&-_=#/]*)""+?";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> links = new List<string>();

            string parentString = WCParent.DownloadString(parent);
            Console.WriteLine("This is {0} => {1}", parent, parentString.Length.ToString());

            MatchCollection matches = rgx.Matches(parentString);

            foreach (Match match in matches)
            {
                links.Add(match.ToString().Split(new Char[] { '"' })[1]);
            }

            if (links != null)
            {
                try
                {
                    Task.WaitAll(links.Select(DownloadChildren).ToArray());
                }
                catch { }
            }
        }

        private async Task DownloadChildren(string address)
        {
            using (var client = new WebClient())
            {
                var uri = new Uri(address);
                client.DownloadDataCompleted += Result;
                client.Headers["address"] = address;
                await client.DownloadDataTaskAsync(uri);
            }
        }

        private void Result(object obj, DownloadDataCompletedEventArgs e)
        {
            if (obj is WebClient client && !e.Cancelled && e.Error == null)
            {
                Console.WriteLine("{0} => {1}", client.Headers["address"], client.Encoding.GetString(e.Result).Length);
            }
        }
    }
}
