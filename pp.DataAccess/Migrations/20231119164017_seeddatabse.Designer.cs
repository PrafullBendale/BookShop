﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pp.DataAccess.Data;


#nullable disable

namespace pp.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231119164017_seeddatabse")]
    partial class seeddatabse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApp.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Action",
                            DisplayOrder = 1
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Drama",
                            DisplayOrder = 2
                        },
                        new
                        {
                            Id = 3,
                            CategoryName = "Horror",
                            DisplayOrder = 3
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
