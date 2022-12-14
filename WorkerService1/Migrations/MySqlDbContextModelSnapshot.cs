// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkerService1.Data;

namespace WorkerService1.Migrations
{
    [DbContext(typeof(MySqlDbContext))]
    partial class MySqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("WorkerService1.Data.Entities.Floor", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("building")
                        .HasColumnType("text");

                    b.Property<string>("campus")
                        .HasColumnType("text");

                    b.Property<string>("company")
                        .HasColumnType("text");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<string>("floorPlanUrl")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("parentFloorId")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("WorkerService1.Data.Entities.Sensor", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FloorId")
                        .HasColumnType("int");

                    b.Property<int>("areaId")
                        .HasColumnType("int");

                    b.Property<string>("groupId")
                        .HasColumnType("text");

                    b.Property<string>("macAddress")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("sensorclass")
                        .HasColumnType("text");

                    b.Property<string>("xaxis")
                        .HasColumnType("text");

                    b.Property<string>("yaxis")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("FloorId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("WorkerService1.Data.Entities.Sensor", b =>
                {
                    b.HasOne("WorkerService1.Data.Entities.Floor", "floor")
                        .WithMany("Sensors")
                        .HasForeignKey("FloorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("floor");
                });

            modelBuilder.Entity("WorkerService1.Data.Entities.Floor", b =>
                {
                    b.Navigation("Sensors");
                });
#pragma warning restore 612, 618
        }
    }
}
