﻿// <auto-generated />
using MapApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MapApplication.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240731114734_AddWktDbTable")]
    partial class AddWktDbTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MapApplication.Data.PointDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("X_coordinate")
                        .HasColumnType("double precision")
                        .HasColumnName("X_coordinate");

                    b.Property<double>("Y_coordinate")
                        .HasColumnType("double precision")
                        .HasColumnName("Y_coordinate");

                    b.HasKey("Id");

                    b.ToTable("points", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
