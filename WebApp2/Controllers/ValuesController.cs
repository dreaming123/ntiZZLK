using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OracleTest.jcz.dao;
using entity;
using Newtonsoft.Json.Linq;
using OracleTest.jcz.model;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WebApp2.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get(MainDate<WCS_TASKINFO> rec)
        {
            var db = OracleDB.GetInstance();

            var date = db.Queryable<WCS_TASKINFO>().ToList();

            string ss = date[0].ToString();
            var requestJson = JsonConvert.SerializeObject(rec);

            HttpContent httpContent = new StringContent(requestJson);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpClient = new HttpClient();

            var responseJson = httpClient.PostAsync("https://localhost:44352/jcz", httpContent).Result.Content.ReadAsStringAsync().Result;

           
            //ViewBag.s = responseJson;


            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [Route("jcz")]
        public WCS_TASKINFO Get1(MainDate<WCS_TASKINFO> rec)
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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
