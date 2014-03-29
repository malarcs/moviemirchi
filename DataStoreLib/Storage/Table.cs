﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    public abstract class Table
    {
        protected CloudTable _table;
        protected Table(CloudTable table)
        {
            _table = table;
        }

        public virtual IDictionary<string, TEntity> GetItemsById<TEntity>(List<string> ids, string partitionKey = "") where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(ids.Count != 0);
            Debug.Assert(_table != null);

            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                partitionKey = GetParitionKey();
            }

            var operationList = new Dictionary<string, TableResult>();
            foreach (var id in ids)
            {
                operationList[id] = _table.Execute(TableOperation.Retrieve<TEntity>(partitionKey, id));
            }

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;
            foreach (var tableResult in operationList)
            {
                TEntity entity = null;
                if (tableResult.Value.HttpStatusCode != (int)HttpStatusCode.OK)
                {
                    Trace.TraceWarning("Couldn't retrieve info for id {0}", ids[iter]);
                }
                else
                {
                    entity = tableResult.Value.Result as TEntity;
                }
                returnDict.Add(ids[iter], entity);
                iter++;
            }

            return returnDict;
        }

        public virtual IDictionary<ITableEntity, bool> UpdateItemsById(List<ITableEntity> items, string partitionKey = "")
        {
            var returnDict = new Dictionary<ITableEntity, bool>();

            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                partitionKey = GetParitionKey();
            }

            var batchOp = new TableBatchOperation();
            foreach (var item in items)
            {
                //batchOp.Insert(item);
                //Replace if entity exists otherwise add new entity.
                batchOp.InsertOrReplace(item);
            }

            var tableResult = _table.ExecuteBatch(batchOp);

            foreach (var result in tableResult)
            {
                Debug.Assert((result.Result as ITableEntity) != null);
                if (result.HttpStatusCode >= 200 || result.HttpStatusCode < 300)
                {
                    returnDict[result.Result as ITableEntity] = true;
                }
                else
                {
                    returnDict[result.Result as ITableEntity] = false;
                }
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetAllItems<TEntity>() where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<MovieEntity> query = new TableQuery<MovieEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<MovieEntity> movieResults = _table.ExecuteQuery<MovieEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in movieResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.MovieId, entity);
                iter++;
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetItemsByMovieId<TEntity>(string movieId) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("MovieId", QueryComparisons.Equal, movieId));
            //TableQuery<TEntity> query = new TableQuery<TEntity>().Where(TableQuery.GenerateFilterCondition("MovieId", QueryComparisons.Equal, movieId));

            // execute query
            //IEnumerable<TEntity> reviewResults = _table.ExecuteQuery<TEntity>(query);
            IEnumerable<ReviewEntity> reviewResults = _table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetItemsByReivewer<TEntity>(string reviewerName) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);
            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("ReviewerName", QueryComparisons.Equal, reviewerName));

            IEnumerable<ReviewEntity> reviewResults = _table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetAllAffilationItems<TEntity>() where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<AffilationEntity> query = new TableQuery<AffilationEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<AffilationEntity> results = _table.ExecuteQuery<AffilationEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in results)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.AffilationId, entity);
                iter++;
            }

            return returnDict;
        }



        public virtual IDictionary<string, TEntity> GetAllReviewItems<TEntity>() where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<ReviewEntity> results = _table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in results)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }

        protected abstract string GetParitionKey();
    }





    internal class MovieTable : Table
    {
        protected MovieTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new MovieTable(table);
        }

        protected override string GetParitionKey()
        {
            return MovieEntity.PARTITION_KEY;
        }

        public IDictionary<string, TEntity> GetItemsByName<TEntity>(string name) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<MovieEntity> query = new TableQuery<MovieEntity>().Where(TableQuery.GenerateFilterCondition("UniqueName", QueryComparisons.Equal, name));

            //execute query
            IEnumerable<MovieEntity> reviewResults = _table.ExecuteQuery<MovieEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.MovieId, entity);
                iter++;
            }

            return returnDict;
        }
    }

    internal class ReviewTable : Table
    {
        protected ReviewTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ReviewTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewEntity.PARTITION_KEY;
        }

        public IDictionary<string, TEntity> GetItemsByReviewerAndMovieId<TEntity>(string reviewerId) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("ReviewerId", QueryComparisons.Equal, reviewerId));

            IEnumerable<ReviewEntity> reviewResults = _table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewerId, entity);
                iter++;
            }

            return returnDict;
        }


        public IDictionary<string, TEntity> GetReviewByMovieAndReviewId<TEntity>(string reviewerId, string movieId) where TEntity : DataStoreLib.Models.TableEntity
        {
            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.CombineFilters(
                                                                TableQuery.GenerateFilterCondition("ReviewerId", QueryComparisons.Equal, reviewerId),
                                                                TableOperators.And,
                                                                TableQuery.GenerateFilterCondition("MovieId", QueryComparisons.Equal, movieId)));
            IEnumerable<ReviewEntity> updateReviewResult = _table.ExecuteQuery<ReviewEntity>(query);
            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;
            foreach (var tableResult in updateReviewResult)
            {

                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }
    }

    internal class UserTable : Table
    {
        protected UserTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new UserTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewEntity.PARTITION_KEY;
        }


        public IDictionary<string, TEntity> GetItemsByUserName<TEntity>(string userName) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("UserName", QueryComparisons.Equal, userName));

            IEnumerable<UserEntity> loginResults = _table.ExecuteQuery<UserEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in loginResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.UserName, entity);
                iter++;
            }

            return returnDict;
        }
        /*
       
        */
        public IDictionary<string, TEntity> UserAuthentication<TEntity>(string userName, string password) where TEntity : DataStoreLib.Models.TableEntity
        {

            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.CombineFilters(
                                                                                TableQuery.GenerateFilterCondition("UserName", QueryComparisons.Equal, userName),
                                                                                TableOperators.And,
                                                                                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password)));


            IEnumerable<UserEntity> loginResults = _table.ExecuteQuery<UserEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in loginResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.UserName, entity);
                iter++;
            }

            return returnDict;
        }
    }
    internal class AffilationTable : Table
    {
        protected AffilationTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new AffilationTable(table);
        }

        protected override string GetParitionKey()
        {
            return AffilationEntity.PARTITION_KEY;
        }
    }

    internal class ReviewerTable : Table
    {
        protected ReviewerTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ReviewerTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewerEntity.PARTITION_KEY;
        }

        public virtual IDictionary<string, TEntity> GetAllReviewers<TEntity>() where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewerEntity> query = new TableQuery<ReviewerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<ReviewerEntity> movieResults = _table.ExecuteQuery<ReviewerEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in movieResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewerId, entity);
                iter++;
            }

            return returnDict;
        }
    }


    internal class UserFavoriteTable : Table
    {
        protected UserFavoriteTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new UserFavoriteTable(table);
        }

        protected override string GetParitionKey()
        {
            return UserFavoriteEntity.PARTITION_KEY;
        }

        public UserFavoriteEntity GetUserFavoritesByUserId(string userId)
        {
            TableQuery<UserFavoriteEntity> query = new TableQuery<UserFavoriteEntity>().Where(TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, userId));
            
            IEnumerable<UserFavoriteEntity> reviewResults = _table.ExecuteQuery<UserFavoriteEntity>(query);

            foreach (var tableResult in reviewResults)
            {
                return tableResult;                
            }

            return null;
        }
    }

    internal class PopularOnMovieMirchiTable : Table
    {
        protected PopularOnMovieMirchiTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new PopularOnMovieMirchiTable(table);
        }

        protected override string GetParitionKey()
        {
            return PopularOnMovieMirchiEntity.PARTITION_KEY;
        }
    }
}
