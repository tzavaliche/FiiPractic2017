using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using ProducerServiceApi;
using ProducerServiceApi.Infrastructure;

namespace ProducerServiceApi.Controllers
{
    [Route("api/[controller]")]
    public class ProducerController : Controller
    {
        private Repository repo;

        public ProducerController(Repository repo)
        {
            this.repo = repo;
        }

        // GET api/producer
        [HttpGet]
        public IList<CarOrder> Get()
        {
            return repo.CarOrders;
        }

        // GET api/producer/5
        [HttpGet("{id}")]
        public CarOrder Get(int id)
        {
            return repo.CarOrders.FirstOrDefault(c => c.Id.Equals(id));
        }

        // POST api/producer/
        [HttpPost]
        public string Post([FromBody]string value)
        {
            //Request for car

            //store building the car in a permanent list

            var id = new Random().Next(100, 199);

            var newOrder = new CarOrder
            {
                Id = id,
                Code = value
            };

            repo.CarOrders.Add(newOrder);

            var buildRequest = "B|" + id + "|" + value;

            ReadWriteQueue.SendBuildRequest(buildRequest);

            return buildRequest;
        }

        // PUT api/producer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            //update list of build

            var existingOrder = repo.CarOrders.FirstOrDefault(c => c.Id.Equals(id));

            var parts = value.Split('|');

            var component = parts[1].Trim();
            existingOrder.ComponentStatus[component] = true;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
