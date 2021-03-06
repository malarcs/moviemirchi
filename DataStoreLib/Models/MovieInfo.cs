﻿using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataStoreLib.Models
{
    public class MovieInfo
    {
        public MovieEntity Movie { get; set; }
        public IEnumerable<ReviewEntity> MovieReviews { get; set; }
        public IEnumerable<MovieEntity> MoviesList { get; set; }
        public string movieId { get; set; }
        public string name { get; set; }
        public PosterInfo poster { get; set; }
        public Rating rating { get; set; }
        public Info info { get; set; }
        public IEnumerable<Review> reviews { get; set; }
    }

    public class PosterInfo
    {
        public int height { get; set; }
        public int width { get; set; }
        public string url { get; set; }

        public string source { get; set; }
    }

    public class Rating
    {
        public int system { get; set; }
        public int critic { get; set; }
        public string hot { get; set; }
    }

    public class Info
    {
        public string synopsis { get; set; }
        public List<Cast> cast { get; set; }
        public Stats stats { get; set; }

        public Multimedia multimedia { get; set; }
    }

    public class Cast
    {
        public string name { get; set; }
        public string charactername { get; set; }
        public PosterInfo image { get; set; }
        public string role { get; set; }

        public string link { get; set; }
    }

    public class Stats
    {
        public string budget { get; set; }
        public string boxoffice { get; set; }
    }

    public class Multimedia
    {
        public IEnumerable<Songs> songs { get; set; }
        public IEnumerable<Songs> trailers { get; set; }
        public IEnumerable<Picture> pics { get; set; }
    }

    public class Songs
    {
        public string SongTitle { get; set; }

        public string Lyrics { get; set; }

        public string Composed { get; set; }

        public string Performer { get; set; }

        public string Recite { get; set; }

        public string Courtsey { get; set; }
    }

    public class Picture
    {
        public string caption { get; set; }
        public PosterInfo pics { get; set; }
    }

    public class Review
    {
        public string name { get; set; }
        public Rating rating { get; set; }
        public string summary { get; set; }
        public string outlink { get; set; }
    }
}