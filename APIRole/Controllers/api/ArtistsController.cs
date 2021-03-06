﻿
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns list of all the movies based on type. Type could be “current”, “all” “current” movies are having release date threshold of 1 month (configurable) 
    /// “all” movies type will return top 100 movies released recently.
    /// </summary>
    public class ArtistsController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        private static object _object = new object();


        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            int resultLimit = 15;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var tableMgr = new TableManager();
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                string artistsInitials = string.Empty;

                if (!string.IsNullOrEmpty(qpParams["q"]))
                {
                    artistsInitials = qpParams["q"].ToString().ToLower();
                }

                var artistsByName = tableMgr.GetAllArtist(artistsInitials.ToLower()).Take(resultLimit).ToList().OrderBy(a => a.ArtistName);
                return jsonSerializer.Value.Serialize(artistsByName);
            }

            return string.Empty;
        }
    }
}
