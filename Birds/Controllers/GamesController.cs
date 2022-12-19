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
        public async Task<ActionResult> MemoryGame(string level = "easypeasy", string cheatMode = "no",int langCombi=0 )
        {
            await JsonUtil.GetStringFromWeb();
            _data = JsonUtil.JsonData;
            Random rnd = new Random();
            int birdsRecordsNo = 0;
            int recordCounter = 0;
            ViewData["CheatMode"] = cheatMode;
            ViewData["level"] = level;
            ViewBag.Lang = langCombi;
            _BirdsGameList = JsonConvert.DeserializeObject<List<BirdModel>>(_data);
            if (!Enumerable.Range(0, 3).Contains(langCombi))
            {
                langCombi = 0;
            }
            ViewData["langCombi"] = langCombi;


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
                switch (langCombi) 
                {
                    case 0:
                        break; 
                    case 1:
                        itm.Name_en = itm.Latin_name;
                        itm.Name_reader_en = (@"https://grynberg.dk/SoundTracks/lt/" + itm.Latin_name + ".mp3").Replace(" ", "_");                       
                       
                        break; 
                    case 2:
                        itm.Name_dk = itm.Latin_name;
                        itm.Name_reader_dk = (@"https://grynberg.dk/SoundTracks/lt/" + itm.Latin_name + ".mp3").Replace(" ", "_");
                        break;
                   
                }
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