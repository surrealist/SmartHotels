﻿// <auto-generated />
using GreatFriends.SmartHoltel.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GreatFriends.SmartHoltel.Services.Data.Migrations
{
    [DbContext(typeof(AppDb))]
    [Migration("20210213080019_update02")]
    partial class update02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("GreatFriends.SmartHoltel.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<double>("AreaSquareMeters")
                        .HasColumnType("float");

                    b.Property<int>("FloorNo")
                        .HasColumnType("int");

                    b.Property<string>("RoomTypeCode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoomTypeCode");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("GreatFriends.SmartHoltel.Models.RoomType", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Code");

                    b.ToTable("RoomTypes");
                });

            modelBuilder.Entity("GreatFriends.SmartHoltel.Models.Room", b =>
                {
                    b.HasOne("GreatFriends.SmartHoltel.Models.RoomType", "RoomType")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("GreatFriends.SmartHoltel.Models.RoomType", b =>
                {
                    b.Navigation("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
