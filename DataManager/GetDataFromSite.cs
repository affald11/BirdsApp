using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public static class GetDataFromSite
    {
       
        public static int CountOjects(string file)
        {
            int cnt = 0;
            char res;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    int count;
                    Stream s = webClient.OpenRead(file);
                    byte[] b;

                    do
                    {
                        b = new byte[1];
                        count = s.Read(b, 0, 1);
                        res = (char)b[0];
                        if (res == '}')
                        {
                            cnt++;
                        }
                    } while (count > 0);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return cnt;
        }

        public static int[] GetRendomNumbers(int min, int max, int amount)
        {
            int[] retVal = new int[amount];

            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = -1;
            }

            int tmp;
            Random random = new Random();

            for (int i = 0; i < amount; i++)
            {
                do tmp = random.Next(min, max);
                while (retVal.Contains(tmp));
                retVal[i] = tmp;
            }
            return retVal;
        }

        public static Bitmap ReadImageFromSite(string urlStr)
        {
            Bitmap bmap;
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    Stream s = webClient.OpenRead(urlStr);
                    bmap = new Bitmap(s);
                    return bmap;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string ReadDataStringFromSite(string urlStr)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string resultStr = webClient.DownloadString(urlStr);
                    return resultStr;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }     
    }
    public static class JsonUtil
    {
        public static HttpClient WClient { get; set; } 

        private const string _BaseUrl = @"https://grynberg.dk";
        public static string JsonData { get; set; } = "";
        public static void InitializeWClient()
        {
            WClient = new HttpClient();
            WClient.BaseAddress = new Uri(_BaseUrl);
            WClient.DefaultRequestHeaders.Accept.Clear();
            WClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async static Task GetStringFromWeb(string url = "/data/BirdsData3.json")
        {
            InitializeWClient();
            HttpResponseMessage response = await JsonUtil.WClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] JsonByte = await response.Content.ReadAsByteArrayAsync();
                JsonData = Encoding.UTF8.GetString(JsonByte);
                response.Dispose();
            }
            else
            {
                response.Dispose();
                throw new Exception(response.ReasonPhrase);

            }
        }
    }
}
   