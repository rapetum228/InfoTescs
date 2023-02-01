﻿// <auto-generated />
using System;
using InfoTecs.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(InfotecsDataContext))]
    partial class InfotecsDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Period", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<byte>("Hours")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Minutes")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Seconds")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Periods");
                });

            modelBuilder.Entity("DAL.Entities.Result", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<double>("AverageDiscretTime")
                        .HasColumnType("float");

                    b.Property<double>("AverageParameters")
                        .HasColumnType("float");

                    b.Property<int>("CountLines")
                        .HasColumnType("int");

                    b.Property<long>("DateTimePeriodId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("MaximalParameter")
                        .HasColumnType("float");

                    b.Property<double>("MedianaByParameters")
                        .HasColumnType("float");

                    b.Property<double>("MinimalParameter")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DateTimePeriodId");

                    b.HasIndex("FileName")
                        .IsUnique();

                    b.ToTable("Results");
                });

            modelBuilder.Entity("DAL.Entities.Value", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DiscretTime")
                        .HasColumnType("int");

                    b.Property<double>("Parameter")
                        .HasColumnType("float");

                    b.Property<long?>("ResultId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ResultId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("DAL.Entities.Result", b =>
                {
                    b.HasOne("DAL.Entities.Period", "DateTimePeriod")
                        .WithMany()
                        .HasForeignKey("DateTimePeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DateTimePeriod");
                });

            modelBuilder.Entity("DAL.Entities.Value", b =>
                {
                    b.HasOne("DAL.Entities.Result", null)
                        .WithMany("Values")
                        .HasForeignKey("ResultId");
                });

            modelBuilder.Entity("DAL.Entities.Result", b =>
                {
                    b.Navigation("Values");
                });
#pragma warning restore 612, 618
        }
    }
}
