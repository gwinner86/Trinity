using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1.Data.Entities
{
   public class Floor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string building { get; set; }
        public string campus { get; set; }
        public string company { get; set; }
        public string description { get; set; }
        public string floorPlanUrl { get; set; }
        public string parentFloorId { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}
