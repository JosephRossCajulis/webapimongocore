#if DEV
#define ConfigForEnv
#endif 
#if PROD
#define ConfigForEnv
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Configuration;
using WAPIMongoDBNetCore.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace WAPIMongoDBNetCore.Controllers
{
    /********************************************************
     * Notes:
     * 1. When using MVC controller in ASP.NET Core, Produces response attribute must be indicated
     * 2. Consumes attribute must be indicated in POST
     * 3. [FromBody] attribute must be indicated in POST and PUT methods if JSON is to be passed from PostMan
     * 
     *********************************************************/

    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    public class NameController : Controller
    {
        private string connStr = "";
        private string database = "";
        private string collection = "";

        public NameController()
        {
            connStr = ConfigurationManager.AppSettings["connstring"];
            database = ConfigurationManager.AppSettings["database"];
            collection = ConfigurationManager.AppSettings["persons_collection"];
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            DALOps dal = new DALOps(connStr, database);
            var dotNetObj = dal.GetCollection(dal, collection);
            return Ok(dotNetObj);
        }

        [HttpGet("{personid}")]
        public IActionResult GetOne(string personid)
        {
            DALOps dal = new DALOps(connStr, database);
            Dictionary<object, object> pairs = new Dictionary<object, object>();
            pairs.Add("personid", personid);
            var dotNetObj = dal.GetCollectionWithParams(dal, collection, pairs);
            return Ok(dotNetObj);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        //[Route("api/[controller]/Post")]
        public IActionResult Post([FromBody] Persons person)
        {
            DALOps dal = new DALOps(connStr, database);
            var coll = dal.GetCollectionCount(dal, collection);
            person.personid = "PH" + (coll + 1).ToString();
            var dotNetObj = dal.InsertToCollection(dal, collection, person);
            return Ok(dotNetObj);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        //[Route("api/[controller]/Put")]
        public IActionResult Put([FromBody] Persons person, string personid)
        {
            DALOps dal = new DALOps(connStr, database);
            Dictionary<object, object> pairs = new Dictionary<object, object>();
            pairs.Add("personid", personid);
            var dotNetObj = dal.UpdateCollection(dal, collection, person, pairs);
            return Ok(dotNetObj);
        }

        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        //[Route("api/[controller]/Delete")]
        public IActionResult Delete(string param)
        {
            DALOps dal = new DALOps(connStr, database);
            var dotNetObj = dal.DeleteCollection(dal, collection, "personid", param);
            return Ok(dotNetObj);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}