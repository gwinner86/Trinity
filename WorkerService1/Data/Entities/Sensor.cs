using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1.Data.Entities
{
   public class Sensor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string xaxis { get; set; }
        public string yaxis { get; set; }
        public string groupId { get; set; }
        public string macAddress { get; set; }
        public string sensorclass { get; set; }
        public string areaId { get; set; }

        //Foreign key for Floor
        public int FloorId { get; set; }
        public Floor floor { get; set; }
    }
}
