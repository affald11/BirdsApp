using DataManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Birds.Models
{
    public  class BirdCount
    {
        public int No { get; set; }

        public static int objectCount()
        {
            return GetDataFromSite.CountOjects(ConfigurationManager.AppSettings["AppJsonDataUrl"]);
        }
    }
}