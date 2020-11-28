#if DEV
#define ConfigForEnv
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
            var dotNetObj = dal.GetCollection(dal, collection);
            return dotNetObj;
        }

        [Route("api/[controller]/GetCitizenDetailsByCitizenId")]
        public ActionResult<object> Get(string citizen_id)
        {
            DALOps dal = new DALOps(connStr, database);
            Dictionary<object, object> pairs = new Dictionary<object, object>();
            pairs.Add("citizenid", citizen_id);
            var dotNetObj = dal.GetCollectionWithParams(dal, collection, pairs);

            return dotNetObj;
        }

        ////[HttpGet]
        ////[Route("GetCitizenDetailsTwoParam")]
        ////public IHttpActionResult GetByTextAndStatus(string id, string status)
        ////{
        ////    DALOps dal = new DALOps(connStr, database);
        ////    Dictionary<object, object> pairs = new Dictionary<object, object>();
        ////    pairs.Add("text", id);
        ////    pairs.Add("done", status);
        ////    var dotNetObj = dal.GetCollectionWithParams(dal, collection, pairs);
        ////    return Ok(dotNetObj);
        ////}


        [HttpPost]
        [Route("api/[controller]/InsertCitizenDetails")]
        public ActionResult<object> Post(Citizens citizen)
        {
            DALOps dal = new DALOps(connStr, database);
            var coll = dal.GetCollectionCount(dal, collection);
            citizen.citizenid = "PH" + (coll + 1).ToString();
            var dotNetObj = dal.InsertToCollection(dal, collection, citizen);
            return dotNetObj;
        }

        [HttpPut("{id}")]
        [Route("api/[controller]/UpdateCitizenDetails")]
        public ActionResult<object> Put(Citizens citizen)
        {
            DALOps dal = new DALOps(connStr, database);
            Dictionary<object, object> pairs = new Dictionary<object, object>();
            pairs.Add("citizenid", citizen.citizenid);
            var dotNetObj = dal.UpdateCollection(dal, collection, citizen, pairs);
            return dotNetObj;
        }

        //[System.Web.Http.HttpPut]
        //[Route("api/[controller]/UpdateCitizenDetails")]
        //public System.Web.Http.IHttpActionResult Put(Citizens citizen)
        //{
        //    DALOps dal = new DALOps(connStr, database);
        //    Dictionary<object, object> pairs = new Dictionary<object, object>();
        //    pairs.Add("citizenid", citizen.citizenid);
        //    var dotNetObj = dal.UpdateCollection(dal, collection, citizen, pairs);
        //    return (System.Web.Http.IHttpActionResult)Ok(dotNetObj);
        //}

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
            var dotNetObj = dal.DeleteCollection(dal, collection, "citizenid", param);
            return dotNetObj;
        }

    }
}
