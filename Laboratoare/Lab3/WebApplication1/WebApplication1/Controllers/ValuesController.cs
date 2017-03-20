using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("sum")]
        public string Get(int n1, int n2)
        {
            int sum = n1 + n2;
            return "sum="+ sum;
        }

        //[HttpGet("{id}")]
        //public string Get(string id)
        //{
        //    return "string=" + id;
        //}

        // POST api/values
        [HttpPost]
        public MathObject Post([FromBody]MathObject obj)
        {
            obj.Sum = obj.N1 + obj.N2;
            return obj;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
