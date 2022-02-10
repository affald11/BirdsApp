using DataManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatABird.Models;

namespace Birds.Controllers
{
    public class BirdController : Controller
    {
        private List<BirdModel> _BirdsList;
        private string _JsonUrl;
        private int[] rendomBirds;
        public List<BirdModel> tmpList = new List<BirdModel>();
        public string test;
        // GET: Bird

        public ActionResult BirdsList()
        {
            Initialize();
            return View(_BirdsList.OrderBy(x => x.Name_dk).ToList());
        }

        public ActionResult Index()
        {
            List<BirdModel> tmpList = new List<BirdModel>();
            Initialize();
            rendomBirds = GetDataFromSite.GetRendomNumbers(0, _BirdsList.Count, 3);
            int chosenOne = GetDataFromSite.GetRendomNumbers(0, 3, 1)[0];

            for (int i = 0; i < 3; i++)
            {
                tmpList.Add(_BirdsList[rendomBirds[i]]);
                if (i != chosenOne)
                {
                    tmpList[i].Birdsong = "NIX";
                }
            }
           //Return View(Tuple.Create(GetDataFromSite.ReadStringFromDb("X"), tmpList));
            return View( tmpList);
        }

        private void Initialize()
        {
             _JsonUrl = ConfigurationManager.AppSettings["AppJsonDataUrl"];

            //_JsonUrl = GetDataFromSite.ReadStringFromDb(ConfigurationManager.ConnectionStrings["BirdConn"].ToString());
            string connStr = ConfigurationManager.ConnectionStrings["BirdConn"].ToString();
            string BirdData= GetDataFromSite.ReadDataStringFromSite(_JsonUrl);
            //DeserializeJson(_JsonUrl);
            DeserializeJson(BirdData);

        }

        private void DeserializeJson(string birdData)
        {
            _BirdsList = JsonConvert.DeserializeObject<List<BirdModel>>(birdData);
        }
    }
}