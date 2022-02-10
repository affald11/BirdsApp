using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatABird.Models
{
    public class BirdModel
    {
        public int Id { get; set; }
        public string Name_dk { get; set; }
        public string Name_en { get; set; }
        public string Imageurl { get; set; }
        public string Name_reader_dk { get; set; }
        public string Name_reader_en { get; set; }
        public string Birdsong { get; set; }
        public string Latin_name { get; set; }
        public string LatinAudio { get; set; }
    }
}