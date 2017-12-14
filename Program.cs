using System;
using System.Diagnostics;

namespace WebPageDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch time = new Stopwatch();
            WebPageDownloader WPD = new WebPageDownloader();

            Console.WriteLine("Enter URI:");
            input:
            try
            {
                Uri input = new Uri(Console.ReadLine());
                time.Start();
                WPD.Download(input);
                time.Stop();
                Console.WriteLine("Time: {0}", time.Elapsed);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Wrong input. Try again:");
                goto input;
            }
            catch(UriFormatException)
            {
                Console.WriteLine("Wrong input. Try again:");
                goto input;
            }

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
        }
    }
}
