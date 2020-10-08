using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace change_Wallpaper2
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]

        private static extern int SystemParametersInfo (int uAction, int uParam, string lpvParam, int fuWinIni);
        
        static void Main(string[] args)
        {
            GetBackgroud();
        }

        public static void GetBackgroud()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://source.unsplash.com/random/1920x1080?hd,wallpapers"))
                {
                    var response = httpClient.SendAsync(request).Result;

                    Console.WriteLine(response.RequestMessage.RequestUri.AbsoluteUri);

                    Random rnd = new Random();

                    string localFilename = @"C:\Users\xalsi\AppData\Local\Temp\DynaWall\" + rnd.Next(1000, 9999) + ".bmp";
                    using (WebClient myWebClient = new WebClient())
                    {
                        try
                        {
                            string[] BmpList = Directory.GetFiles(@"C:\Users\xalsi\AppData\Local\Temp\DynaWall", "*.bmp");

                            // Delete source files that were copied.
                            foreach (string i in BmpList)
                            {
                                File.Delete(i);
                            }

                        }
                        catch (DirectoryNotFoundException dirNotFound)
                        {
                            Console.WriteLine(dirNotFound.Message);
                        }
                        
                        myWebClient.DownloadFile(response.RequestMessage.RequestUri.AbsoluteUri, localFilename);

                        RefreshWallpaper(localFilename);
                    }
                }
            }
        }

        public static void RefreshWallpaper(string path)
        {
            SystemParametersInfo( 20, 0, path, 0x01 | 0x02 );
        }
    }
}

