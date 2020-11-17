using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAPIMongoDBNetCore.Models
{
    public class Citizens
    {
        [System.ComponentModel.DataAnnotations.Key]
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string _id { get; set; }


        public string CName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string citizenid { get; set; }

        public Citizens()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
    }
}
