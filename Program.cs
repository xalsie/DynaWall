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
            DateTime thisDay = DateTime.Now;
                WriteLog(@"C:\Users\xalsi\AppData\Local\Temp\DynaWall\", "------- " + thisDay.ToString("F") + " -------\n" + "#  START DynaWall", true);
            
            GetBackgroud(thisDay);
        }

        public static void GetBackgroud(DateTime thisDay)
        {
            using (var httpClient = new HttpClient())
            {
                // using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://source.unsplash.com/random/1920x1080?hd,wallpapers"))

                int[] TabCoolection = {8680928, 11037116, 841333, 11635639, 18897794, 8253652, 1158549, 6805496, 1691032, 3802272, 3373312, 9468605, 2288557, 1905257, 4265857};

                Random rnd = new Random();

                var TadRndChoise = TabCoolection[rnd.Next(0, 16)];

                Console.WriteLine("#######################\n\n   -> " + TadRndChoise + "\n\n#######################");

                string uriR = "https://source.unsplash.com/collection/" + TadRndChoise + "/1920x1080";

                Console.WriteLine(uriR);

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), uriR))

                {
                    var response = httpClient.SendAsync(request).Result;

                    var TxtUri = "https://" + response.RequestMessage.RequestUri.Host + response.RequestMessage.RequestUri.AbsolutePath;

                    Console.WriteLine(TxtUri);

                    // Random rnd = new Random();

                    string path = @"C:\Users\xalsi\AppData\Local\Temp\DynaWall\";
                    string localFilename = path + rnd.Next(1000, 9999) + ".bmp";
                    using (WebClient myWebClient = new WebClient())
                    {
                        WriteLog(path, "Launch scan directory :", false);
                            VerifPath(path);

                        try
                        {
                            string[] BmpList = Directory.GetFiles(path, "*.bmp");

                            // Delete source files that were copied.
                            foreach (string i in BmpList) {
                                File.Delete(i);
                                    WriteLog(path, "    -> Delete file : " + i, false);
                            }
                        }
                        catch (DirectoryNotFoundException dirNotFound)
                        {
                            // Console.WriteLine(dirNotFound.Message);
                            WriteLog(path, dirNotFound.Message, false);
                        }
                        finally
                        {}

                        WriteLog(path, "\nImage Uri :\n" + "   -> " + TxtUri + "\n\nDownload image from Uri.\n", false);
                            myWebClient.DownloadFile(TxtUri, localFilename);
                        WriteLog(path, "Refresh Desktop WallPaper.", false);
                            RefreshWallpaper(localFilename, path);
                    }
                }
            }
        }

        public static void RefreshWallpaper(string localFilename, string path)
        {
            DateTime thisDay = DateTime.Now;
            WriteLog(path, "\n#  STOP\n" + "------- " + thisDay.ToString("F") + " -------\n##################################################", false);
                SystemParametersInfo( 20, 0, localFilename, 0x01 | 0x02 );
        }

        public static string VerifPath(string path)
        {
            string returnText = "";

            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    Console.WriteLine("    -> " + path);

                        returnText = "\n\n" + path;
                        returnText += "\n    -> That path exists already.\n";

                        Console.WriteLine(returnText);
                    return returnText;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                // Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                returnText = "\n\n" + path;
                returnText += "\n    -> The directory was created successfully at " + Directory.GetCreationTime(path) + "\n";
                return returnText;
            }
            catch (Exception e)
            {
                // Console.WriteLine("The process failed: {0}", e.ToString());
                returnText = "The process failed: {0}" + e.ToString();
                return returnText;
            }
            finally
            {}
        }

        public static void WriteLog(string path, string TxtLog, bool verifpathbool = true)
        {            
            if (verifpathbool == true) {
                TxtLog += VerifPath(path);
            }

            string LogFile = path + "log.txt";

            if (!File.Exists(LogFile))
            {
                // Create a file to write to.
                using (StreamWriter ct = File.CreateText(LogFile))
                {
                    ct.WriteLine(TxtLog);
                    ct.Close();
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(LogFile, true))
                {
                    sw.WriteLine(TxtLog);
                    sw.Close();
                }
            }
        }
    }
}