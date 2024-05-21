﻿// <auto-generated />
using System;
using Hippocampus.Domain.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hippocampus.Domain.Migrations
{
    [DbContext(typeof(HippocampusContext))]
    [Migration("20240521022718_AddedGardenModeColumn")]
    partial class AddedGardenModeColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.WaterTank", b =>
                {
                    b.Property<Guid>("WaterTankId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CurrentLevel")
                        .HasColumnType("integer");

                    b.Property<bool?>("GardenMode")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("LevelWhenEmpty")
                        .HasColumnType("integer");

                    b.Property<int>("LevelWhenFull")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool?>("PumpingWater")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("PumpsToWaterTankId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("WaterTankId");

                    b.HasIndex("PumpsToWaterTankId");

                    b.ToTable("WaterTank");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.WaterTankLog", b =>
                {
                    b.Property<long>("WaterTankLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("WaterTankLogId"));

                    b.Property<bool?>("GardenMode")
                        .HasColumnType("boolean");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("PumpingWater")
                        .HasColumnType("boolean");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WaterTankId")
                        .HasColumnType("uuid");

                    b.HasKey("WaterTankLogId");

                    b.HasAlternateKey("WaterTankId", "LogDate");

                    b.HasIndex("LogDate")
                        .IsDescending();

                    b.ToTable("WaterTankLog");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.WaterTank", b =>
                {
                    b.HasOne("Hippocampus.Domain.Models.Entities.WaterTank", "PumpsTo")
                        .WithMany("PumpedFrom")
                        .HasForeignKey("PumpsToWaterTankId");

                    b.Navigation("PumpsTo");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.WaterTankLog", b =>
                {
                    b.HasOne("Hippocampus.Domain.Models.Entities.WaterTank", "WaterTank")
                        .WithMany("WaterTankLogs")
                        .HasForeignKey("WaterTankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WaterTank");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.WaterTank", b =>
                {
                    b.Navigation("PumpedFrom");

                    b.Navigation("WaterTankLogs");
                });
#pragma warning restore 612, 618
        }
    }
}