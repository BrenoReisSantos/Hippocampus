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
    [Migration("20240514233243_AddedLevelAndStateFieldToTheRecipientMonitorEntity")]
    partial class AddedLevelAndStateFieldToTheRecipientMonitorEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.RecipientLog", b =>
                {
                    b.Property<int>("RecipientLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RecipientLogId"));

                    b.Property<Guid>("RecipientMonitorId")
                        .HasColumnType("uuid");

                    b.Property<string>("RecipientState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("RecipientLogId");

                    b.HasAlternateKey("RecipientMonitorId", "RegisterDate");

                    b.HasIndex("RegisterDate")
                        .IsDescending();

                    b.ToTable("RecipientLogs");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.RecipientMonitor", b =>
                {
                    b.Property<Guid>("RecipientMonitorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MacAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaxHeight")
                        .HasColumnType("integer");

                    b.Property<int>("MinHeight")
                        .HasColumnType("integer");

                    b.Property<Guid?>("MonitorLinkedToRecipientMonitorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("RecipientState")
                        .HasColumnType("integer");

                    b.Property<string>("RecipientType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("RecipientMonitorId");

                    b.HasIndex("MacAddress")
                        .IsUnique();

                    b.HasIndex("MonitorLinkedToRecipientMonitorId");

                    b.ToTable("RecipientMonitors");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.RecipientLog", b =>
                {
                    b.HasOne("Hippocampus.Domain.Models.Entities.RecipientMonitor", "RecipientMonitor")
                        .WithMany("RecipientLogs")
                        .HasForeignKey("RecipientMonitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Hippocampus.Domain.Models.Values.LevelPercentage", "LevelPercentage", b1 =>
                        {
                            b1.Property<int>("RecipientLogId")
                                .HasColumnType("integer");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("LevelPercentage");

                            b1.HasKey("RecipientLogId");

                            b1.ToTable("RecipientLogs");

                            b1.WithOwner()
                                .HasForeignKey("RecipientLogId");
                        });

                    b.Navigation("LevelPercentage")
                        .IsRequired();

                    b.Navigation("RecipientMonitor");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.RecipientMonitor", b =>
                {
                    b.HasOne("Hippocampus.Domain.Models.Entities.RecipientMonitor", "MonitorLinkedTo")
                        .WithMany()
                        .HasForeignKey("MonitorLinkedToRecipientMonitorId");

                    b.OwnsOne("Hippocampus.Domain.Models.Values.LevelPercentage", "LevelPercentage", b1 =>
                        {
                            b1.Property<Guid>("RecipientMonitorId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("LevelPercentage");

                            b1.HasKey("RecipientMonitorId");

                            b1.ToTable("RecipientMonitors");

                            b1.WithOwner()
                                .HasForeignKey("RecipientMonitorId");
                        });

                    b.Navigation("LevelPercentage")
                        .IsRequired();

                    b.Navigation("MonitorLinkedTo");
                });

            modelBuilder.Entity("Hippocampus.Domain.Models.Entities.RecipientMonitor", b =>
                {
                    b.Navigation("RecipientLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
