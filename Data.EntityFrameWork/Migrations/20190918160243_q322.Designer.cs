﻿// <auto-generated />
using System;
using Data.EntityFrameWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.EntityFrameWork.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20190918160243_q322")]
    partial class q322
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Entities.Commentary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Commentaries");
                });

            modelBuilder.Entity("Data.Entities.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("Data.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Data.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("Subject");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Data.Entities.Commentary", b =>
                {
                    b.HasOne("Data.Entities.Project", "Project")
                        .WithMany("Comments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Entities.Content", b =>
                {
                    b.HasOne("Data.Entities.Project", "Project")
                        .WithOne("Content")
                        .HasForeignKey("Data.Entities.Content", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Entities.Post", b =>
                {
                    b.HasOne("Data.Entities.Project", "Project")
                        .WithMany("Posts")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
