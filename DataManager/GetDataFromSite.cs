using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public static class GetDataFromSite
    {
        private static char[] LeterExchageTable = new char[]{'!','J','#','0','$','7','%','!','&','K','(','4',')','8','*','e','/','E','?','y','@','æ','[',
                                                             '£','\\','G',']','*','{','f','}','T','£','å','¤','[','+','C','<','+','=','B','>','¤','§','g',
                                                             '0','F','1','v','2','o','3','O','4','n','5','P','6','}','7','I','8','R','9','l','A','6','a',
                                                             'h','B','M','b','%','C','/','c','A','D','Æ','d','N','E','r','e','S','F','i','f','t','G','s',
                                                             '§','Y','H','2','h','\\','I','@','i','b','J','c','j','H','K','&','k','q','L','z','l','a','M',
                                                             'p','m','?','N','1','n','Q','O','$','o','(','P','U','p',')','Q','9','q','g','R','5','r','j',
                                                             'S','k','s','Ø','T','D','t','3','U','d','u','V','V','ø','v','u','Y','#','y','<','Z','m','z',
                                                             'L','Æ','Å','æ','{','Ø','=','ø','>','Å','Z','å',']',' ','|'};

        static private List<char> specletters = new List<char>() { '@', '!', '§', '#', '$', '&', '/', '(', ')', '=', '?', '+',
                                                                   '{', '[', ']', '}', '*', '%','\\', '¤','£','>','<'};

        static private List<char> excludedLetters = new List<char>() { '\"', '\'', ',', '½', '~', '^', '¨', ';', '~', '.', '\'' };
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

        public static string ReadStringFromDb(string conStr)
        {
            string secr = Decr();
            //string sqlStr = @"SELECT[id],[name_dk],[name_en],[imageurl],[name_reader_dk],[name_reader_en],[birdsong],[latin_name],replace('https://grynberg.dk/SoundTracks/lt/' + Latin_name + '.mp3',' ','_') AS LatinAudio FROM [grynberg_dk_db_test].[dbo].[Birds] for json path";
            ////string sqlStr = @"SELECT[id],[name_dk],[name_en],[imageurl],[name_reader_dk],[name_reader_en],[birdsong],[LatinAudio] FROM [web_view].[bird_v] for json path";
            //SqlConnection dbConn = new SqlConnection(secr);
            ////SqlConnection dbConn = new SqlConnection(conStr);
            //string jn = "";
            //dbConn.Open();
            //SqlCommand cmd = new SqlCommand(sqlStr, dbConn);
            //SqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    jn = jn + reader.GetString(0);
            //}
            //reader.Close();
            //dbConn.Close();
            //// return jn;
            return secr;
        }

        private static string Decr()
        {
            string HashStr = "%4¤%8Kq<|";
            
            string secr = ConfigurationManager.AppSettings["licenceType"].ToString();
            byte[] data = Convert.FromBase64String(secr);

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashStr));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    string retVal = UTF8Encoding.UTF8.GetString(result);
                    retVal = CharArrToStr(MasterArrExchange(Masterarrey(retVal), 1), true);
                    retVal = "Server = mssql8.unoeuro.com; Database = ; User Id =; Password = )" + retVal + " ;";
                    return retVal;
                }
            }

        }
        private static char[,] MasterArrExchange(char[,] masterArray, int changeType)
        {

            for (int j = 0; j < masterArray.GetLongLength(1); j++)
            {
                for (int i = 0; i < masterArray.GetLongLength(0); i++)
                {
                    masterArray[i, j] = ExLetter(masterArray[i, j], changeType);
                }
            }
            return masterArray;
        }
        private static char ExLetter(char c, int x = 0)
        {
            char retVal = c;

            for (int i = 0; i < LeterExchageTable.Length; i++)
            {
                if ((i % 2 == 0) && (LeterExchageTable[i] == c) && (x == 0))
                {
                    retVal = LeterExchageTable[i + 1];
                }
                if ((i % 2 == 1) && (LeterExchageTable[i] == c) && (x != 0))
                {
                    retVal = LeterExchageTable[i - 1];
                }
            }
            return retVal;
        }

        public static string CharArrToStr(char[,] arr, bool removeSpec = false)
        {
            string retVal = "";
            char c;
            string tmpStr = "";

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    c = arr[i, j];
                    tmpStr = c.ToString();
                    if (removeSpec && excludedLetters.Contains(c))
                    {
                        tmpStr = "";
                    }
                    retVal = retVal + tmpStr;
                }
            }
            return retVal;
        }
        private static char[,] Masterarrey(string masterKey, bool addPeddings = true)
        {
            int cols = 4;
            int rows = 0;
            rows = (masterKey.Length / 4) + 1;
            char[,] retVal = new char[cols, rows];
            char[] master = masterKey.ToCharArray();
            int idx = 0;

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (addPeddings)
                    { retVal[i, j] = excludedLetters[j % excludedLetters.Count]; }
                    if (idx <= masterKey.Length - 1)
                    {
                        retVal[i, j] = master[idx];
                        idx++;
                    }
                }
            }
            return retVal;
        }
    }
}
   
