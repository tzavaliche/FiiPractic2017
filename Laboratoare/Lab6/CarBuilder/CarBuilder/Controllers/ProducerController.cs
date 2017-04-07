using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarBuilder.Controllers
{
    [Route("api/[controller]")]
    public class ProducerController : Controller
    {
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //Return status of build or car if completed
            return "value";
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string value)
        {
            //Request for car
            var id = new Random().Next(100, 199);

            var buildRequest = "B|" + id + "|" + value;

            ReadWriteQueue.SendBuildRequest(buildRequest);

            return buildRequest;
        }

    }
}
