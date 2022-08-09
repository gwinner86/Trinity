using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WorkerService1.Data;
using WorkerService1.Data.Entities;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;
        string username;
        string apiKey;

        long timestamp;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();

            username = "enappgy-demo";
            apiKey = "12345";

            timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();


            client.DefaultRequestHeaders.Add("ts", timestamp.ToString());
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            client.DefaultRequestHeaders.Add("Authorization", Hash($"{username}{apiKey}{timestamp}"));

            return base.StartAsync(cancellationToken);


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {

                    var result = await client.GetAsync("https://assignment.enappgy.io/ems/api/org/floor/list");

                    Console.WriteLine(timestamp);
                    Console.WriteLine(Hash($"{username}{apiKey}{timestamp}"));

                    if (result.IsSuccessStatusCode)
                    {

                        var response =await result.Content.ReadAsStringAsync();

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(response);

                        string json = JsonConvert.SerializeXmlNode(doc);

                        dynamic jsonObj = JsonConvert.DeserializeObject(json);

                        Console.WriteLine(jsonObj.floors.floor);
                        using (var mydbContext = new MySqlDbContext())
                        {
                            foreach (var item in jsonObj.floors.floor)
                            {
                                Floor floor = new Floor();
                                floor.id = item.id;
                                floor.name = item.name;
                                floor.floorPlanUrl = item.floorPlanUrl;
                                floor.building = item.building;
                                floor.campus = item.campus;
                                floor.company = item.company;
                                floor.description = item.description;
                                floor.floorPlanUrl = item.floorPlanUrl;
                                floor.parentFloorId = item.parentFloorId;
                                Console.WriteLine($"Floors  : {floor.name} {floor.floorPlanUrl}");

                                // var floorExist = await mydbContext.Floors.FindAsync(floor.id);

                                bool floorExists = mydbContext.Set<Floor>().Any(x => x.id == floor.id);

                                if(!floorExists)
                                {
                                    mydbContext.Floors.Add(floor);
                                }
                                else
                                {
                                    mydbContext.Floors.Update(floor);
                                }

                                
                                

                                var sensorResponse = await client.GetAsync($"https://assignment.enappgy.io/ems/api/org/fixture/location/list/floor/{floor.id}/fixtured");

                                var sensorResult = await sensorResponse.Content.ReadAsStringAsync();
                                sensorResult = sensorResult.Replace("class", "sensorclass");

                                XmlDocument docSensor = new XmlDocument();
                                docSensor.LoadXml(sensorResult);

                                string sensorJsonString = JsonConvert.SerializeXmlNode(docSensor);

                                dynamic SensorJsonObj = JsonConvert.DeserializeObject(sensorJsonString);


                                foreach (var sen in SensorJsonObj.fixtures.fixture)
                                {
                                    Sensor sensor = new Sensor();
                                    sensor.id = sen.id;
                                    sensor.name = sen.name;
                                    sensor.macAddress = sen.macAddress;
                                    sensor.sensorclass = sen.sensorclass;
                                    sensor.xaxis = sen.xaxis;
                                    sensor.yaxis = sen.yaxis;
                                    sensor.groupId = sen.groupId;
                                    sensor.areaId = sen.areaId;
                                    sensor.FloorId = floor.id;
                                    
                                    // kindly check for the update you talked about 
                                    // if it exist just do up

                                    Console.WriteLine($"Sensor List : {sensor.id} {sensor.name} {sensor.macAddress}");
                                    //
                                    bool sensorExists = mydbContext.Set<Sensor>().Any(x => x.id == sensor.id);

                                    if (!sensorExists)
                                    {
                                        mydbContext.Sensors.Add(sensor);
                                    }
                                    else
                                    {
                                        mydbContext.Sensors.Update(sensor);
                                    }
                                }

                            }

                            mydbContext.SaveChanges();
                        }

                        _logger.LogInformation(" Success ");
                    }
                    else
                    {
                        _logger.LogError(" Failed");
                    }

                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine( "Error ",ex.Message.ToString());
                    throw;
                }
            }
        }

        static string Hash(string input)
        {
        
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
