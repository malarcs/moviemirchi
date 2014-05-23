﻿using DataStoreLib.Constants;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class ArtistMoviesController : BaseController
    {

        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/ArtistMovies?q=artist-name&page={default 30}
        protected override string ProcessRequest()
        {
            int resultLimit = 30;
            string artistName = string.Empty;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["page"]))
                {
                    int.TryParse(qpParams["page"].ToString(), out resultLimit);
                }

                if (!string.IsNullOrEmpty(qpParams["name"]))
                {
                    artistName = qpParams["name"];
                }
            }

            try
            {
                var tableMgr = new TableManager();
                var moviesByName = tableMgr.GetArtistMovies(artistName);
                List<MovieEntity> movies = moviesByName.Take(resultLimit).ToList();
                return jsonSerializer.Value.Serialize(movies);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_ARTIST_MOVIES, ActualError = ex.Message });
            }
        }
    }
}