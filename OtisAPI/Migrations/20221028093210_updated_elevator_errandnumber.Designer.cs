﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OtisAPI.DataAccess;

#nullable disable

namespace OtisAPI.Migrations
{
    [DbContext(typeof(SqlContext))]
    [Migration("20221028093210_updated_elevator_errandnumber")]
    partial class updated_elevator_errandnumber
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EmployeeEntityErrandEntity", b =>
                {
                    b.Property<Guid>("AssignedErrandsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignedTechniciansId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AssignedErrandsId", "AssignedTechniciansId");

                    b.HasIndex("AssignedTechniciansId");

                    b.ToTable("EmployeeEntityErrandEntity");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Elevators.ElevatorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Elevators");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Errands.ErrandEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ElevatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ErrandNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(18)");

                    b.Property<bool>("IsResolved")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(99)");

                    b.HasKey("Id");

                    b.HasIndex("ElevatorId");

                    b.HasIndex("ErrandNumber")
                        .IsUnique();

                    b.ToTable("Errands");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Errands.ErrandUpdateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfUpdate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ErrandEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ErrandEntityId");

                    b.ToTable("ErrandUpdates");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Users.EmployeeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EmployeeNumber")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeNumber")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EmployeeEntityErrandEntity", b =>
                {
                    b.HasOne("OtisAPI.Model.DataEntities.Errands.ErrandEntity", null)
                        .WithMany()
                        .HasForeignKey("AssignedErrandsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OtisAPI.Model.DataEntities.Users.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("AssignedTechniciansId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Errands.ErrandEntity", b =>
                {
                    b.HasOne("OtisAPI.Model.DataEntities.Elevators.ElevatorEntity", "Elevator")
                        .WithMany("Errands")
                        .HasForeignKey("ElevatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Elevator");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Errands.ErrandUpdateEntity", b =>
                {
                    b.HasOne("OtisAPI.Model.DataEntities.Errands.ErrandEntity", null)
                        .WithMany("ErrandUpdates")
                        .HasForeignKey("ErrandEntityId");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Elevators.ElevatorEntity", b =>
                {
                    b.Navigation("Errands");
                });

            modelBuilder.Entity("OtisAPI.Model.DataEntities.Errands.ErrandEntity", b =>
                {
                    b.Navigation("ErrandUpdates");
                });
#pragma warning restore 612, 618
        }
    }
}
