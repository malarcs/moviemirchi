﻿using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Storage
{
    public interface IStore
    {
        #region Login
        IDictionary<string, UserEntity> GetAllUser();
        IDictionary<string, UserEntity> GetUsersById(IEnumerable<string> userId);
        IDictionary<string, UserEntity> GetUsersByName(string userName);
        IDictionary<UserEntity, bool> UpdateUsersById(IEnumerable<UserEntity> user);
        #endregion

        #region Movie
        IDictionary<string, MovieEntity> GetAllMovies();
        IDictionary<string, MovieEntity> GetMoviesById(IEnumerable<string> id);
        IDictionary<string, MovieEntity> GetMoviesByUniqueName(string name);
        IDictionary<MovieEntity, bool> UpdateMoviesById(IEnumerable<MovieEntity> movies);

        IDictionary<string, MovieEntity> GetArtistMovies();

        IDictionary<string, MovieEntity> GetGenrewiseMovies();
        #endregion

        #region Review
        IDictionary<string, ReviewEntity> GetReviewsDetailById(string reviewerId, string movieId);
        IDictionary<string, ReviewEntity> GetReviewsByMovieId(string movieId);
        IDictionary<string, ReviewEntity> GetReviewsById(IEnumerable<string> id);
        IDictionary<string, ReviewEntity> GetReviewsByReviewer(string reviewerName);
        IDictionary<ReviewEntity, bool> UpdateReviewsById(IEnumerable<ReviewEntity> reviews);
        IDictionary<ReviewEntity, bool> UpdateReviewesByReviewerId(IEnumerable<ReviewEntity> reviewer);
        #endregion

        #region Reviewer
        IDictionary<string, ReviewerEntity> GetAllReviewer();
        IDictionary<string, ReviewerEntity> GetReviewersById(IEnumerable<string> id);
        IDictionary<ReviewerEntity, bool> UpdateReviewers(IEnumerable<ReviewerEntity> reviewer);
        #endregion

        #region Affiliation
        IDictionary<string, AffilationEntity> GetAllAffilation();
        IDictionary<string, AffilationEntity> GetAffilationsByid(IEnumerable<string> id);
        IDictionary<AffilationEntity, bool> UpdateAffilationsById(IEnumerable<AffilationEntity> affilation);
        #endregion

        #region User favorites tables functions
        IDictionary<string, UserFavoriteEntity> GetUserFavoritesById(IEnumerable<string> ids);
        IDictionary<UserFavoriteEntity, bool> UpdateUserFavoritesById(IEnumerable<Models.UserFavoriteEntity> userFavorites);
        #endregion

        #region Popular on movie mirchi table
        IDictionary<string, PopularOnMovieMirchiEntity> GetPopularOnMovieMirchisById(IEnumerable<string> id);
        IDictionary<PopularOnMovieMirchiEntity, bool> UpdatePopularOnMovieMirchisById(IEnumerable<PopularOnMovieMirchiEntity> popularOnMovieMirchi);
        #endregion

        #region Twitter table
        IDictionary<string, TwitterEntity> GetTweetById(string id);
        IDictionary<TwitterEntity, bool> UpdateTweetById(IEnumerable<TwitterEntity> tweets);
        #endregion

        #region News table
        IDictionary<string, NewsEntity> GetNewsItems();
        IDictionary<NewsEntity, bool> UpdateNewsItemById(IEnumerable<NewsEntity> newsItems);
        #endregion
    }

    public static class IStoreHelpers
    {
        #region Login

        /// <summary>
        /// Return the List of UserName
        /// </summary>
        /// <param name="store"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static List<UserEntity> SearchUser(this IStore store, string searchText)
        {
            var retList = store.GetAllUser();
            Debug.Assert(retList.Count == 1);

            List<UserEntity> users = new List<UserEntity>();

            foreach (var user in retList.Values)
            {
                if (user.UserName.Contains(searchText.ToLower()))
                {
                    users.Add(user);
                }
            }

            users.Equals(searchText);

            //currentSongs.Sort();
            return users;
        }

        /// <summary>
        /// Return particular User By Id
        /// </summary>
        /// <param name="store"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserEntity GetUserById(this IStore store, string userId)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(userId));
            var list = new List<string> { userId };
            var retList = store.GetUsersById(list);

            return retList[retList.Keys.FirstOrDefault()];
        }

        /// <summary>
        /// Return the user if key(Name) found  
        /// </summary>
        /// <param name="store"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static UserEntity GetUserByName(this IStore store, string userName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(userName));

            var retList = store.GetUsersByName(userName);

            Debug.Assert(retList.Count == 1);
            if (retList.Count > 0)
                return retList[retList.Keys.FirstOrDefault()];
            else
                return null;
        }

        /// <summary>
        /// Return update the user if user is found
        /// </summary>
        /// <param name="store"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool UpdateUserById(this IStore store, UserEntity user)
        {
            Debug.Assert(user != null);
            var list = new List<UserEntity> { user };
            var retList = store.UpdateUsersById(list);

            Debug.Assert(retList.Count == 1);
            if (retList.Count > 0)
                return retList[retList.Keys.FirstOrDefault()];
            else
                return false;
        }
        #endregion

        #region Movie

        /// <summary>
        /// Return the movie By MovieId
        /// </summary>
        /// <param name="store"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MovieEntity GetMovieById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new string[] { id };
            var retList = store.GetMoviesById(list);

            Debug.Assert(retList.Count == 1);
            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : null;
        }

        /// <summary>
        /// Return the  Movie name
        /// </summary>
        /// <param name="store"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MovieEntity GetMovieByUniqueName(this IStore store, string name)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(name));
            var retList = store.GetMoviesByUniqueName(name);

            Debug.Assert(retList.Count == 1);
            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : null;
        }
        /// <summary>
        /// Update the movie detail by its id 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public static bool UpdateMovieById(this IStore store, MovieEntity movie)
        {
            Debug.Assert(movie != null);
            var list = new List<MovieEntity> { movie };
            var retList = store.UpdateMoviesById(list);

            Debug.Assert(retList.Count == 1);
            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : false;
        }
        /// <summary>
        /// Return the runing movie  
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static IEnumerable<MovieEntity> GetCurrentMovies(this IStore store)
        {
            var retList = store.GetAllMovies();
            ////TODO Do we need to add month logic? How do we filter to current?
            return retList.Values;

            //// TODO: Clean the comments
            //////Debug.Assert(retList.Count == 1);

            ////List<MovieEntity> currentMovies = new List<MovieEntity>();

            ////foreach (var currentMovie in retList.Values)
            ////{

            ////    currentMovies.Add(currentMovie);
            ////    /*
            ////    string currentMonth = DateTime.Now.ToString("MMMM");
            ////    string year = DateTime.Now.Year.ToString();


            ////    if (currentMovie.Month == currentMonth && currentMovie.Year == year)
            ////    {
            ////        currentMovies.Add(currentMovie);
            ////    }*/
            ////}

            ////return currentMovies;
        }

        public static IEnumerable<MovieEntity> GetUpcomingMovies(this IStore store)
        {
            var retList = store.GetAllMovies();
            return
                retList.Values
                // Need to update Trailer with new column - State = Released, Upcoming, Production, PreProduction, Script, Planning etc. 
                    .Where(movie => string.IsNullOrEmpty(movie.Trailers) || movie.Trailers.ToLower() == "upcoming");

            ////TODO: Clean the comments
            ////List<MovieEntity> upcomingMovies = new List<MovieEntity>();

            ////foreach (var upcomingMovie in retList.Values)
            ////{
            ////    // Need to update Trailer with new column - State = Released, Upcoming, Production, PreProduction, Script, Planning etc. 
            ////    if (string.IsNullOrEmpty(upcomingMovie.Trailers) || upcomingMovie.Trailers.ToLower() == "upcoming")
            ////        upcomingMovies.Add(upcomingMovie);
            ////    else
            ////    {
            ////        // Temp = Assuming Month column will be blank for future releases or movies with future release date shall be returned by this block
            ////        // however need to take a call - if we shall rely on date!

            ////    }
            ////}

            ////return upcomingMovies;
        }

        public static IEnumerable<MovieEntity> GetArtistMovies(this IStore store, string artistName)
        {
            var retList = store.GetAllMovies();
            return retList.Values.Where(movie => movie.Casts.Contains(artistName));
        }

        public static IEnumerable<MovieEntity> GetGenrewiseMovies(this IStore store, string genre)
        {
            var retList = store.GetAllMovies();
            return retList.Values.Where(movie => movie.Genre.Contains(genre));
        }

        /// <summary>
        /// Return the movie name in sorted order
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static IEnumerable<MovieEntity> GetSortedMoviesByName(this IStore store)
        {
            var retList = store.GetAllMovies();
            return
                retList.Values
                    .OrderBy(movie => movie.Name);

            ////TODO: Clean the comments
            //////Debug.Assert(retList.Count == 1);

            ////List<MovieEntity> currentMovies = new List<MovieEntity>();

            ////if (retList != null && retList.Values != null)
            ////{
            ////    currentMovies = (List<MovieEntity>)retList.Values.OrderBy(m => m.Name).ToList();
            ////}

            ////return currentMovies;
        }


        #region Search
        public static IEnumerable<MovieEntity> SearchSongs(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            searchText = searchText.ToLower();
            return
                retList.Values
                    .Where(movie => movie.Songs.ToLower().Contains(searchText));


            ////TODO: What are you returning song that matches the searchtext or movie which has atleast one song that matches the searchtext?
            //// I guess it is the latter, so you might want to rename the method by GetMovieWithSong
            //// What is movie.Songs? I would imagine this as a List<string> but it is not. So, I dont know what to expect here.

            ////TODO: Clean comments
            ////List<MovieEntity> currentSongs = new List<MovieEntity>();

            ////foreach (var currentSong in retList.Values)
            ////{
            ////    if (currentSong.Songs.ToLower().Contains(searchText.ToLower()))
            ////    {
            ////        currentSongs.Add(currentSong);
            ////    }
            ////}

            ////return currentSongs;
        }
        public static IEnumerable<MovieEntity> SearchMovies(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            searchText = searchText.ToLower();
            return
                retList.Values
                    .Where(movie => movie.Name.ToLower().Contains(searchText));

            ////TODO: Clean comments
            ////List<MovieEntity> currentMovies = new List<MovieEntity>();

            ////foreach (var currentMovie in retList.Values)
            ////{
            ////    if (currentMovie.Name.ToLower().Contains(searchText.ToLower()))
            ////    {
            ////        currentMovies.Add(currentMovie);
            ////    }
            ////}

            ////return currentMovies;
        }
        public static IEnumerable<MovieEntity> SearchMoviesByActor(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            searchText = searchText.ToLower();
            return
                retList.Values
                    .Where(movie => movie.Casts.ToString().ToLower().Contains(searchText));

            ////TODO: Clean comments
            ////List<MovieEntity> actors = new List<MovieEntity>();

            ////foreach (var actor in retList.Values)
            ////{
            ////    if (actor.Casts.ToString().ToLower().Contains(searchText.ToLower()))
            ////    {
            ////        actors.Add(actor);
            ////    }
            ////}

            ////return actors;
        }


        public static IEnumerable<MovieEntity> SearchTitle(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            searchText = searchText.ToLower();
            return
                retList.Values
                    .Where(movie => movie.Name.ToLower().Contains(searchText));

            //// TODO: How is different from SearchMovies?
            //// Do you want to add a TODO in SearchMovies to search on other features along with Title like cast, songs, etc?

            ////TODO: Clean comments
            ////List<MovieEntity> titles = new List<MovieEntity>();

            ////foreach (var title in retList.Values)
            ////{
            ////    if (title.Name.ToLower().Contains(searchText.ToLower()))
            ////    {
            ////        titles.Add(title);
            ////    }
            ////}

            ////return titles;
        }

        public static IEnumerable<MovieEntity> SearchTrailer(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            searchText = searchText.ToLower();
            return
                retList.Values
                    .Where(movie => movie.Trailers.ToLower().Contains(searchText));

            ////TODO: Clean comments
            ////List<MovieEntity> traileres = new List<MovieEntity>();

            ////foreach (var trailer in retList.Values)
            ////{
            ////    if (trailer.Trailers.ToLower().Contains(searchText.ToLower()))
            ////    {
            ////        traileres.Add(trailer);
            ////    }
            ////}

            ////return traileres;
        }

        public static IEnumerable<MovieEntity> SearchCharacter(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            searchText = searchText.ToLower();
            return
                retList.Values
                    .Where(movie => movie.Casts.ToLower().Contains(searchText));

            ////TODO: Clean comments
            ////List<MovieEntity> characters = new List<MovieEntity>();

            ////foreach (var character in retList.Values)
            ////{
            ////    if (character.Casts.ToLower().Contains(searchText.ToLower()))
            ////    {
            ////        characters.Add(character);
            ////    }
            ////}

            ////return characters;
        }

        //public static List<MovieEntity> 

        #endregion

        #endregion of Movie

        #region Review
        /// <summary>
        /// Update the review by its id
        /// </summary>
        /// <param name="store"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        public static bool UpdateReviewById(this IStore store, ReviewEntity review)
        {
            Debug.Assert(review != null);
            var list = new ReviewEntity[] { review };
            var retList = store.UpdateReviewsById(list);

            Debug.Assert(retList.Count == 1);
            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : false;
        }
        /// <summary>
        /// Return the Review by the Id
        /// </summary>
        /// <param name="store"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ReviewEntity GetReviewById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new string[] { id };
            var retList = store.GetReviewsById(list);

            Debug.Assert(retList.Count == 1);
            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : null;
        }
        /// <summary>
        /// Return the review detail by its id
        /// </summary>
        /// <param name="store"></param>
        /// <param name="reviewerId"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public static ReviewEntity GetReviewDetailById(this IStore store, string reviewerId, string movieId)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(reviewerId));
            var retList = store.GetReviewsDetailById(reviewerId, movieId);

            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : null;
        }

        #endregion

        #region Reviewer
        /// <summary>
        /// Update the reviewer by reviewerid
        /// </summary>
        /// <param name="store"></param>
        /// <param name="reviewer"></param>
        /// <returns></returns>
        public static bool UpdateReviewerById(this IStore store, ReviewerEntity reviewer)
        {
            Debug.Assert(reviewer != null);
            var list = new List<ReviewerEntity> { reviewer };
            var retList = store.UpdateReviewers(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        /// <summary>
        /// Return the Reviewer name in sorted order
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static List<ReviewerEntity> GetSortedReviewerByName(this IStore store)
        {
            var retList = store.GetAllReviewer();

            //Debug.Assert(retList.Count == 1);

            List<ReviewerEntity> reviewers = new List<ReviewerEntity>();

            if (retList != null && retList.Values != null)
            {
                reviewers = (List<ReviewerEntity>)retList.Values.OrderBy(m => m.ReviewerName).ToList();
            }

            return reviewers;
        }
        /// <summary>
        /// Return the reviewer by Reviewerid
        /// </summary>
        /// <param name="store"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ReviewerEntity GetReviewerById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetReviewersById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        #endregion

        #region Affiliation
        /// <summary>
        /// Update the affiliation by Affiliation id
        /// </summary>
        /// <param name="store"></param>
        /// <param name="affilation"></param>
        /// <returns></returns>
        public static bool UpdateAffilationById(this IStore store, AffilationEntity affilation)
        {
            Debug.Assert(affilation != null);
            var list = new List<AffilationEntity> { affilation };
            var retList = store.UpdateAffilationsById(list);


            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        /// <summary>
        /// Return the list of Affiliation name in the sorted order
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static List<AffilationEntity> GetSortedAffilationByName(this IStore store)
        {
            var retList = store.GetAllAffilation();

            //Debug.Assert(retList.Count == 1);

            List<AffilationEntity> allAffilation = new List<AffilationEntity>();

            if (retList != null && retList.Values != null)
            {
                allAffilation = (List<AffilationEntity>)retList.Values.OrderBy(m => m.AffilationName).ToList();
            }

            return allAffilation;
        }
        /// <summary>
        /// Return the Affiliation by affiliation id
        /// </summary>
        /// <param name="store"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AffilationEntity GetAffilationById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetAffilationsByid(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        #endregion

        #region User favorites tables functions
        public static UserFavoriteEntity GetUserFavoriteById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetUserFavoritesById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static bool UpdateUserFavoriteById(this IStore store, UserFavoriteEntity userFavorite)
        {
            Debug.Assert(userFavorite != null);
            var list = new List<UserFavoriteEntity> { userFavorite };
            var retList = store.UpdateUserFavoritesById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        #endregion

        #region Popular on Movie mirchi table
        public static PopularOnMovieMirchiEntity GetPopularOnMovieMirchiById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetPopularOnMovieMirchisById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static bool UpdatePopularOnMovieMirchiId(this IStore store, PopularOnMovieMirchiEntity popularOnMovieMirchi)
        {
            Debug.Assert(popularOnMovieMirchi != null);
            var list = new List<PopularOnMovieMirchiEntity> { popularOnMovieMirchi };
            var retList = store.UpdatePopularOnMovieMirchisById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        #endregion

        #region Twitter
        public static bool IsTweetExist(this IStore store, string tweetId)
        {
            var retList = store.GetTweetById(tweetId);

            //Debug.Assert(retList.Count == 1);

            List<TwitterEntity> tweets = new List<TwitterEntity>();

            if (retList != null && retList.Values != null)
            {
                tweets = (List<TwitterEntity>)retList.Values.OrderBy(m => m.Timestamp).ToList();
            }

            return tweets.Count > 0 ? true : false;
        }

        public static bool UpdateTweetById(this IStore store, TwitterEntity tweet)
        {
            Debug.Assert(tweet != null);
            var list = new List<TwitterEntity> { tweet };
            var retList = store.UpdateTweetById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
        #endregion

        #region News
        public static bool UpdateNewsById(this IStore store, IEnumerable<NewsEntity> news)
        {
            Debug.Assert(news != null);
            var retList = store.UpdateNewsItemById(news);

            Debug.Assert(retList.Count == 1);
            var key = retList.Keys.FirstOrDefault();
            return (key != null) ? retList[key] : false;
        }

        public static IEnumerable<NewsEntity> GetNewsItems(this IStore store)
        {
            var retList = store.GetNewsItems();

            ////TODO: Uncomment
            //Debug.Assert(retList.Count == 1);

            return
                retList.Values
                    .OrderByDescending(m => m.Timestamp);

            ////TODO: Clean comments
            ////List<NewsEntity> news = new List<NewsEntity>();

            ////if (retList != null && retList.Values != null)
            ////{
            ////    news = (List<NewsEntity>)retList.Values.OrderBy(m => m.Timestamp).ToList();
            ////}

            ////return news;
        }
        #endregion

    }

}
