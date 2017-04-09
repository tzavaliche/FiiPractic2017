using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerServiceApi.Infrastructure
{
    public class Repository
    {
        public Repository()
        {
            CarOrders = new List<CarOrder>();
        }

        public IList<CarOrder> CarOrders { get; set; }
    }
}
