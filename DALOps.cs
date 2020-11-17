using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAPIMongoDBNetCore
{
    public class DALOps
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public DALOps()
        {
            _client = new MongoClient("mongodb+srv://joseph-test:Jcajulis11@testdb1.jh6jv.mongodb.net/testdb1?retryWrites=true&w=majority");
        }

        /// <summary>
        /// Initializes an object with a MongoClient object with connection string and database name specified
        /// </summary>
        /// <param name="constring">Connection string of the MongoDB connection</param>
        /// <param name="db">Name of the database to connect to</param>
        public DALOps(string constring, string db)
        {
            _client = new MongoClient(constring);
            _database = _client.GetDatabase(db);
        }

        public object GetCollection(string constring, string db, string collection)
        {
            MongoClient dbClient = new MongoClient(constring);
            var dbase = dbClient.GetDatabase(db);
            var coll = dbase.GetCollection<BsonDocument>("Todo");
            var returnDocument = coll.Find(new BsonDocument()).FirstOrDefault();

            var dotNetObj = BsonTypeMapper.MapToDotNetValue(returnDocument);
            //var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
            JsonConvert.SerializeObject(dotNetObj);

            return dotNetObj;
        }

        /// <summary>
        /// Retrieves all of the contents of the collection object
        /// </summary>
        /// <param name="dal">The MongoDB client object initialized</param>
        /// <param name="collection">Name of the collection to retrieve</param>
        /// <returns></returns>
        public object GetCollection(DALOps dal, string collection)
        {
            var coll = dal._database.GetCollection<BsonDocument>(collection);
            var returnDocument = coll.Find(new BsonDocument()).ToList();

            //var dotNetObj = BsonTypeMapper.MapToDotNetValue(returnDocument);
            var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
            JsonConvert.SerializeObject(dotNetObj);

            return dotNetObj;
        }

        public int GetCollectionCount(DALOps dal, string collection)
        {
            var coll = dal._database.GetCollection<BsonDocument>(collection);
            var returnDocument = coll.Find(new BsonDocument()).ToList();

            var count = returnDocument.Count;

            return count;
        }

        /// <summary>
        /// Retrieves the content of the collection object with respect to the filters
        /// </summary>
        /// <param name="dal">The MongoDB client object initialized</param>
        /// <param name="collection">Name of the collection to retrieve</param>
        /// <param name="paramss">The filters to be included in a form of key/value pairs</param>
        /// <returns></returns>
        public object GetCollectionWithParams(DALOps dal, string collection, Dictionary<object, object> paramss)
        {
            if (paramss.Count == 1)
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                var coll = dal._database.GetCollection<BsonDocument>(collection);
                var returnDocument = coll.Find(filter).FirstOrDefault();
                var dotNetObj = BsonTypeMapper.MapToDotNetValue(returnDocument);
                //var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                JsonConvert.SerializeObject(dotNetObj);

                return dotNetObj;
            }
            else
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                for (int i = 1; i < paramss.Count; i++)
                {
                    var key = paramss.Keys.ElementAt(i).ToString();
                    var value = paramss.Values.ElementAt(i).ToString();

                    if ((value.ToString().ToLowerInvariant() == "true") || (value.ToString().ToLowerInvariant() == "false"))
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, Convert.ToBoolean(value))); }
                    else
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, value)); }


                }
                var coll = dal._database.GetCollection<BsonDocument>(collection);
                var returnDocument = coll.Find(filter).FirstOrDefault();
                var dotNetObj = BsonTypeMapper.MapToDotNetValue(returnDocument);
                //var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                JsonConvert.SerializeObject(dotNetObj);

                return dotNetObj;
            }
        }

        /// <summary>
        /// Inserts a model object into the collection
        /// </summary>
        /// <param name="dal">The MongoDB client object initialized</param>
        /// <param name="collection">Name of the collection to retrieve</param>
        /// <param name="item">Model object to be inserted</param>
        /// <returns></returns>
        public object InsertToCollection(DALOps dal, string collection, object item)
        {
            var coll = dal._database.GetCollection<BsonDocument>(collection);

            var returnDocument = new BsonDocument(item.ToBsonDocument());
            returnDocument.Remove("_t");
            coll.InsertOne(returnDocument);

            var dotNetObj = BsonTypeMapper.MapToDotNetValue(returnDocument);
            JsonConvert.SerializeObject(dotNetObj);

            return dotNetObj;
        }

        /// <summary>
        /// Updates a model object into the collection with respect to the search parameters
        /// </summary>
        /// <param name="dal">The MongoDB client object initialized</param>
        /// <param name="collection">Name of the collection to retrieve</param>
        /// <param name="item">Model object to be inserted</param>
        /// <param name="paramss">The filters to be included in a form of key/value pairs</param>
        /// <returns></returns>
        public object UpdateCollection(DALOps dal, string collection, object item, Dictionary<object, object> paramss)
        {
            if (paramss.Count == 1)
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                var updatedDoc = item.ToBsonDocument();
                updatedDoc.Remove("_t");
                updatedDoc.Remove("_id");
                var coll = dal._database.GetCollection<BsonDocument>(collection);
                var returnDocument = coll.ReplaceOne(filter, updatedDoc);
                JsonConvert.SerializeObject(returnDocument);

                return returnDocument;
            }
            else
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                for (int i = 1; i < paramss.Count; i++)
                {
                    var key = paramss.Keys.ElementAt(i).ToString();
                    var value = paramss.Values.ElementAt(i).ToString();

                    if ((value.ToString().ToLowerInvariant() == "true") || (value.ToString().ToLowerInvariant() == "false"))
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, Convert.ToBoolean(value))); }
                    else
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, value)); }


                }
                var updatedDoc = item.ToBsonDocument();
                updatedDoc.Remove("_t");
                updatedDoc.Remove("_id");
                var coll = dal._database.GetCollection<BsonDocument>(collection);
                var returnDocument = coll.UpdateOne(filter, updatedDoc);
                JsonConvert.SerializeObject(returnDocument);

                return returnDocument;
            }
        }

        /// <summary>
        /// Deletes a model object into the collection with respect to the search parameters
        /// </summary>
        /// <param name="dal">The MongoDB client object initialized</param>
        /// <param name="collection">Name of the collection to retrieve</param>
        /// <param name="key">Name of the column to be used in the filter</param>
        /// <param name="param">Value of the column to be used in the filter</param>
        /// <returns></returns>
        public object DeleteCollection(DALOps dal, string collection, string key, string param)
        {
            var coll = dal._database.GetCollection<BsonDocument>(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(key, param);
            var res = coll.DeleteOne(filter);

            JsonConvert.SerializeObject(res);
            return res;
        }
    }
}
