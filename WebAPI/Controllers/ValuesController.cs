using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OracleTest.jcz.dao;
using entity;
using Newtonsoft.Json.Linq;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var db = OracleDB.GetInstance();

            var date = db.Queryable<WCS_TASKINFO>().ToList();

            string ss = date[0].ToString();
         
            return new string[] { "value1", "value2" };
           
        }

       
        [HttpGet]
        [Route("jcz")]
        public WCS_TASKINFO Get1()
        {
            var db = OracleDB.GetInstance();

            var date = db.Queryable<WCS_TASKINFO>().ToList();

            string ss = date[0].ToString();
            return date[0];
          

        }

        [HttpGet]
        [Route("json")]
        public WCS_TASKINFO Get2([FromBody]JObject data)
        {
            Console.WriteLine(data.ToString());
           
            var db = OracleDB.GetInstance();

            var date = db.Queryable<WCS_TASKINFO>().ToList();

            string ss = date[0].ToString();
            return date[0];


        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
