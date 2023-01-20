﻿// <auto-generated />
using System;
using Hippocampus.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hippocampus.Migrations
{
    [DbContext(typeof(LogContext))]
    partial class LogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hippocampus.Models.RecipientLog", b =>
                {
                    b.Property<Guid>("RecipientLogId")
                        .HasColumnType("uuid");

                    b.Property<string>("MacAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RecipientLogId");

                    b.HasIndex("MacAddress")
                        .IsUnique();

                    b.ToTable("RecipientLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
