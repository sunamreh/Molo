﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Molo.Infrastructure.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Molo.Infrastructure.Migrations
{
    [DbContext(typeof(MoloDbContext))]
    partial class MoloDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Molo.Domain.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Msisdn")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("SubscriberId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SubscriberId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Molo.Domain.Entities.InterestRate", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("InterestRate");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Description = "5 Percent",
                            Percentage = 5m
                        },
                        new
                        {
                            Id = (byte)2,
                            Description = "10 Percent",
                            Percentage = 10m
                        },
                        new
                        {
                            Id = (byte)3,
                            Description = "15 Percent",
                            Percentage = 15m
                        },
                        new
                        {
                            Id = (byte)4,
                            Description = "20 Percent",
                            Percentage = 20m
                        },
                        new
                        {
                            Id = (byte)5,
                            Description = "25 Percent",
                            Percentage = 25m
                        },
                        new
                        {
                            Id = (byte)6,
                            Description = "30 Percent",
                            Percentage = 30m
                        });
                });

            modelBuilder.Entity("Molo.Domain.Entities.Loan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<DateTimeOffset>("CollectionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<byte>("InterestRateId")
                        .HasPrecision(18, 2)
                        .HasColumnType("smallint");

                    b.Property<bool>("IsSettled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("SettlementDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SubscriberClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriberId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InterestRateId");

                    b.HasIndex("SubscriberClientId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("Loan");
                });

            modelBuilder.Entity("Molo.Domain.Entities.Subscriber", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("DateInactive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Msisdn")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<DateTimeOffset>("SubscriptionDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Subscriber");
                });

            modelBuilder.Entity("Molo.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriberClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriberId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriberLoanId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TransactionAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.Property<byte>("TransactionTypeId")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("SubscriberClientId");

                    b.HasIndex("SubscriberId");

                    b.HasIndex("SubscriberLoanId");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Molo.Domain.Entities.TransactionType", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("TransactionType");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Description = "Credit"
                        },
                        new
                        {
                            Id = (byte)2,
                            Description = "Debit"
                        });
                });

            modelBuilder.Entity("Molo.Domain.Entities.Client", b =>
                {
                    b.HasOne("Molo.Domain.Entities.Subscriber", "Subscriber")
                        .WithMany("Clients")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("Molo.Domain.Entities.Loan", b =>
                {
                    b.HasOne("Molo.Domain.Entities.InterestRate", "InterestRate")
                        .WithMany()
                        .HasForeignKey("InterestRateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Molo.Domain.Entities.Client", "SubscriberClient")
                        .WithMany("Loans")
                        .HasForeignKey("SubscriberClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Molo.Domain.Entities.Subscriber", "Subscriber")
                        .WithMany("Loans")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InterestRate");

                    b.Navigation("Subscriber");

                    b.Navigation("SubscriberClient");
                });

            modelBuilder.Entity("Molo.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("Molo.Domain.Entities.Client", "SubscriberClient")
                        .WithMany()
                        .HasForeignKey("SubscriberClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Molo.Domain.Entities.Subscriber", "Subscriber")
                        .WithMany("Transactions")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Molo.Domain.Entities.Loan", "SubscriberLoan")
                        .WithMany()
                        .HasForeignKey("SubscriberLoanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Molo.Domain.Entities.TransactionType", "TransactionType")
                        .WithMany()
                        .HasForeignKey("TransactionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");

                    b.Navigation("SubscriberClient");

                    b.Navigation("SubscriberLoan");

                    b.Navigation("TransactionType");
                });

            modelBuilder.Entity("Molo.Domain.Entities.Client", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Molo.Domain.Entities.Subscriber", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Loans");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
