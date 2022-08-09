using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService1.Data.Entities;

namespace WorkerService1.Data
{
   public class MySqlDbContext : DbContext
    {
          private readonly IConfiguration _configuration;



        public MySqlDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
           
        }
        public MySqlDbContext()
        {

        }

        public DbSet<Floor> Floors { get; set; }
        public DbSet<Sensor> Sensors { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //"server=localhost;user=root;database=floorsendb;password=;"
            optionsBuilder.UseMySQL("server=localhost;user=root;database=floorsendb;password=;");
        }
    }
}
