﻿// <auto-generated />
using System;
using DevBlog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DevBlog.Domain.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("DevBlog.Domain.Entities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("DevBlog.Domain.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("DevBlog.Domain.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<string>("HeaderImage")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PublishDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ReadMinutes")
                        .HasColumnType("integer");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Body = "Hello world",
                            HeaderImage = "angular-card2.png",
                            PublishDate = new DateTime(2021, 3, 31, 15, 34, 27, 461, DateTimeKind.Utc).AddTicks(9050),
                            ReadMinutes = 8,
                            Slug = "angular",
                            Title = "Angular"
                        },
                        new
                        {
                            Id = 2,
                            Body = "Hello world",
                            HeaderImage = "dotnet-card2.png",
                            PublishDate = new DateTime(2021, 3, 31, 15, 34, 27, 461, DateTimeKind.Utc).AddTicks(9952),
                            ReadMinutes = 6,
                            Slug = "core",
                            Title = ".NET Core"
                        },
                        new
                        {
                            Id = 3,
                            Body = "Hello world",
                            HeaderImage = "code-card3.png",
                            PublishDate = new DateTime(2021, 3, 31, 15, 34, 27, 461, DateTimeKind.Utc).AddTicks(9968),
                            ReadMinutes = 9,
                            Slug = "git",
                            Title = "Git Hub"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
