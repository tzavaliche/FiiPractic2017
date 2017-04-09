using System.Collections;
using System.Collections.Generic;

namespace ProducerServiceApi.Infrastructure
{
    public class CarOrder
    {
        public CarOrder()
        {
            ComponentStatus = new Dictionary<string, bool>();
            foreach (var c in new string[] { "C", "W", "E", "I" })
            {
                ComponentStatus[c] = false;
            }
        }

        public int Id { get; set; }

        public string Code { get; set; }

        public string Status
        {
            get
            {
                var status = 0;
                foreach (var key in ComponentStatus.Keys)
                {
                    if (ComponentStatus[key])
                    {
                        status += 25;
                    }
                }

                return $"{status}%";
            }
        }

        public IDictionary<string, bool> ComponentStatus { get; set; }
    }
}