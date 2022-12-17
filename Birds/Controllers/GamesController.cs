using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatABird.Models;
using DataManager;
using System.Xml.Linq;
using System.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Birds.Views.Games
{
    public class GamesController : Controller
    {
        private List<BirdModel>_BirdsGameList = new List<BirdModel>();
        private string _data;
        private List<BirdModel> _tmpList = new List<BirdModel>();
        private string _JsonUrl = ConfigurationManager.AppSettings["AppJsonDataUrl"];

        // GET: Games
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> MemoryGame(string level = "easypeasy", string cheatMode = "no")
        {
            // _data= GetDataFromSite.ReadDataStringFromSite(_JsonUrl);
            await JsonUtil.GetStringFromWeb();
            _data = JsonUtil.JsonData;

            Random rnd = new Random();
            int birdsRecordsNo = 0;
            int recordCounter = 0;
            ViewData["CheatMode"] = cheatMode;
            ViewData["level"] = level;
            _BirdsGameList = JsonConvert.DeserializeObject<List<BirdModel>>(_data).OrderBy(a=> rnd.Next()).ToList();
            //DeserializeJson(_JsonUrl);
            switch (level)
            {
                case "easypeasy":
                    birdsRecordsNo = _BirdsGameList.Count / 5;
                    break;
                case "notsoeasy":
                    birdsRecordsNo = _BirdsGameList.Count / 3;
                    break;
                case "hardy":
                    birdsRecordsNo = _BirdsGameList.Count / 2;
                    break;
                case "impossible":
                    birdsRecordsNo = _BirdsGameList.Count;
                    break;
            }

            foreach (var itm in _BirdsGameList)
            {

                BirdModel bird = new BirdModel();
                bird.Name_dk = itm.Name_dk;
                bird.Id = 0 - itm.Id;
                bird.Name_en = itm.Name_en;
                bird.Latin_name = itm.Latin_name;
                bird.Name_reader_dk = itm.Name_reader_dk;
                bird.Name_reader_en = itm.Name_reader_en;
                bird.Imageurl = itm.Imageurl;
                bird.Birdsong = itm.Birdsong;
                bird.LatinAudio = itm.LatinAudio;
                _tmpList.Add(itm);
                _tmpList.Add(bird);
                recordCounter++;
                if (recordCounter == birdsRecordsNo) { break; }
            }
            return View(_tmpList.OrderBy(a => rnd.Next()).ToList());
        }
    }
}