﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Redgum.ServerMonitor.Web.ServerMonitor.Data;
using System;

namespace Redgum.ServerMonitor.Web.Migrations
{
    [DbContext(typeof(MonitorDbContext))]
    [Migration("20171207041240_InitialCreate2")]
    partial class InitialCreate2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataAgregateDataModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<string>("DataKey")
                        .HasMaxLength(50);

                    b.Property<Guid>("Identifier");

                    b.Property<int?>("ServerId");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("ServerDataAgregates");
                });

            modelBuilder.Entity("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataDataModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<string>("DataKey")
                        .HasMaxLength(50);

                    b.Property<Guid>("Identifier");

                    b.Property<int?>("ServerId");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("ServerData");
                });

            modelBuilder.Entity("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Domain")
                        .HasMaxLength(100);

                    b.Property<Guid>("Identifier");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Redgum.ServerMonitor.Web.ServerMonitor.Data.SettingsDataModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<string>("DataKey")
                        .HasMaxLength(50);

                    b.Property<Guid>("Identifier");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataAgregateDataModel", b =>
                {
                    b.HasOne("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataModel", "Server")
                        .WithMany("DataAggregate")
                        .HasForeignKey("ServerId");
                });

            modelBuilder.Entity("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataDataModel", b =>
                {
                    b.HasOne("Redgum.ServerMonitor.Web.ServerMonitor.Data.ServerDataModel", "Server")
                        .WithMany("Data")
                        .HasForeignKey("ServerId");
                });
#pragma warning restore 612, 618
        }
    }
}
