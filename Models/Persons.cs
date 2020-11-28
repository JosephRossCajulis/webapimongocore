using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAPIMongoDBNetCore.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Persons")]
    public class Persons
    {
        [System.ComponentModel.DataAnnotations.Key]
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string _id { get; set; }

        public string personid { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

        public Persons()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
    }
}