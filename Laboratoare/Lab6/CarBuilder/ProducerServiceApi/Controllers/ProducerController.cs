using System;
using Microsoft.AspNetCore.Mvc;

namespace ProducerServiceApi.Controllers
{
    [Route("api/[controller]")]
    public class ProducerController : Controller
    {
        // GET api/producer/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //Return status of build or car if completed
            return "value";
        }

        // POST api/producer/
        [HttpPost]
        public string Post([FromBody]string value)
        {
            //Request for car

            //store building the car in a permanent list

            var id = new Random().Next(100, 199);

            var buildRequest = "B|" + id + "|" + value;

            ReadWriteQueue.SendBuildRequest(buildRequest);

            return buildRequest;
        }

        // PUT api/producer/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody]string value)
        {
            //update list of build

            return "Id=" + id + " | string";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
