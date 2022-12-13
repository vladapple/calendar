﻿// <auto-generated />
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Data.Migrations
{
    [DbContext(typeof(CalendarDbContext))]
    [Migration("20221207162538_calendarDb")]
    partial class calendarDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.Attending", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Attendings", "public");
                });

            modelBuilder.Entity("Backend.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AgeRestricted")
                        .HasColumnType("boolean");

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("integer");

                    b.Property<decimal>("AverageRating")
                        .HasColumnType("numeric");

                    b.Property<int>("CurrentStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Descr")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EventCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventCategoryId");

                    b.HasIndex("UserId");

                    b.HasIndex("VenueId");

                    b.ToTable("Events", "public");
                });

            modelBuilder.Entity("Backend.Models.EventCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Descr")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.HasKey("Id");

                    b.ToTable("EventCategories", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Concert",
                            Descr = "Live music"
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Play",
                            Descr = "Theatrical presentation"
                        });
                });

            modelBuilder.Entity("Backend.Models.EventMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("Media")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventMedias", "public");
                });

            modelBuilder.Entity("Backend.Models.EventReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("EventReviews", "public");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("DOB")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Users", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContactName = "",
                            DOB = new DateTimeOffset(new DateTime(2004, 12, 7, 16, 25, 38, 341, DateTimeKind.Unspecified).AddTicks(6808), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "User@Test.com",
                            IsBanned = false,
                            OrganizationName = "",
                            Password = "$2a$11$DV2zHCqRmEeyVy4esvhLGOMuQj2RDhNCHVMe/dhQMBzJdhot5jeoO",
                            Role = 0,
                            UserName = "TestUser"
                        },
                        new
                        {
                            Id = 2,
                            ContactName = "",
                            DOB = new DateTimeOffset(new DateTime(2004, 12, 7, 16, 25, 38, 474, DateTimeKind.Unspecified).AddTicks(8893), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "Admin@Test.com",
                            IsBanned = false,
                            OrganizationName = "",
                            Password = "$2a$11$O5ioQrDYwFEg.kdzdxJKE.zJAn/JU3TKG5OTAApXMVeburC5cLlLW",
                            Role = 1,
                            UserName = "TestAdmin"
                        });
                });

            modelBuilder.Entity("Backend.Models.Venue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<decimal>("AverageRating")
                        .HasColumnType("numeric");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("Coordinates")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Drink")
                        .HasColumnType("boolean");

                    b.Property<bool>("Food")
                        .HasColumnType("boolean");

                    b.Property<int>("VenueCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("WheelchairAccess")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("VenueCategoryId");

                    b.ToTable("Venues", "public");
                });

            modelBuilder.Entity("Backend.Models.VenueCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Descr")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.HasKey("Id");

                    b.ToTable("VenueCategories", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Bar",
                            Descr = "Drinking establishment"
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Theatre",
                            Descr = "Area for dramatic performances"
                        });
                });

            modelBuilder.Entity("Backend.Models.VenueMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Media")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("VenueMedias", "public");
                });

            modelBuilder.Entity("Backend.Models.VenueReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VenueId");

                    b.ToTable("VenueReviews", "public");
                });

            modelBuilder.Entity("Backend.Models.Attending", b =>
                {
                    b.HasOne("Backend.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.Event", b =>
                {
                    b.HasOne("Backend.Models.EventCategory", "EventCategory")
                        .WithMany()
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventCategory");

                    b.Navigation("User");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("Backend.Models.EventMedia", b =>
                {
                    b.HasOne("Backend.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Backend.Models.EventReview", b =>
                {
                    b.HasOne("Backend.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.Venue", b =>
                {
                    b.HasOne("Backend.Models.VenueCategory", "VenueCategory")
                        .WithMany()
                        .HasForeignKey("VenueCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VenueCategory");
                });

            modelBuilder.Entity("Backend.Models.VenueMedia", b =>
                {
                    b.HasOne("Backend.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("Backend.Models.VenueReview", b =>
                {
                    b.HasOne("Backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Venue");
                });
#pragma warning restore 612, 618
        }
    }
}
