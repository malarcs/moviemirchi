﻿using Crawler;
using DataStoreLib.BlobStorage;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        #region Set connection string
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        public ActionResult Index()
        {
            /* XMLMovieProperties testMovie = new XMLMovieProperties();
             testMovie.MovieName = "Lagaan 3";
             testMovie.MovieLink = "http://www.c-sharpcorner.com/UploadFile/";
             testMovie.Year = 2001;
             testMovie.Month = "April";

             List<XMLReivewProperties> testReivew = new List<XMLReivewProperties> { 
                 new XMLReivewProperties() { Name = "Hidustan Times", Link = "http://www.c-sharpcorner.com/UploadFile/" } ,
                 new XMLReivewProperties() { Name = "Film fare", Link = "http://www.c-sharpcorner.com/UploadFile/" },
                 new XMLReivewProperties() { Name = "Bollywood Hungama", Link = "http://www.c-sharpcorner.com/UploadFile/" }
             };

             testMovie.Reviews = testReivew;

             var filePath = @"D:\GitHub-SVN\moviemirchi\MvcWebRole2\Filters";

             new GenerateXMLFile().CreatingFile(filePath, testMovie);*/

            return View();
        }

        #region Artist view
        public ActionResult Artists()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateArtists(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ArtistEntity movie = json.Deserialize(data, typeof(ArtistEntity)) as ArtistEntity;

                if (movie != null)
                {
                    string uniqueName = movie.ArtistName.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();

                    var tableMgr = new TableManager();

                    ArtistEntity entity = new ArtistEntity();

                    entity.RowKey = entity.ArtistId = movie.ArtistId;
                    entity.ArtistName = movie.ArtistName;
                    entity.Bio = movie.Bio;
                    entity.Posters = movie.Posters;
                    entity.Born = movie.Born;
                    entity.MovieList = movie.MovieList;
                    entity.UniqueName = movie.UniqueName;
                    entity.MyScore = movie.MyScore;
                    entity.JsonString = movie.JsonString;
                    entity.Popularity = movie.Popularity;

                    tableMgr.UpdateArtistById(entity);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Critics
        public ActionResult Critics()
        {
            return View();
        }
        #endregion

        #region Crawler
        [HttpGet]
        public ActionResult Crawler()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateXMLFile(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                data = Server.UrlDecode(data);
                JavaScriptSerializer json = new JavaScriptSerializer();
                XMLMovieProperties movieProps = json.Deserialize(data, typeof(XMLMovieProperties)) as XMLMovieProperties;

                if (movieProps != null)
                {
                    XMLMovieProperties crawlMovieEntity = new XMLMovieProperties();

                    crawlMovieEntity.MovieName = movieProps.MovieName;
                    crawlMovieEntity.MovieLink = movieProps.MovieLink;
                    crawlMovieEntity.Year = Convert.ToInt32(movieProps.Month.Split(new char[] { ' ' })[1]);
                    crawlMovieEntity.Month = movieProps.Month.Split(new char[] { ' ' })[0];
                    crawlMovieEntity.Reviews = movieProps.Reviews;

                    string xmlFileContent = new GenerateXMLFile().CreatingFile(crawlMovieEntity);

                    //new AccountController().CrawlfromXML(xmlFileContent);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", ActualError = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateXMLFileAndCrawl(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                data = Server.UrlDecode(data);
                JavaScriptSerializer json = new JavaScriptSerializer();
                XMLMovieProperties movieProps = json.Deserialize(data, typeof(XMLMovieProperties)) as XMLMovieProperties;

                if (movieProps != null)
                {
                    XMLMovieProperties crawlMovieEntity = new XMLMovieProperties();

                    crawlMovieEntity.MovieName = movieProps.MovieName;
                    crawlMovieEntity.MovieLink = movieProps.MovieLink;
                    crawlMovieEntity.Year = Convert.ToInt32(movieProps.Month.Split(new char[] { ' ' })[1]);
                    crawlMovieEntity.Month = movieProps.Month.Split(new char[] { ' ' })[0];
                    crawlMovieEntity.Reviews = movieProps.Reviews;

                    string xmlFileContent = new GenerateXMLFile().CreatingFile(crawlMovieEntity);                   
                    new AccountController().CrawlfromXML(xmlFileContent);
                    //new AccountController().Crawl();

                    // cral artits 
                    //new AccountController().GetArtists();
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", ActualError = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region New section
        [HttpGet]
        public ActionResult News()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteNews(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<string> newsIds = json.Deserialize(data, typeof(List<string>)) as List<string>;

                if (newsIds != null)
                {
                    TableManager tabmgr = new TableManager();
                    tabmgr.DeleteNewsItemById(newsIds);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", Message = "Selected news deleted successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetActiveNews(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<string> newsIds = json.Deserialize(data, typeof(List<string>)) as List<string>;

                if (newsIds != null)
                {
                    TableManager tabmgr = new TableManager();
                    var newsEntity = tabmgr.GetNewsById(newsIds);

                    if (newsEntity != null)
                    {
                        List<NewsEntity> updatedList = new List<NewsEntity>();

                        foreach (NewsEntity news in newsEntity.Values)
                        {
                            news.IsActive = true;
                            updatedList.Add(news);
                        }

                        tabmgr.UpdateNewsById(updatedList);
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", Message = "Selected news updated successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrawlNews(string data)
        {
            try
            {
                BlobStorageService _blobStorageService = new BlobStorageService();

                string newsXmlFileBlobPath = _blobStorageService.GetSinglFile(BlobStorageService.Blob_XMLFileContainer, "News.xml");

                new AccountController().GetNews(newsXmlFileBlobPath);

                return Json(new { Status = "Ok", Message = "successfully crawl news." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Message = "Error occured.", ActualMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Tweeter section
        [HttpGet]
        public ActionResult Twitter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteTwitt(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<string> twittIds = json.Deserialize(data, typeof(List<string>)) as List<string>;

                if (twittIds != null)
                {
                    TableManager tabmgr = new TableManager();
                    tabmgr.DeleteTwitterItemById(twittIds);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", Message = "Selected news deleted successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetActiveTwitt(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data) || data == null)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<string> twittIds = json.Deserialize(data, typeof(List<string>)) as List<string>;

                if (twittIds != null)
                {
                    TableManager tabmgr = new TableManager();

                    List<TwitterEntity> updatedList = new List<TwitterEntity>();
                    foreach (string twittId in twittIds)
                    {
                        var newsEntity = tabmgr.GetTweetById(twittId);

                        if (newsEntity != null)
                        {
                            foreach (TwitterEntity news in newsEntity.Values)
                            {
                                news.IsActive = true;
                                updatedList.Add(news);
                            }
                        }
                    }

                    tabmgr.UpdateTweetById(updatedList);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", Message = "Selected news updated successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrawlTwitts(string data)
        {
            try
            {
                BlobStorageService _blobStorageService = new BlobStorageService();

                string twitXmlBlobFilePath = _blobStorageService.GetSinglFile(BlobStorageService.Blob_XMLFileContainer, "Twitter.xml");

                new AccountController().GetTweets(twitXmlBlobFilePath);

                return Json(new { Status = "Ok", Message = "Successfully crawl twitts." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Message = "Error occured.", ActualMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
