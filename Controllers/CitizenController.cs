#if DEV
#define DALChanges1
#endif 
#if PROD
#define ConfigForEnv
#endif

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
//using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WAPIMongoDBNetCore.Models;


namespace WAPIMongoDBNetCore.Controllers
{
    //[Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class CitizenController : ControllerBase
    {
        private string connStr = "";
        private string database = "";
        private string collection = "";

        public CitizenController()
        {
            #if (ConfigForEnv)
                connStr = Environment.GetEnvironmentVariable("DATABASE_URL");
                database = Environment.GetEnvironmentVariable("DATABASE_NAME");
                collection = Environment.GetEnvironmentVariable("COLLECTION_NAME");
            #else
                connStr = ConfigurationManager.AppSettings["connstring"];
                database = ConfigurationManager.AppSettings["database"];
                collection = ConfigurationManager.AppSettings["collection"];
            #endif
        }

        [Route("api/[controller]/GetCitizenDetails")]
        public ActionResult<object> Get()
        { 
            DALOps dal = new DALOps(connStr, database);

            #if DALChanges1
            var dotNetObj = dal.GetCollection(collection);
            #else
            var dotNetObj = dal.GetCollection(dal, collection);
            #endif

            return dotNetObj;
        }

        [Route("api/[controller]/GetCitizenDetailsByCitizenId")]
        public ActionResult<object> Get(string citizen_id)
        {
            DALOps dal = new DALOps(connStr, database);
            Dictionary<object, object> pairs = new Dictionary<object, object>();
            pairs.Add("citizenid", citizen_id);
#if DALChanges1
            var dotNetObj = dal.GetCollectionWithParams(collection, pairs);
#else
            var dotNetObj = dal.GetCollectionWithParams(dal, collection, pairs);
#endif
            return dotNetObj;
        }


        [HttpPost]
        [Route("api/[controller]/InsertCitizenDetails")]
        public ActionResult<object> Post(Citizens citizen)
        {
            DALOps dal = new DALOps(connStr, database);
#if DALChanges1
            var coll = dal.GetCollectionCount(collection);
#else
            var coll = dal.GetCollectionCount(dal, collection);
#endif
            citizen.citizenid = "PH" + (coll + 1).ToString();

#if DALChanges1
            var dotNetObj = dal.InsertToCollection(collection, citizen);
#else
            var dotNetObj = dal.InsertToCollection(dal, collection, citizen);
#endif
            return dotNetObj;
        }

        [HttpPut("{id}")]
        [Route("api/[controller]/UpdateCitizenDetails")]
        public ActionResult<object> Put(Citizens citizen)
        {
            DALOps dal = new DALOps(connStr, database);
            Dictionary<object, object> pairs = new Dictionary<object, object>();
            pairs.Add("citizenid", citizen.citizenid);
#if DALChanges1
            var dotNetObj = dal.UpdateCollection(collection, citizen, pairs);
#else
            var dotNetObj = dal.UpdateCollection(dal, collection, citizen);
#endif
            return dotNetObj;
        }

        private Citizens formModel(string id, string Cname, string Gender, string Email)
        {
            Citizens citizen = new Citizens();
            citizen.citizenid = id;
            citizen.CName = Cname;
            citizen.Gender = Gender;
            citizen.Email = Email;
            return citizen;
        }

        [HttpDelete]
        [Route("api/[controller]/DeleteCitizenDetails")]
        public ActionResult<object> Delete(string param)
        {
            DALOps dal = new DALOps(connStr, database);
#if DALChanges1
            var dotNetObj = dal.DeleteCollection(collection, "citizenid", param);
#else
            var dotNetObj = dal.DeleteCollection(dal, collection, "citizenid", param);
#endif
            return dotNetObj;
        }

    }
}
