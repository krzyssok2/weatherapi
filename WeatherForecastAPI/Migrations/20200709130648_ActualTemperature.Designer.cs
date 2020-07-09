﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherForecastAPI;

namespace WeatherForecastAPI.Migrations
{
    [DbContext(typeof(WeatherContext))]
    [Migration("20200709130648_ActualTemperature")]
    partial class ActualTemperature
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WeatherForecastAPI.Entities.ActualTemperature", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CitiesId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ForecastTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CitiesId");

                    b.ToTable("ActualTemperatures");
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.Cities", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.Forecasts", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CitiesId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ForecastTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Provider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CitiesId");

                    b.ToTable("Forecasts");
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.Providers", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.UniqueProviderID", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CityId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ProviderId")
                        .HasColumnType("bigint");

                    b.Property<string>("UniqueCityID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("ProviderId");

                    b.ToTable("CityProviderID");
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.ActualTemperature", b =>
                {
                    b.HasOne("WeatherForecastAPI.Entities.Cities", null)
                        .WithMany("ActualTemparture")
                        .HasForeignKey("CitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.Forecasts", b =>
                {
                    b.HasOne("WeatherForecastAPI.Entities.Cities", null)
                        .WithMany("Forecasts")
                        .HasForeignKey("CitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WeatherForecastAPI.Entities.UniqueProviderID", b =>
                {
                    b.HasOne("WeatherForecastAPI.Entities.Cities", "City")
                        .WithMany("UniqueProviderID")
                        .HasForeignKey("CityId");

                    b.HasOne("WeatherForecastAPI.Entities.Providers", "Provider")
                        .WithMany("UniqueID")
                        .HasForeignKey("ProviderId");
                });
#pragma warning restore 612, 618
        }
    }
}
